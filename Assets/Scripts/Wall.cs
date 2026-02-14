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
    private void SetPlayerEmotion()
    {
        if (ActiveStress)
            playerScript.GetStressed();

        if (ActiveFear)
            playerScript.GetFear();

        if (ActivePain)
            playerScript.GetPain();

        if (ActiveFatigue)
            playerScript.GetTired();

        if (ActiveCalm)
            playerScript.GetCalm();
    }
}