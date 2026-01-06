using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    [Header("XP System")]
    public XPBar xpBar;
    public int currentXP = 0;
    public int nextLevelXP = 100;
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
        if (xpBar != null) xpBar.SetMaxXP(nextLevelXP);
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
    public void GainExperience(int amount)
    {
        currentXP += amount;

        // 3. Update de visuele balk
        if (xpBar != null) xpBar.SetXP(currentXP);

        if (currentXP >= nextLevelXP)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        // Bereken hoeveel XP we 'over' hebben voor het volgende level
        int leftoverXP = currentXP - nextLevelXP;

        currentXP = (leftoverXP > 0) ? leftoverXP : 0;
        nextLevelXP = Mathf.RoundToInt(nextLevelXP * 1.5f);

        if (xpBar != null)
        {
            xpBar.SetMaxXP(nextLevelXP);
            xpBar.SetXP(currentXP); // Zet hem op de overgebleven XP ipv 0
        }

        Debug.Log("LEVEL UP! Nieuw doel: " + nextLevelXP);
    }

    void Die()
    {
        Debug.Log("Player has died.");
        // Hier kan je animatie of respawn logica toevoegen
    }
}
