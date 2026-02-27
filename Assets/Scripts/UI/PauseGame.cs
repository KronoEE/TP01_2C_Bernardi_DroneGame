using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject deathMenu;
    [SerializeField] Button pauseBtn;
    [SerializeField] Button resumeBtn;
    [SerializeField] Button settingsBtn;
    [SerializeField] Button homeBtn;
    [SerializeField] Button settingsBackBtn;

    public bool isPaused = false;

    private void Awake()
    {
        pauseBtn.onClick.AddListener(OnPauseClicked);
        resumeBtn.onClick.AddListener(ResumeGame);
        settingsBtn.onClick.AddListener(OnSettingsClicked);
        homeBtn.onClick.AddListener(OnHomeClicked);
        settingsBackBtn.onClick.AddListener(OnSettingsBack);
    }
    private void OnDestroy()
    {
        pauseBtn.onClick.RemoveAllListeners();
        resumeBtn.onClick.RemoveAllListeners();
        settingsBtn.onClick.RemoveAllListeners();
        homeBtn.onClick.RemoveAllListeners();
        settingsBackBtn.onClick.RemoveAllListeners();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                OnPauseClicked();
            }
        }
    }
    private void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
    private void OnPauseClicked()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
    private void OnSettingsClicked()
    {
        settingsMenu.SetActive(true);
    }
    private void OnHomeClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1;
    }
    private void OnSettingsBack()
    {
        settingsMenu.SetActive(false);
    }
}