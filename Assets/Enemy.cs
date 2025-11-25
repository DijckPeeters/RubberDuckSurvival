using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed = 5f;          // snelheid van de enemy
    public float damage = 10f;            // schade per hit
    public float damageCooldown = 1f;     // tijd tussen schade

    private float lastDamageTime = 0f;

    private Rigidbody2D rb;
    private Transform target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Zoek de Player via tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            // Direction berekenen in 2D (Vector2)
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;

            // Vloeiend bewegen met MovePosition
            Vector2 newPos = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Periodieke schade en knockback toepassen op Player
        if (collision.collider.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                Player player = collision.collider.GetComponent<Player>();
                if (player != null)
                {
                    // Richting van Enemy naar Player
                    Vector2 knockDir = ((Vector2)collision.collider.transform.position - rb.position).normalized;
                    player.TakeDamage((int)damage, knockDir);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}
