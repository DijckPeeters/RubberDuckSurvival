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
    public UpgradeManager upgradeManager;

    [Header("Score System")]
    public ScoreManager scoreManager; // Sleep je ScoreManager hierin!

    [Header("Components")]
    public HealthBar healthBar;
    public SpriteRenderer spriteRenderer;
    public float flashDuration = 0.2f;
    public float knockbackForce = 20f;

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
        if (healthBar != null) healthBar.SetMaxHealth(maxHealth);
        if (xpBar != null) xpBar.SetMaxXP(nextLevelXP);
    }

    public void GainExperience(int amount)
    {
        currentXP += amount;
        if (xpBar != null) xpBar.SetXP(currentXP);

        // Geef ook punten voor de score!
        if (scoreManager != null) scoreManager.AddScore(amount);

        if (currentXP >= nextLevelXP)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentXP = 0;
        nextLevelXP = Mathf.RoundToInt(nextLevelXP * 1.5f);

        if (xpBar != null)
        {
            xpBar.SetMaxXP(nextLevelXP);
            xpBar.SetXP(0);
        }

        if (upgradeManager != null) upgradeManager.OpenMenu();
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        currentHealth -= damage;
        if (healthBar != null) healthBar.SetHealth(currentHealth);
        StartCoroutine(FlashRed());
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
        Debug.Log("Player Dead");
        if (scoreManager != null)
        {
            scoreManager.HandleGameOver();
        }
    }
}