using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    // De scrollSpeed bepaalt de gevoeligheid van de achtergrondbeweging. 
    // Een lage waarde zorgt ervoor dat het gras niet te snel onder de voeten wegglijdt.
    public float scrollSpeed = 0.00000000001f;
    private MeshRenderer meshRenderer;
    private Transform playerTransform;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        // Dynamische koppeling aan de speler zodat de achtergrond altijd meebeweegt.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // De Quad wordt exact op de positie van de speler geplaatst (met een offset op de Z-as).
            // Hierdoor verlaat de speler visueel nooit het midden van het grasveld.
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, 10f);

            // In plaats van het object te bewegen, verschuiven we de Texture op het materiaal (UV-offset).
            // Door de speler-positie te vermenigvuldigen met scrollSpeed, creëren we de illusie 
            // van beweging in de tegenovergestelde richting.
            Vector2 offset = new Vector2(playerTransform.position.x * scrollSpeed, playerTransform.position.y * scrollSpeed);

            // mainTextureOffset werkt alleen correct als de Texture Wrap Mode op 'Repeat' staat.
            meshRenderer.material.mainTextureOffset = offset;
        }
    }
}