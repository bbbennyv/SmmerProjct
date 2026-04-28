using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 500.0f;
    [SerializeField]
    private float jumpHeight = 10.0f;

    private float horizontalMovement; 
    private float verticalMovement;

    private bool isGrounded = true;

    private int jumpsToUse = 2;

    private Rigidbody2D rb;
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
    }

    public void Move(InputAction.CallbackContext action)
    {
        //^read inputs
        //code in what to do with the input
        //its in vector2 so it should be easier for you

        horizontalMovement = action.ReadValue<Vector2>().x;

        //rb.linearVelocity = new Vector2(horizontalMovement * movementSpeed, rb.linearVelocity.y);

        Debug.Log("MOVEMENT");
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

        Debug.Log("Jump");

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsToUse = 2;
        }
    }

    void Update()
    {

        rb.linearVelocity = new Vector2(horizontalMovement * movementSpeed, rb.linearVelocity.y);
        Debug.Log(isGrounded);
    }
}
