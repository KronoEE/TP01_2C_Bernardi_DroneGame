using UnityEngine;

public class LaserShoot : MonoBehaviour
{
    [Header("Shoot Config")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private float range = 50f;
    [SerializeField] private int damage = 35;

    [Header("Score")]
    [SerializeField] private ScoreSystem scoreSystem;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject impactEffect;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        audioManager.PlaySFX(audioManager.LaserShootSfx);
        RaycastHit hit;
        bool hitted = Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, range, hitLayer);

        if (!hitted) return;

        HealthSystem health = hit.collider.GetComponent<HealthSystem>();

        if (health == null) return;

            health.TakeDamage(damage);

        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int civilLayer = LayerMask.NameToLayer("NPC");

        if (hit.collider.gameObject.layer == enemyLayer)
            scoreSystem?.AddScore(100);
        else if
            (hit.collider.gameObject.layer == civilLayer)
                scoreSystem?.AddScore(-50);

            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 0.3f);
            }
        Debug.Log($"Hit: {hit.collider.name}");
    }
}
