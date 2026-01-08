using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private Vector2 lastMoveInput;
    private float knockbackTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (knockbackTimer > 0) knockbackTimer -= Time.deltaTime;

        // Update animator parameters
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
        animator.SetBool("isWalking", moveInput.sqrMagnitude > 0);

        // Update last direction voor idle animations
        if (moveInput.sqrMagnitude > 0)
        {
            lastMoveInput = moveInput;
            animator.SetFloat("LastInputX", lastMoveInput.x);
            animator.SetFloat("LastInputY", lastMoveInput.y);
        }
    }

    void FixedUpdate()
    {
        if (knockbackTimer <= 0)
        {
            // Gebruik velocity in plaats van linearVelocity als je een oudere Unity versie hebt, 
            // of blijf bij linearVelocity in Unity 6, maar zorg dat we de waarde direct zetten:
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    public void ApplyKnockback() => knockbackTimer = 0.25f;

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}