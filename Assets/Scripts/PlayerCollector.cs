using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    // Wordt geactiveerd wanneer een object met een Trigger de cirkel rondom de speler binnentreedt.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Controleert via de Tag of het object daadwerkelijk een ervaringspunt (Gem) is.
        if (collision.CompareTag("Gem"))
        {
            // Activeert de 'magnetische' logica in het Gem-script zodat deze naar de speler toe beweegt.
            collision.GetComponent<ExperienceGem>().StartFollowing(transform);
        }
    }
}