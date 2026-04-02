using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private float damageAmount = 20f;

    private void OnCollisionEnter(Collision collision)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == playerLayer)
        {
            collision.gameObject.GetComponent<PlayerController>().TakingDamage(damageAmount);
        }
    }
}