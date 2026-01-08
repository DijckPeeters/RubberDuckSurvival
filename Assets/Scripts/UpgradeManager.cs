using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject levelUpMenu;
    public Player player;

    [Header("Weapon Settings")]
    public GameObject weaponObject;

    // Pauzeert de game en opent de UI-interface wanneer de speler een level stijgt.
    public void OpenMenu()
    {
        levelUpMenu.SetActive(true);
        Time.timeScale = 0f; // Bevriest de hele game-wereld.
    }

    // Verhoogt de maximale gezondheid en vult deze direct volledig aan.
    public void UpgradeHealth()
    {
        player.maxHealth += 20;
        player.currentHealth = player.maxHealth;
        // Werkt de UI-balk direct bij naar de nieuwe schaal.
        if (player.healthBar != null) player.healthBar.SetMaxHealth(player.maxHealth);
        CloseMenu();
    }

    public void UpgradeDamage()
    {
        // Toekomstige functie om de schade-variabele in het wapen-script te verhogen.
        Debug.Log("Damage Upgraded!");
        CloseMenu();
    }

    public void UpgradeSize()
    {
        if (weaponObject != null)
        {
            // Vergroot de transform-scale met 25%, wat automatisch ook de hitbox-berekening vergroot.
            weaponObject.transform.localScale *= 1.25f;
            Debug.Log("Wapen en Hitbox zijn nu groter!");
        }
        else
        {
            Debug.LogWarning("Geen weaponObject toegewezen in de UpgradeManager!");
        }

        CloseMenu();
    }

    // Sluit het menu en zet de tijd weer op normale snelheid om verder te spelen.
    public void CloseMenu()
    {
        levelUpMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}