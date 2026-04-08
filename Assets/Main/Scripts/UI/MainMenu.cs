using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Canvas references")]
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject settingsCanvas;

    [Header("Button reference")]
    [SerializeField] private Button settingsBtnBack;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        AddHoverSound(settingsBtnBack);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Scene_01");
    }
    public void OpenSettings()
    {
        settingsCanvas.SetActive(true);
        mainCanvas.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void CloseSettings()
    {
        audioManager.PlayUI(audioManager.ButtonUI);
        settingsCanvas.SetActive(false);
        mainCanvas.SetActive(true);
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