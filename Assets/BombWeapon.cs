using System.Collections;
using UnityEngine;

public class BombWeapon : MonoBehaviour
{
    public float damage = 50f;
    public float explosionRadius = 4f;
    public float visualExplosionScale = 2f;
    public float fuseTime = 2f;

    void Start()
    {
        // De bom begint als klein object en wacht fuseTime
        Invoke("StartExplosion", fuseTime);
    }

    void StartExplosion()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            // Maak de bom visueel groter voor de explosie-animatie
            transform.localScale = new Vector3(visualExplosionScale, visualExplosionScale, 1f);
            anim.SetTrigger("ExplodeNow");
        }
        StartCoroutine(DealDamageOverTime());
    }

    IEnumerator DealDamageOverTime()
    {
        float duration = 0.5f; // De hitbox blijft een halve seconde actief
        float timer = 0f;

        while (timer < duration)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D enemy in hitEnemies)
            {
                // Vervang 'Enemy' door de naam van jouw vijand-script
                if (enemy.GetComponent<Enemy>() != null)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(damage * Time.deltaTime * 5);
                }
            }
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}