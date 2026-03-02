using UnityEngine;

public class WinZone : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == playerLayer)
        {
            audioManager.Stop();
            audioManager.PlayLoopedSfx(audioManager.WinSfx);
            winScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
}