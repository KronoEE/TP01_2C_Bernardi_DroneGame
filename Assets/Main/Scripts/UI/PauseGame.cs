using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] Button resumeBtn;
    [SerializeField] Button settingsBtn;
    [SerializeField] Button homeBtn;
    [SerializeField] Button settingsBackBtn;

    public bool isPaused = false;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        resumeBtn.onClick.AddListener(ResumeGame);
        settingsBtn.onClick.AddListener(OnSettingsClicked);
        homeBtn.onClick.AddListener(OnHomeClicked);
        settingsBackBtn.onClick.AddListener(OnSettingsBack);

        AddHoverSound(resumeBtn);
        AddHoverSound(settingsBtn);
        AddHoverSound(homeBtn);
        AddHoverSound(settingsBackBtn);
    }
    private void OnDestroy()
    {
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
        audioManager.PlayUI(audioManager.ButtonUI);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
    private void OnPauseClicked()
    {
        audioManager.PlayUI(audioManager.ButtonUI);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
    private void OnSettingsClicked()
    {
        audioManager.PlayUI(audioManager.ButtonUI);
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }
    private void OnHomeClicked()
    {
        audioManager.PlayUI(audioManager.ButtonUI);
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    private void OnSettingsBack()
    {
        audioManager.PlayUI(audioManager.ButtonUI);
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
    private void AddHoverSound(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();

        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;

        entry.callback.AddListener
            (
                (eventData) => { audioManager.PlayUI(audioManager.HoverUi); }
            );

        trigger.triggers.Add(entry);
    }
}