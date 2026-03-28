using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float fuseTime = 2f;
    public float explosionRadius = 3f;
    public float damage = 50f;
    public GameObject explosionEffect;

    private bool hasExploded = false; // Zorgt dat de bom maar 1x afgaat

    void Update()
    {
        // Tel de tijd af
        fuseTime -= Time.deltaTime;

        // Als de tijd op is EN hij is nog niet ontploft...
        if (fuseTime <= 0 && !hasExploded)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded) return; // Beveiliging: maar 1x knallen
        hasExploded = true;

        // --- VISUELE EXPLOSIE FIX ---
        // Laat de visuele explosie zien (vlammen)
        if (explosionEffect != null)
        {
            // 1. We spawnen de explosie en slaan hem even op in 'exp'
            GameObject exp = Instantiate(explosionEffect, transform.position, Quaternion.identity);

            // 2. We dwingen de explosie om PRECIES DEZELFDE grootte te worden als de bom!
            // transform.localScale is de huidige grootte van de bom (Idle).
            exp.transform.localScale = transform.localScale;
        }

        // --- SCHADE LOGICA ---
        // Zoek alles in de buurt
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D col in objectsInRange)
        {
            if (col.CompareTag("Enemy"))
            {
                // Roep de schade functie van je vijand aan
                col.GetComponent<Enemy>()?.TakeDamage(damage);
            }
        }

        // --- OPRUIMEN ---
        Destroy(gameObject); // Verwijder de bom zelf
    }

    // Teken een rode cirkel in de Scene-view zodat je de grootte kunt zien
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}