using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public float scrollSpeed = 0.00000000001f;
    private MeshRenderer meshRenderer;
    private Transform playerTransform;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        // Zoek je speler op basis van de tag die je al gebruikt
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Verplaats de Quad mee met de speler zodat hij altijd onder de speler blijft
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, 10f);

            // Verschuif de texture in de tegenovergestelde richting van de beweging
            Vector2 offset = new Vector2(playerTransform.position.x * scrollSpeed, playerTransform.position.y * scrollSpeed);
            meshRenderer.material.mainTextureOffset = offset;
        }
    }
}