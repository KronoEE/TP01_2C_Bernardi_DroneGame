using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private float damageAmount = 20f;
    private void OnTriggerEnter(Collider other)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        if (other.gameObject.layer == playerLayer)
        {
            other.gameObject.GetComponent<PlayerController>().TakingDamage(damageAmount);
        }

    }
}