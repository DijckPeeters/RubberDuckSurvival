using UnityEngine;

public class BombWeapon : MonoBehaviour
{
    public GameObject bombPrefab;
    public float dropInterval = 5f;
    private float timer;
    public bool isUnlocked = false;

    [Header("Upgradeble Stats")]
    public float explosionRadius = 3f;
    public float bombDamage = 50f;
    public float visualExplosionScale = 1f; // NIEUW: Dit lost je error op!

    void Update()
    {
        if (!isUnlocked) return;

        timer += Time.deltaTime;
        if (timer >= dropInterval)
        {
            DropBomb();
            timer = 0;
        }
    }

    void DropBomb()
    {
        GameObject newBomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);

        // Geef alle stats door aan de bom
        Bomb bombScript = newBomb.GetComponent<Bomb>();
        if (bombScript != null)
        {
            bombScript.explosionRadius = explosionRadius;
            bombScript.damage = bombDamage;

            // Pas de visuele grootte van de bom/explosie aan
            newBomb.transform.localScale = Vector3.one * visualExplosionScale;
        }
    }
}