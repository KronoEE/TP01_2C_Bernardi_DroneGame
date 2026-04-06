using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [Header("Rocket Config")]
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float rocketSpeed = 25f;
    [SerializeField] private KeyCode fireKey = KeyCode.Q;

    [Header("Trajectory")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private int trajectoryResolution = 30;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private float maxRange = 80f;

    [Header("Trajectory Preview")]
    [SerializeField] private LineRenderer trajectoryLine;
    [SerializeField] private bool showTrajectory = true;
    [SerializeField] private KeyCode toggleTrajectoryKey = KeyCode.T;
    private void Update()
    {
        if (Input.GetKeyDown(toggleTrajectoryKey))
            showTrajectory = !showTrajectory;

        if (showTrajectory)
            DrawTrajectoryPreview();
        else if (trajectoryLine != null)
            trajectoryLine.enabled = false;

        if (Input.GetKeyDown(fireKey))
            Launch();
    }
    private void Launch()
    {
        if (rocketPrefab == null) return;

        Vector3 target = GetTargetPoint();
        Vector3[] points = CalculateTrajectory(shootPoint.position, target);

        Vector3 initialDirection = (points[1] - points[0]).normalized;
        Quaternion initialRotation = Quaternion.LookRotation(initialDirection);

        GameObject rocketObj = Instantiate(rocketPrefab, shootPoint.position, initialRotation);
        PlayerRocket rocket = rocketObj.GetComponent<PlayerRocket>();
        if (rocket != null)
            rocket.Initialize(points, rocketSpeed);
    }
    private Vector3[] CalculateTrajectory(Vector3 origin, Vector3 target)
    {
        Vector3[] points = new Vector3[trajectoryResolution];

        float distance = Vector3.Distance(origin, target);
        float arcHeight = Mathf.Clamp(distance * 0.25f, 3f, 12f);

        for (int i = 0; i < trajectoryResolution; i++)
        {
            float t = (float)i / (trajectoryResolution - 1);
            Vector3 linearPoint = Vector3.Lerp(origin, target, t);
            float parabola = 4f * arcHeight * t * (1f - t);
            points[i] = linearPoint + Vector3.up * parabola;
        }
        return points;
    }
    private Vector3 GetTargetPoint()
    {
        Collider[] hits = Physics.OverlapSphere(shootPoint.position, maxRange, hitLayer);

        float closestDistance = Mathf.Infinity;
        Vector3 closestPoint = shootPoint.position + shootPoint.forward * maxRange;

        foreach (Collider hit in hits)
        {
            if (hit.GetComponent<HealthSystem>() == null) continue;
            float dist = Vector3.Distance(shootPoint.position, hit.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestPoint = hit.transform.position + Vector3.up * 1f;
            }
        }
        return closestPoint;
    }
    private void DrawTrajectoryPreview()
    {
        if (trajectoryLine == null) return;

        Vector3 target = GetTargetPoint();
        Vector3[] points = CalculateTrajectory(shootPoint.position, target);

        trajectoryLine.enabled = true;
        trajectoryLine.positionCount = points.Length;
        trajectoryLine.SetPositions(points);
    }
}
