using System.Linq;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool ActiveStress;
    public bool ActiveFear;
    public bool ActivePain;
    public bool ActiveFatigue;
    public bool ActiveCalm;

    PlayerController playerScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int playerLayer = LayerMask.NameToLayer("Player");

        if (collision.gameObject.layer == playerLayer)
        {
            Debug.Log("Player collided with wall");

            playerScript = collision.gameObject.GetComponent<PlayerController>();

            if (playerScript != null)
            {
                SetPlayerEmotion();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == playerLayer)
        {
            if (ActiveStress)
            {
                playerScript.emotions.First(x => x.EmotionType == PlayerController.EmotionType.Stressed).bIsActive = false;
            }
            if (ActiveFear)
            {
                playerScript.emotions.First(x => x.EmotionType == PlayerController.EmotionType.Fear).bIsActive = false;
            }
            if (ActivePain)
            {
                playerScript.emotions.First(x => x.EmotionType == PlayerController.EmotionType.Pain).bIsActive = false;
            }
            if (ActiveFatigue)
            {
                playerScript.emotions.First(x => x.EmotionType == PlayerController.EmotionType.Tired).bIsActive = false;
            }
            if (ActiveCalm)
            {
                playerScript.emotions.First(x => x.EmotionType == PlayerController.EmotionType.Calm).bIsActive = false;
            }
            playerScript.UpdateEmotions();
            Destroy(gameObject);
        }
    }
    private void SetPlayerEmotion()
    {
        if (ActiveStress)
        {
            playerScript.GetStressed();
        }
        if (ActiveFear)
        {
            playerScript.GetFear();
        }
        if (ActivePain)
        {
            playerScript.GetPain();
        }
        if (ActiveFatigue)
        {
            playerScript.GetTired();
        }
        if (ActiveCalm)
        {
            playerScript.GetCalm();
        }
        playerScript.UpdateEmotions();
    }
}