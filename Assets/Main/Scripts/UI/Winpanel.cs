using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Winpanel : MonoBehaviour
{
    [SerializeField] Button restartBtn;
    [SerializeField] Button homeBtn;
    private void Awake()
    {
        restartBtn.onClick.AddListener(OnRestartClick);
        homeBtn.onClick.AddListener(OnHomeClicked);
    }
    private void OnDestroy()
    {
        restartBtn.onClick.RemoveAllListeners();
        homeBtn.onClick.RemoveAllListeners();
    }
    private void OnRestartClick()
    {
        SceneManager.LoadScene("Scene_01");
        Time.timeScale = 1;
    }
    private void OnHomeClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}