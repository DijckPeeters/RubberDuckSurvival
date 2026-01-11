using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject bombPrefab;
    public float dropInterval = 3f;

    [Header("Upgrade Stats")]
    public float explosionRadius = 4f;
    public float visualScale = 1f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= dropInterval)
        {
            DropBomb();
            timer = 0;
        }
    }

    void DropBomb()
    {
        if (bombPrefab == null) return;

        // De 'null' aan het einde zorgt dat de bom GEEN parent krijgt (dus niet aan de player plakt)
        GameObject newBomb = Instantiate(bombPrefab, transform.position, Quaternion.identity, null);

        // De rest van je logica...
        BombWeapon bombScript = newBomb.GetComponent<BombWeapon>();
        if (bombScript != null)
        {
            bombScript.explosionRadius = explosionRadius;
            bombScript.visualExplosionScale = visualScale;
        }
    }
}