using UnityEngine;
using UnityEngine.Pool;

public class EnemyPooler : MonoBehaviour
{
    public GameObject enemyPrefab;
    public IObjectPool<GameObject> Pool;

    void Awake()
    {
        Pool = new ObjectPool<GameObject>(
            createFunc: () => {
                GameObject go = Instantiate(enemyPrefab);
                go.GetComponent<Enemy>().SetPool(this); // Geef de pool mee aan de enemy
                return go;
            },
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            defaultCapacity: 100,
            maxSize: 1000
        );
    }

    public GameObject GetEnemy() => Pool.Get();
    public void ReleaseEnemy(GameObject enemy) => Pool.Release(enemy);
}