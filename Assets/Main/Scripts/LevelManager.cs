using System.Collections;
using System.Diagnostics;
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
    private bool levelCompleted = false;
    private LevelDataSO CurrentLevel => levels[currentLevelIndex];
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnEnemyKilled()
    {
        if (levelCompleted) return;
        enemiesKilled++;
        if (enemiesKilled >= CurrentLevel.enemiesToKill)
            Victory();
    }
    private void CheckVictoryCondition(int newScore)
    {

    }
    public void Victory()
    {
        if (levelCompleted) return;
        levelCompleted = true;
        audioManager.PlaySFX(audioManager.WinSfx);
        if (CurrentLevel.autoAdvance)
        {
            LoadNextLevel();
        }
        else
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (victoryPanel != null)
                victoryPanel.SetActive(true);
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        levelCompleted = false;
        enemiesKilled = 0;
        Time.timeScale = 1f;

        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.scene != scene) continue;
            if (obj.name == "VictoryPanel") victoryPanel = obj;
            if (obj.name == "DeathPanel") gameOverPanel = obj;
        }

        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (ScoreSystem.Instance != null)
            ScoreSystem.Instance.onScoreChanged += CheckVictoryCondition;
    }
    private IEnumerator LoadNextLevelDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0f;
        LoadNextLevel();
    }
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        enemiesKilled = 0;

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
        audioManager.PlaySFX(audioManager.LooseSfx);
        Time.timeScale = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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