using System;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public static Action OnCampFireEntered;
    public static Action OnCampFireExited;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == playerLayer)
        {
            OnCampFireEntered?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == playerLayer)
        {
            OnCampFireExited?.Invoke();
        }
    }
}
