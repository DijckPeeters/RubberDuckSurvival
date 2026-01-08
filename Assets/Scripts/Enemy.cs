using UnityEngine;
using System.Collections; // Nodig voor Coroutines

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed = 5f;
    public float damage = 10f;
    public float damageCooldown = 1f;
    public float baseHealth = 20f;
    public GameObject gemPrefab;

    [Header("Visual Effects")]
    public Color flashColor = Color.red; // De kleur van de flash
    public float flashDuration = 0.1f;    // Hoe lang de flash duurt

    private float currentHealth;
    private float lastDamageTime = 0f;
    private Rigidbody2D rb;
    private Transform target;
    private EnemyPooler originPool;

    // Nieuwe variabelen voor de flash
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashCoroutine;

    public void SetPool(EnemyPooler pool) => originPool = pool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        // Pak de SpriteRenderer op het moment dat de enemy wordt aangemaakt
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void OnEnable()
    {
        // Reset de kleur voor het geval de enemy uit de pool komt terwijl hij nog rood was
        if (spriteRenderer != null) spriteRenderer.color = originalColor;

        float timeInMinutes = Time.timeSinceLevelLoad / 60f;
        currentHealth = baseHealth * (1f + (timeInMinutes * 0.2f));

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) target = playerObj.transform;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            // Draai de sprite op basis van de richting
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false; // Kijkt naar rechts
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true; // Kijkt naar links
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // Start de flash effect
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(HitFlash());

        if (currentHealth <= 0) Die();
    }

    // De Coroutine die de kleur verandert
    IEnumerator HitFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                Player player = collision.collider.GetComponent<Player>();
                if (player != null)
                {
                    Vector2 knockDir = ((Vector2)collision.collider.transform.position - rb.position).normalized;
                    player.TakeDamage((int)damage, knockDir);
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    public void Die()
    {
        if (gemPrefab != null) Instantiate(gemPrefab, transform.position, Quaternion.identity);

        if (originPool != null) originPool.ReleaseEnemy(gameObject);
        else Destroy(gameObject);
    }
}