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
        // Telt de knockback tijd af zodat de speler na een klap weer controle krijgt over de beweging.
        if (knockbackTimer > 0) knockbackTimer -= Time.deltaTime;

        // Stuurt de bewegingswaarden naar de Animator om te wisselen tussen Idle en Walking.
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
        animator.SetBool("isWalking", moveInput.sqrMagnitude > 0);

        // Onthoudt de laatste looprichting zodat de speler in de juiste richting blijft kijken bij stilstand.
        if (moveInput.sqrMagnitude > 0)
        {
            lastMoveInput = moveInput;
            animator.SetFloat("LastInputX", lastMoveInput.x);
            animator.SetFloat("LastInputY", lastMoveInput.y);
        }
    }

    void FixedUpdate()
    {
        // Past de fysieke snelheid alleen aan als de speler niet wordt weggeslagen door een vijand.
        if (knockbackTimer <= 0)
        {
            // Gebruikt linearVelocity (Unity 6) om een constante snelheid te garanderen zonder vertraging.
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    // Blokkeert tijdelijk de speler-input om de fysieke impact van een vijand te verwerken.
    public void ApplyKnockback() => knockbackTimer = 0.25f;

    // Wordt aangeroepen door het InputSystem wanneer de joystick of WASD-toetsen worden gebruikt.
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}