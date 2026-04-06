using UnityEngine;

public class PlayerRocket : MonoBehaviour
{
    [SerializeField] private int damage = 50;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private LayerMask damageLayer;

    private Vector3[] trajectoryPoints;
    private int currentPoint;
    private float speed;
    private bool isInitialized;
    private bool hasExploded;
    public void Initialize(Vector3[] points, float rocketSpeed)
    {
        trajectoryPoints = points;
        speed = rocketSpeed;
        currentPoint = 0;
        isInitialized = true;
        hasExploded = false;
    }
    private void Update()
    {
        if (!isInitialized || hasExploded) return;

        while (currentPoint < trajectoryPoints.Length)
        {
            Vector3 target = trajectoryPoints[currentPoint];
            float step = speed * Time.deltaTime;
            float distanceToTarget = Vector3.Distance(transform.position, target);

            if (step >= distanceToTarget)
            {
                transform.position = target;
                currentPoint++;
            }
            else
            {
                Vector3 direction = (target - transform.position).normalized;
                transform.position += direction * step;
                transform.forward = direction;
                break;
            }
        }
        if (currentPoint >= trajectoryPoints.Length)
            OnImpact(transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isInitialized || hasExploded) return;
        if (other.GetComponentInParent<PlayerController>() != null) return;
        if (other.isTrigger) return;

        OnImpact(transform.position);
    }
    private void OnImpact(Vector3 position)
    {
        if (hasExploded) return;
        hasExploded = true;

        Collider[] hits = Physics.OverlapSphere(position, explosionRadius, damageLayer);
        foreach (Collider hit in hits)
        {
            HealthSystem health = hit.GetComponent<HealthSystem>();
            if (health != null)
                health.TakeDamage(damage);
        }

        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, position, Quaternion.identity);
            Destroy(effect, 1f);
        }

        Destroy(gameObject);
    }
}

