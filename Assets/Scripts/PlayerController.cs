using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Move(InputAction.CallbackContext action)
    {
        //^read inputs
        //code in what to do with the input
        //its in vector2 so it should be easier for you
        Debug.Log("MOVEMENT");
    }

    public void Jump(InputAction.CallbackContext action) 
    {
        //^read jump input
        //code in what to do with the input
        //i assume its on pressed so..
        Debug.Log("Jump");

    }


    void Update()
    {
        
    }
}
