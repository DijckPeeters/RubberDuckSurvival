using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed = 5f;
    public float damage = 10f;
    public float damageCooldown = 1f;
    public GameObject gemPrefab;
    private float lastDamageTime = 0f;
    private Rigidbody2D rb;
    private Transform target;
    private EnemyPooler originPool;

    public void SetPool(EnemyPooler pool) => originPool = pool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Voorkomt tollen
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) target = playerObj.transform;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            Vector2 newPos = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
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
        // 1. Altijd eerst de orb spawnen op de huidige positie
        if (gemPrefab != null)
        {
            Instantiate(gemPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Geen gemPrefab gevonden op " + gameObject.name);
        }

        // 2. Daarna de vijand wegsturen of vernietigen
        if (originPool != null)
        {
            originPool.ReleaseEnemy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}