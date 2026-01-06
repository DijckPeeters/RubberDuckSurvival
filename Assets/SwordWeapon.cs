using UnityEngine;

public class SwordWeapon : MonoBehaviour
{
    [Header("Settings")]
    public float damage = 20f;
    public float attackSpeed = 1.5f; // Seconden tussen attacks
    public float range = 3f;
    public Vector2 slashSize = new Vector2(2f, 1f);

    [Header("Visuals")]
    public GameObject slashPrefab; // Een simpel wit streepje of sprite

    private float timer;
    private bool slashRight = true;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= attackSpeed)
        {
            Attack();
            timer = 0;
        }
    }

    void Attack()
    {
        float side = slashRight ? 1f : -1f;
        Vector2 spawnPos = (Vector2)transform.position + new Vector2(side * 1.5f, 0);

        // --- VISUEEL EFFECT ---
        if (slashPrefab != null)
        {
            // Spawn de witte streep
            GameObject slash = Instantiate(slashPrefab, spawnPos, Quaternion.identity);
            // Verwijder de streep automatisch na 0.1 seconde (flits)
            Destroy(slash, 0.1f);
        }

        // --- DAMAGE LOGICA ---
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(spawnPos, slashSize, 0);
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Enemy enemyScript = enemyCollider.GetComponentInParent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Die();
            }
        }

        slashRight = !slashRight;
    }
    // Teken de range in de editor zodat je het kunt zien
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        float side = slashRight ? 1f : -1f;
        Gizmos.DrawWireCube((Vector2)transform.position + new Vector2(side * 1.5f, 0), slashSize);
    }
}