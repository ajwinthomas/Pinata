using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 7f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isJumpPressed;
    private bool isGrounded;
    


    [Header("Ground Checking")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Ground check with overlapcircle
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);

        //Handle jump input
        if(isJumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
            isJumpPressed = false;
        }

        

        

    }



    private void FixedUpdate()
    {
        //Apply horizontal movement
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed,rb.linearVelocity.y);
    }

    //--------InputSystem Callbacks----------

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isJumpPressed=true;
        }
        else if(context.canceled)
        {
            isJumpPressed=false;
        }
    }

   

    //Debugging aid 

    private void OnDrawGizmosSelected()
    {
        if(groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
        }
    }


}
