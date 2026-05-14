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

    private Vector3 weaponOffset;

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

        weaponOffset = swordData.weaponOffset;

    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameplay) return;
        
        float targetSpeed = horizontalMovement * movementSpeed;

        Vector2 velocity = rb.linearVelocity;

        velocity.x = Mathf.Lerp(
            velocity.x,
            targetSpeed,
            12f * Time.fixedDeltaTime
        );

        rb.linearVelocity = velocity;

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
            EquipWeapon(swordData);
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
                if (leftFist.GetFistFull())
                {
                    leftFist.GetWeapon().BeginCharge();
                }

            }
            if (action.canceled)
            {

                punch?.ReleaseCharge(Hand.Left);
                if (leftFist.GetFistFull())
                {
                    leftFist.GetWeapon().Use();
                }
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
                if(rightFist.GetFistFull())
                {
                    rightFist.GetWeapon().BeginCharge();
                }
            }

            if (action.canceled)
            {
                punch?.ReleaseCharge(Hand.Right);
                if (rightFist.GetWeapon())
                {
                    rightFist.GetWeapon().Use();
                }
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
        weaponObject.transform.localPosition =  weaponOffset;
        weaponObject.transform.localRotation = Quaternion.identity;

        BaseWeapon weapon = weaponObject.GetComponent<BaseWeapon>();

        weapon.Initialize(weaponData);
        fist.SetWeapon(weapon);

        MeleeWeapon meleeWeapon = weapon as MeleeWeapon;
        if (meleeWeapon != null)
        {
            meleeWeapon.SetArmPivot(fist.transform);
        }


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

    private void SetWeaponOffset(Vector3 offset)
    {
        weaponOffset = offset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsToUse = 2;
        }
    }

}
