using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private MonoBehaviour playerController;

    [SerializeField] private float normalTimeScale = 1f;
    [SerializeField] private float gameOverTimeScale = 0f;

    // Tracks whether the game is currently in a game-over state
    private bool isGameOver = false;

    // Total survival time in seconds since the level started
    [SerializeField] private float survivalTimeSeconds = 0f;

    private void Start()
    {
        Time.timeScale = normalTimeScale;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        survivalTimeSeconds = 0f;
        isGameOver = false;
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        // Update survival time while the player is alive
        survivalTimeSeconds += Time.deltaTime;

        if (playerStats != null && playerStats.IsDead())
        {
            HandleGameOver();
        }
    }

    private void HandleGameOver()
    {
        isGameOver = true;

        Time.timeScale = gameOverTimeScale;

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = normalTimeScale;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    // === Getter for survival time ===

    public float GetSurvivalTimeSeconds()
    {
        return survivalTimeSeconds;
    }
}