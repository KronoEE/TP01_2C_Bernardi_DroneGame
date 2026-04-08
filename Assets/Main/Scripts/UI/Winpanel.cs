using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Winpanel : MonoBehaviour
{
    [SerializeField] Button restartBtn;
    [SerializeField] Button homeBtn;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        restartBtn.onClick.AddListener(OnRestartClick);
        homeBtn.onClick.AddListener(OnHomeClicked);

        AddHoverSound(restartBtn);
        AddHoverSound(homeBtn);
    }
    private void OnDestroy()
    {
        restartBtn.onClick.RemoveAllListeners();
        homeBtn.onClick.RemoveAllListeners();
    }
    private void OnRestartClick()
    {
        audioManager.PlayUI(audioManager.ButtonUI);
        SceneManager.LoadScene("Scene_01");
        Time.timeScale = 1;
    }
    private void OnHomeClicked()
    {
        audioManager.PlayUI(audioManager.ButtonUI);
        SceneManager.LoadScene("MainMenu");
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