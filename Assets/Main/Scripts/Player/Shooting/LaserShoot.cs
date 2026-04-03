using UnityEngine;

public class LaserShoot : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask layer;

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
        bool hitted = Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, 5f, layer);

        if (hitted)
        {
            hitted = hit.collider;
            Debug.Log("Hit: " + hit.collider.name);
        }
    }
}
