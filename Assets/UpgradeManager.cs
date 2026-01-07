using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject levelUpMenu;
    public Player player;

    [Header("Weapon Settings")]
    public GameObject weaponObject; // Sleep hier je 'WeaponHolder' of 'Sword' in

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
        // Als je een wapen script hebt, kun je hier damage++ doen
        Debug.Log("Damage Upgraded!");
        CloseMenu();
    }

    public void UpgradeSize()
    {
        if (weaponObject != null)
        {
            // Dit vergroot het HELE object (Sprite + Hitbox + Animatie)
            weaponObject.transform.localScale *= 1.25f;
            Debug.Log("Wapen en Hitbox zijn nu groter!");
        }
        else
        {
            Debug.LogWarning("Geen weaponObject toegewezen in de UpgradeManager!");
        }

        CloseMenu();
    }

    public void CloseMenu()
    {
        levelUpMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}