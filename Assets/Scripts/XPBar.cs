using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public Slider slider;

    // Wordt aangeroepen bij een level-up om de nieuwe drempelwaarde voor het volgende level in te stellen.
    public void SetMaxXP(int xp)
    {
        slider.maxValue = xp;
        slider.value = 0; // Reset de visuele voortgangsbalk naar het beginpunt.
    }

    // Werkt de huidige stand van de balk bij wanneer de speler een Experience Gem oppakt.
    public void SetXP(int xp)
    {
        slider.value = xp;
    }
}