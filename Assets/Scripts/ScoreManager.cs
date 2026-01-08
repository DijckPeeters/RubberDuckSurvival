using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI leaderboardText;

    private int score = 0;

    void Start()
    {
        UpdateScoreUI();
        // Zorgt ervoor dat het Game Over scherm onzichtbaar is bij de start van een nieuwe ronde.
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    // Werkt de tekst in het HUD (Heads-Up Display) bij zodat de speler zijn score live ziet.
    void UpdateScoreUI()
    {
        if (currentScoreText != null)
            currentScoreText.text = "Score: " + score;
    }

    public void HandleGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        // PlayerPrefs wordt gebruikt om de High Score permanent op te slaan op de harde schijf.
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save(); // Forceert het wegschrijven van de nieuwe topscore.
        }

        if (highScoreText != null)
            highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");

        ShowLeaderboard();

        // Zet de simulatiesnelheid op 0 om alle bewegingen en physics in de game te bevriezen.
        Time.timeScale = 0f;
    }

    void ShowLeaderboard()
    {
        // Haalt de opgeslagen data op om een simpele ranglijst te tonen.
        if (leaderboardText != null)
            leaderboardText.text = "1. You: " + PlayerPrefs.GetInt("HighScore");
    }

    public void RestartGame()
    {
        // Reset de tijdschaal naar 1, anders zou de nieuwe scene ook direct gepauzeerd zijn.
        Time.timeScale = 1f;

        // Gebruikt de SceneManager om de huidige actieve scene volledig opnieuw te laden.
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}