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
    [SerializeField]
    private float movementSpeed = 500.0f;
    [SerializeField]
    private float jumpHeight = 10.0f;

    private float horizontalMovement; 

    private int jumpsToUse = 2;

    private Rigidbody2D rb;
    private PunchSystem punch;

    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * movementSpeed, rb.linearVelocity.y);
    }

    public void Move(InputAction.CallbackContext action)
    {
        //^read inputs
        //code in what to do with the input
        //its in vector2 so it should be easier for you

        horizontalMovement = action.ReadValue<Vector2>().x;

    }

    public void Jump(InputAction.CallbackContext action) 
    {
        //^read jump input
        //code in what to do with the input
        //i assume its on pressed so..

        
        if (action.started) { 
        
            if(jumpsToUse > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
                jumpsToUse --;
            }
        }

    }

    public void LeftPunch(InputAction.CallbackContext action)
    {
        if (action.started) {

            punch?.ChargePunch(Hand.Left);
            Debug.Log("charge left");
        }
        else
        {
            punch?.ReleaseCharge(Hand.Left);
        }

    }

    public void RightPunch(InputAction.CallbackContext action)
    {
        if (action.started)
        {

            punch?.ChargePunch(Hand.Right);
            Debug.Log("charge right");

        }
        else
        {
            punch?.ReleaseCharge(Hand.Right);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsToUse = 2;
        }
    }

}
