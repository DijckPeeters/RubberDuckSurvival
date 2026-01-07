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
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (currentScoreText != null)
            currentScoreText.text = "Score: " + score;
    }

    public void HandleGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }

        if (highScoreText != null)
            highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");

        ShowLeaderboard();
        Time.timeScale = 0f; // Pauzeert de game
    }

    void ShowLeaderboard()
    {
        if (leaderboardText != null)
            leaderboardText.text = "1. You: " + PlayerPrefs.GetInt("HighScore");
    }

    // Hang deze functie aan een 'Restart' knop op je GameOver scherm
    public void RestartGame()
    {
        // HEEL BELANGRIJK: Zet de tijd weer op 1, anders blijft de game gepauzeerd!
        Time.timeScale = 1f;

        // Herlaadt de huidige scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}