using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private float knockbackTimer;

    private Vector3 originalScale;

    private float lastX = 0;
    private float lastY = -1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // ... rest van je update code (timers, etc.) ...

        // Check of we echt bewegen (sqrMagnitude is sneller dan Magnitude)
        bool isWalking = moveInput.sqrMagnitude > 0.01f;
        animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            // Update de richting voor de Blend Tree
            lastX = moveInput.x;
            lastY = moveInput.y;

            // --- DE FLIP LOGICA ---
            // We veranderen de scale ALLEEN als we naar links of rechts drukken.
            // Als we stoppen (isWalking = false), komt hij hier niet en blijft de schaal zoals hij was!
            if (moveInput.x > 0.1f)
            {
                // Kijk naar Rechts: Forceer positieve X
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else if (moveInput.x < -0.1f)
            {
                // Kijk naar Links: Forceer negatieve X
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
        }
        float animatorX = Mathf.Abs(moveInput.x);
        float animatorY = moveInput.y;

        float lastAnimatorX = Mathf.Abs(lastX);
        float lastAnimatorY = lastY;

        // Stuur de aangepaste waarden naar de animator
        animator.SetFloat("InputX", animatorX);
        animator.SetFloat("InputY", animatorY);
        animator.SetFloat("LastInputX", lastAnimatorX);
        animator.SetFloat("LastInputY", lastAnimatorY);
    }

    void FixedUpdate()
    {
        if (knockbackTimer <= 0)
        {
            // Gebruik rb.velocity als rb.linearVelocity een error geeft (afhankelijk van je Unity versie)
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    public void ApplyKnockback() => knockbackTimer = 0.25f;

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}