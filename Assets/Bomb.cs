using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float fuseTime = 2f;
    public float explosionRadius = 3f; // Moet public zijn
    public float damage = 50f;         // Moet public zijn
    public GameObject explosionEffect; // Sleep hier een effect in (optioneel)

    void Start()
    {
        // Start de timer voor de explosie
        Invoke("Explode", fuseTime);
    }

    void Explode()
    {
        // 1. Visueel effect (optioneel)
        if (explosionEffect != null) Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // 2. Vind alle vijanden in de buurt
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Check of het een enemy is (pas de "Enemy" component naam aan naar jouw script)
            Enemy health = enemy.GetComponent<Enemy>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        // 3. Verwijder de bom
        Destroy(gameObject);
    }

    // Laat de radius zien in de Editor (handig voor afstellen!)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}