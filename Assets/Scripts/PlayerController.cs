using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start wordt eenmalig aangeroepen bij het opstarten van het script.
    void Start()
    {

    }

    // Update wordt elke frame uitgevoerd; ideaal voor het constant checken van input.
    void Update()
    {
        // Raadpleegt de Singleton-instantie van de InputController om te zien of de Jump-knop is ingedrukt.
        if (InputController.instance.Jump())
        {
            // Stuurt een bericht naar de Console ter bevestiging dat de input correct wordt geregistreerd.
            Debug.Log("Jump Key pressed");
        }
    }
}