using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject levelUpMenu;
    public Player player;

    [Header("Weapon Settings")]
    public GameObject weaponObject; // Hier sleep je morgen je wapen in!

    public void OpenMenu()
    {
        levelUpMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void UpgradeHealth()
    {
        player.maxHealth += 20;
        player.currentHealth = player.maxHealth;
        if (player.healthBar != null) player.healthBar.SetMaxHealth(player.maxHealth);
        CloseMenu();
    }

    public void UpgradeDamage()
    {
        Debug.Log("Damage Upgraded!");
        CloseMenu();
    }

    public void UpgradeSize()
    {
        // We zoeken het script op het wapen-object dat je hebt gesleept
        // Verander 'SwordWeapon' naar de exacte naam van jouw wapen-script!
        var weaponScript = weaponObject.GetComponent<SwordWeapon>();

        if (weaponScript != null)
        {
            // We maken het wapen groter via de variabele in jouw script
            // Ik gok dat de variabele 'weaponSize' of 'scale' heet
            weaponObject.transform.localScale *= 1.3f;

            Debug.Log("Wapen script succesvol aangepast!");
        }

        CloseMenu();
    }

    public void CloseMenu()
    {
        levelUpMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}