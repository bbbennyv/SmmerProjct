using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public enum Hand
{
    Left, 
    Right
}

public class PlayerController : MonoBehaviour
{

    [Header("Movement Config")]
    [SerializeField]
    private float movementSpeed = 500.0f;
    [SerializeField]
    private float jumpHeight = 10.0f;

    [SerializeField]
    private float dashAmount = 20.0f;
    [SerializeField]
    private float dashCooldown = 1.0f;

    private bool isDashing; 
    private float horizontalMovement;

    private int jumpsToUse = 2;

    private Rigidbody2D rb;
    private PunchSystem punch;

    private FistController leftFist;
    private FistController rightFist;

    public WeaponData swordData;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        punch = GetComponent<PunchSystem>();

        /*       GameManager.Instance.spawnedPlayers.Add(this);
               if (!GameManager.Instance.alivePlayers.Contains(this))
               {
                   GameManager.Instance.alivePlayers.Add(this);
               }
       */

        leftFist = punch.GetLeftFist();
        rightFist = punch.GetRightFist();

    }

    private void FixedUpdate()
    {

        if (GameManager.Instance.IsGameplay)
        {
            float targetSpeed = horizontalMovement * movementSpeed;
            float speedDiff = targetSpeed - rb.linearVelocity.x;

            rb.AddForce(new Vector2(speedDiff * 10f, 0f));
        }
    }

    public void Move(InputAction.CallbackContext action)
    {
        horizontalMovement = action.ReadValue<Vector2>().x;

    }

    public void Jump(InputAction.CallbackContext action)
    {

        if (action.started && GameManager.Instance.IsGameplay) {

            if (jumpsToUse > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
                jumpsToUse--;
            }
        }

    }

    public void Dash(InputAction.CallbackContext action)
    {
        if (action.started && !isDashing && GameManager.Instance.IsGameplay)
        {
            StartCoroutine(DashAction());
        }
    }

    private IEnumerator DashAction()
    {
        isDashing = true;
        rb.AddForce(new Vector2(rb.linearVelocity.x * dashAmount, rb.linearVelocity.y), ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }

    public void LeftPunch(InputAction.CallbackContext action)
    {
        if (GameManager.Instance.IsGameplay)
        {

            if (action.started)
            {

                punch?.ChargePunch(Hand.Left);

            }
            if (action.canceled)
            {

                punch?.ReleaseCharge(Hand.Left);
            }
        }
    }

    public void RightPunch(InputAction.CallbackContext action)
    {
        if (GameManager.Instance.IsGameplay)
        {

            if (action.started)
            {

                punch?.ChargePunch(Hand.Right);
                EquipWeapon(swordData);

            }

            if (action.canceled)
            {
                punch?.ReleaseCharge(Hand.Right);
            }
        }
    }

    public void EquipWeapon(WeaponData weaponData)
    {

        if (weaponData != null) { } ;


        if (!rightFist.GetFistFull())
        {
            SpawnWeaponInHand(rightFist, weaponData);
            rightFist.SetFistFull(true);
            return;
        }

        if (!leftFist.GetFistFull())
        {
            SpawnWeaponInHand(leftFist, weaponData);
            leftFist.SetFistFull(true);

            return;
        }

        RemoveWeaponInHand(rightFist);

        SpawnWeaponInHand(rightFist, weaponData);


    }

    private void SpawnWeaponInHand(FistController fist, WeaponData weaponData)
    {
        GameObject weaponObject = Instantiate(weaponData.weaponPrefab, fist.transform.position, fist.transform.rotation);

        weaponObject.transform.SetParent(fist.transform);
        weaponObject.transform.rotation = Quaternion.identity;

        BaseWeapon weapon = weaponObject.GetComponent<BaseWeapon>();

        fist.SetWeapon(weapon);
    }

    private void RemoveWeaponInHand(FistController fist)
    {
        BaseWeapon weapon = fist.GetWeapon();

        if (weapon != null) 
        {
            Destroy(weapon.gameObject);
        }

        fist.SetWeapon(null);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsToUse = 2;
        }
    }

}
