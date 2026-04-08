using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Levels")]
    [SerializeField] private LevelDataSO[] levels;
    [SerializeField] private int currentLevelIndex = 0;

    [Header("UI")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject gameOverPanel;

    private int enemiesKilled = 0;
    private LevelDataSO CurrentLevel => levels[currentLevelIndex];

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        enemiesKilled = 0;

        if (ScoreSystem.Instance != null)
            ScoreSystem.Instance.onScoreChanged += CheckVictoryCondition;

        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }
    private void OnDestroy()
    {
        if (ScoreSystem.Instance != null)
            ScoreSystem.Instance.onScoreChanged -= CheckVictoryCondition;
    }
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        Debug.Log($"Enemigos eliminados: {enemiesKilled}/{CurrentLevel.enemiesToKill}");

        if (enemiesKilled >= CurrentLevel.enemiesToKill)
            Victory();
    }
    private void CheckVictoryCondition(int newScore)
    {
        if (enemiesKilled >= CurrentLevel.enemiesToKill)
            Victory();
    }
    public void Victory()
    {
        Debug.Log($"Nivel {CurrentLevel.levelName} completado!");
        Time.timeScale = 0f;

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        Invoke(nameof(LoadNextLevel), 3f);
    }
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        int nextIndex = currentLevelIndex + 1;

        if (nextIndex < levels.Length)
        {
            currentLevelIndex = nextIndex;
            SceneManager.LoadScene(CurrentLevel.sceneName);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        enemiesKilled = 0;
        SceneManager.LoadScene(CurrentLevel.sceneName);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}