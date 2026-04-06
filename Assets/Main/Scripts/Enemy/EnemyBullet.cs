using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int damage = 25;
    [SerializeField] private GameObject impactEffect;

    private float lifeSpan = 4.5f;
    private Vector3 direction;

    public void Initialize(Vector3 targetPos)
    {
        direction = (targetPos - transform.position).normalized;
        transform.forward = direction;
    }
    private void Start()
    {
        rb.linearVelocity = direction * speed;
        Destroy(gameObject, lifeSpan);
    }

    private void OnTriggerEnter(Collider collision)
    {
            if (collision.isTrigger) return;

            PlayerController player = collision.GetComponent<PlayerController>();

            if (player == null) return;

            HealthSystem healthSystem = player.GetComponent<HealthSystem>();
            if (healthSystem != null)
                healthSystem.TakeDamage(damage);

            SpawnImpact();
            Destroy(gameObject);
    }
    private void SpawnImpact()
    {
        if (impactEffect == null) return;
        GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(impact, 0.5f);
    }
}
