using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private int damage = 25;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private float lifeSpan = 4.5f;

    private float spawnTime;
    private float activationDelay = 0.2f;
    private Vector3 direction;
    private bool isReady;

    public void Initialize(Vector3 targetPos)
    {
        direction = (targetPos - transform.position).normalized;
        transform.forward = direction;
        spawnTime = Time.time;
        isReady = true;
        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifeSpan);
    }

    private void OnEnable()
    {
        isReady = false;
        direction = Vector3.zero;
    }

    private void OnDisable()
    {
        isReady = false;
        CancelInvoke();
    }

    private void Update()
    {
        if (!isReady) return;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!isReady) return;
        if (Time.time < spawnTime + activationDelay) return;
        if (collision.isTrigger) return;

        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player == null) return;

        HealthSystem health = player.GetComponent<HealthSystem>();
        if (health != null)
            health.TakeDamage(damage);

        SpawnImpact();
        ReturnToPool();
    }

    private void SpawnImpact()
    {
        if (impactEffect == null) return;
        GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(impact, 0.5f);
    }

    private void ReturnToPool()
    {
        isReady = false;
        CancelInvoke();
        PoolManager.Instance?.ReturnEnemyBullet(this);
    }
}