using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPooler enemyPool;
    public Transform player;
    public float baseSpawnInterval = 2f;
    public float difficultyScaling = 0.05f;
    public float spawnDistance = 15f;

    private float timer;

    void Update()
    {
        if (player == null) return;

        timer += Time.deltaTime;
        float currentInterval = baseSpawnInterval / (1 + (Time.timeSinceLevelLoad * difficultyScaling));

        if (timer >= currentInterval)
        {
            SpawnEnemy();
            timer = 0;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPool == null) return;

        GameObject enemy = enemyPool.GetEnemy();

        // Bereken spawn positie
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnDistance;

        // Forceer positie en reset physics
        enemy.transform.position = new Vector3(spawnPos.x, spawnPos.y, 0);

        if (enemy.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = Vector2.zero;
        }

        Debug.Log("Enemy gespawned op: " + spawnPos);
    }
}