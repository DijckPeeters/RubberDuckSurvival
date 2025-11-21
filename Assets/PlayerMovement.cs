using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private Vector2 lastMoveInput; // Keeps the last non-zero direction

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Update animator parameters every frame
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
        animator.SetBool("isWalking", moveInput != Vector2.zero);

        // Update last non-zero direction for idle animation
        if (moveInput != Vector2.zero)
        {
            lastMoveInput = moveInput;
            animator.SetFloat("LastInputX", lastMoveInput.x);
            animator.SetFloat("LastInputY", lastMoveInput.y);
        }
    }

    void FixedUpdate()
    {
        // Apply movement in FixedUpdate for smoother physics
        rb.linearVelocity = moveInput * moveSpeed;
    }

    // Called by the Input System
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
