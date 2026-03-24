using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public GameObject levelUpMenu;
    public Player player;
    public GameObject weaponObject;
    public SwordWeapon swordWeapon;

    [Header("UI Elementen")]
    public TextMeshProUGUI[] buttonTexts; // Zorg dat hier 3 items in staan in de Inspector

    // De pool van beschikbare upgrades
    private List<string> upgradeOptions = new List<string> { "Health", "Size", "Speed", "Attack Speed", "Bomb" };

    public void OpenMenu()
    {
        if (levelUpMenu == null) return;

        levelUpMenu.SetActive(true);
        Time.timeScale = 0f; // Pauzeert de game
        PrepareRandomButtons();
    }

    void PrepareRandomButtons()
    {
        if (buttonTexts.Length == 0)
        {
            Debug.LogError("Sleep de Text objecten in de UpgradeManager lijst!");
            return;
        }

        // Maak een kopie van de lijst om unieke keuzes te maken per knop
        List<string> currentOptions = new List<string>(upgradeOptions);

        for (int i = 0; i < buttonTexts.Length; i++)
        {
            if (currentOptions.Count == 0) break;

            int randomIndex = Random.Range(0, currentOptions.Count);
            string chosenUpgrade = currentOptions[randomIndex];

            buttonTexts[i].text = chosenUpgrade;
            currentOptions.RemoveAt(randomIndex);
        }
    }

    public void SelectUpgrade(int buttonIndex)
    {
        if (buttonIndex < 0 || buttonIndex >= buttonTexts.Length)
        {
            Debug.LogError("Button Index " + buttonIndex + " is fout! Gebruik 0, 1 of 2.");
            return;
        }

        string upgradeType = buttonTexts[buttonIndex].text;
        Debug.Log("Gekozen upgrade: " + upgradeType);

        // Check welk type upgrade is gekozen
        if (upgradeType == "Health") UpgradeHealth();
        else if (upgradeType == "Size") UpgradeSize();
        else if (upgradeType == "Speed") UpgradeSpeed();
        else if (upgradeType == "Attack Speed") UpgradeAttackSpeed();
        else if (upgradeType == "Bomb") UnlockBomb();
        else if (upgradeType == "Bigger Bombs") UpgradeBombSize();

        CloseMenu();
    }

    // --- Upgrade Logica ---

    void UpgradeHealth()
    {
        player.maxHealth += 20;
        player.currentHealth = player.maxHealth;
        if (player.healthBar != null) player.healthBar.SetMaxHealth(player.maxHealth);
    }

    void UpgradeSize()
    {
        // Maakt de hitbox van je zwaard groter
        if (swordWeapon != null) swordWeapon.hitboxSize *= 1.2f;
    }

    void UpgradeSpeed()
    {
        player.GetComponent<PlayerMovement>().moveSpeed += 1.5f;
    }

    void UpgradeAttackSpeed()
    {
        // Aangezien we met knoppen werken, maken we de animatie sneller
        Animator anim = player.GetComponent<Animator>();
        if (anim != null)
        {
            float currentSpeed = anim.GetFloat("AttackSpeedMultiplier");
            if (currentSpeed == 0) currentSpeed = 1f; // Fallback
            anim.SetFloat("AttackSpeedMultiplier", currentSpeed * 1.2f);
        }
    }

    void UnlockBomb()
    {
        BombWeapon bombWeapon = player.GetComponent<BombWeapon>();
        if (bombWeapon != null)
        {
            bombWeapon.isUnlocked = true;
            Debug.Log("BOMMEN GEACTIVEERD!");

            // Vervang 'Bomb' door 'Bigger Bombs' in de lijst voor de volgende level up
            upgradeOptions.Remove("Bomb");
            upgradeOptions.Add("Bigger Bombs");
        }
    }

    void UpgradeBombSize()
    {
        BombWeapon bombWeapon = player.GetComponent<BombWeapon>();
        if (bombWeapon != null)
        {
            // We vergroten zowel de logische radius als de visuele schaal
            bombWeapon.explosionRadius *= 1.4f;
            bombWeapon.visualExplosionScale *= 1.4f; // Dit matcht nu met het script hierboven
            Debug.Log("BOM RADIUS EN VISUALS VERGROOT!");
        }
    }

    public void CloseMenu()
    {
        levelUpMenu.SetActive(false);
        Time.timeScale = 1f; // Hervat de game
    }
}