using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;
    public SpriteRenderer spriteRenderer;
    public float flashDuration = 0.2f;
    public float knockbackForce = 20f; // verhoogd voor duidelijke knockback

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        // Test: spatie om zelf damage te nemen
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10, Vector2.right);
        }
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        StartCoroutine(FlashRed());

        // Zorg dat knockback alleen Player beïnvloedt
        rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white;
    }

    void Die()
    {
        Debug.Log("Player has died.");
        // Hier kan je animatie of respawn logica toevoegen
    }
}
