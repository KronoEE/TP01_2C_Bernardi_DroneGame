using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Winpanel : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] Button settingsButtonbackBtn;
    [SerializeField] Button restartBtn;
    [SerializeField] Button settingsBtn;
    [SerializeField] Button homeBtn;
    private void Awake()
    {
        restartBtn.onClick.AddListener(OnRestartClick);
        settingsBtn.onClick.AddListener(OnSettingsClicked);
        homeBtn.onClick.AddListener(OnHomeClicked);
        settingsButtonbackBtn.onClick.AddListener(OnSettingsBack);
    }

    private void OnDestroy()
    {
        restartBtn.onClick.RemoveAllListeners();
        settingsBtn.onClick.RemoveAllListeners();
        homeBtn.onClick.RemoveAllListeners();
        settingsButtonbackBtn.onClick.RemoveAllListeners();
    }

    private void OnRestartClick()
    {
        SceneManager.LoadScene("Level01");
        Time.timeScale = 1;
    }

    private void OnSettingsClicked()
    {
        settingsPanel.SetActive(true);
    }

    private void OnHomeClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnSettingsBack()
    {
        settingsPanel.SetActive(false);
    }
}