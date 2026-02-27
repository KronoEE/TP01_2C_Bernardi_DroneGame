using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button SettingsBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button SettingsBackBtn;
    [Header("Panels")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsPanel;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        // CLICK EVENTS
        playBtn.onClick.AddListener(OnPlayClicked);
        quitBtn.onClick.AddListener(OnQuitClicked);
        SettingsBtn.onClick.AddListener(OnSettingsClicked);
        SettingsBackBtn.onClick.AddListener(OnSettingsBackButton);

        // HOVER EVENTS
        AddHoverSound(playBtn);
        AddHoverSound(quitBtn);
        AddHoverSound(SettingsBtn);
        AddHoverSound(SettingsBackBtn);
    }
    private void OnDestroy()
    {
        playBtn.onClick.RemoveAllListeners();
        quitBtn.onClick.RemoveAllListeners();
        SettingsBtn.onClick.RemoveAllListeners();
        SettingsBackBtn.onClick.RemoveAllListeners();
    }
    private void OnPlayClicked()
    {
        audioManager.PlayUI(audioManager.ButtonUI);
        SceneManager.LoadScene("Level01");
        Time.timeScale = 1;
    }
    private void OnQuitClicked()
    {
        audioManager.PlayUI(audioManager.ButtonUI);
        Application.Quit();
    }
    private void OnSettingsClicked()
    {
        audioManager.PlayUI(audioManager.ButtonUI);
        settingsPanel.SetActive(true);
    }
    private void OnSettingsBackButton()
    {
        audioManager.PlayUI(audioManager.ButtonUI);
        settingsPanel.SetActive(false);
        mainMenu.SetActive(true);
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
                (eventData) =>{audioManager.PlayUI(audioManager.HoverUi);}   
            );

        trigger.triggers.Add(entry);
    }
}