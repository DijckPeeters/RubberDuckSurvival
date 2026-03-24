using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private float knockbackTimer;

    // Voor de Flip
    private Vector3 originalScale;

    // NIEUW: Voor de Blend Tree (moet exact matchen met de namen in je Animator!)
    private float lastX = 0;
    private float lastY = -1; // Standaard kijkt hij naar beneden (IdleDown)

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (knockbackTimer > 0) knockbackTimer -= Time.deltaTime;

        // 1. STUUR DATA NAAR DE ANIMATOR
        // We sturen de huidige beweging door voor de 'Walk' animaties
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

        // We sturen de laatst bekende richting door voor de 'Idle' animaties
        // Dit zorgt dat de Blend Tree weet welke kant je op moet kijken als je stopt.
        animator.SetFloat("LastInputX", lastX);
        animator.SetFloat("LastInputY", lastY);

        bool isWalking = moveInput.sqrMagnitude > 0;
        animator.SetBool("isWalking", isWalking);

        // 2. LOGICA VOOR RICHTING EN FLIP
        if (isWalking)
        {
            // Onthoud de richting voor de Blend Tree
            lastX = moveInput.x;
            lastY = moveInput.y;

            // Flip de sprite op basis van X (Links/Rechts)
            if (moveInput.x > 0)
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            }
            else if (moveInput.x < 0)
            {
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            }
        }
    }

    void FixedUpdate()
    {
        if (knockbackTimer <= 0)
        {
            rb.linearVelocity = moveInput * moveSpeed; // Unity 6
        }
    }

    public void ApplyKnockback() => knockbackTimer = 0.25f;

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}