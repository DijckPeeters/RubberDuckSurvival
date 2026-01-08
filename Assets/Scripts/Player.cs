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
    public ScoreManager scoreManager; // Koppeling naar het UI-systeem voor de scorepunten.

    [Header("Components")]
    public HealthBar healthBar;
    public SpriteRenderer spriteRenderer;
    public float flashDuration = 0.2f;
    public float knockbackForce = 20f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Haalt de physics-component op voor de knockback-krachten.
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        // Zet de UI-balkjes direct op de juiste beginwaarden.
        if (healthBar != null) healthBar.SetMaxHealth(maxHealth);
        if (xpBar != null) xpBar.SetMaxXP(nextLevelXP);
    }

    public void GainExperience(int amount)
    {
        currentXP += amount;
        if (xpBar != null) xpBar.SetXP(currentXP);

        // Synchroniseert de verdiende XP direct met de globale score.
        if (scoreManager != null) scoreManager.AddScore(amount);

        if (currentXP >= nextLevelXP)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentXP = 0;
        // Verhoogt de XP-eis voor het volgende level met 50% voor een progressieve moeilijkheid.
        nextLevelXP = Mathf.RoundToInt(nextLevelXP * 1.5f);

        if (xpBar != null)
        {
            xpBar.SetMaxXP(nextLevelXP);
            xpBar.SetXP(0);
        }

        // Pauzeert de game-loop en opent het keuzemenu voor upgrades.
        if (upgradeManager != null) upgradeManager.OpenMenu();
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        currentHealth -= damage;
        if (healthBar != null) healthBar.SetHealth(currentHealth);
        StartCoroutine(FlashRed()); // Start de visuele feedback in een aparte tijdlijn.

        // Past een korte, krachtige fysieke impuls toe in de richting weg van de vijand.
        rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Tijdelijke kleurverandering om de speler te waarschuwen dat hij geraakt is.
    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration); // Wacht het aantal seconden voor het terugzetten.
        spriteRenderer.color = Color.white;
    }

    void Die()
    {
        Debug.Log("Player Dead");
        if (scoreManager != null)
        {
            scoreManager.HandleGameOver(); // Activeert het game-over scherm en stopt de timer.
        }
    }
}