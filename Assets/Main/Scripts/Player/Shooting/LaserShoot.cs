using UnityEngine;

public class LaserShoot : MonoBehaviour
{
    [Header("Shoot Config")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private float range = 50f;
    [SerializeField] private int damage = 35;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject impactEffect;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        RaycastHit hit;
        bool hitted = Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, range, hitLayer);

        if (!hitted) return;

        HealthSystem health = hit.collider.GetComponent<HealthSystem>();

        if (health == null) return;

            health.TakeDamage(damage);

            //if (hit.collider.CompareTag("Enemy"))
                //scoreSystem?.AddScore(100);
            //else if (hit.collider.CompareTag("Civil"))
                //scoreSystem?.AddScore(-50);
            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 0.3f);
            }
        Debug.Log($"Hit: {hit.collider.name}");
    }
}
