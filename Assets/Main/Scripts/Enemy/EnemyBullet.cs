using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int damage = 25;
    [SerializeField] private GameObject impactEffect;

    private float lifeSpan = 4.5f;
    private void Start()
    {
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifeSpan);
    }

    private void OnTriggerEnter(Collider collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakingDamage(damage);

            GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(impact, 0.5f);
            Destroy(gameObject);
        }
    }
}
