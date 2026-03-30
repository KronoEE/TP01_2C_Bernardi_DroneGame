using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform drone;
    private Vector3 velocityCameraFollow;

    [Header("Offset desde el drone (local space)")]
    public Vector3 offset = new Vector3(0f, 1f, -3f);

    [Header("Suavizado")]
    public float smoothTime = 0.1f;

    [Header("Rotación")]
    public float pitchAngle = 10f;

    private void Awake()
    {
        drone = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = drone.TransformPoint(offset);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocityCameraFollow,
            smoothTime
        );
        float droneYaw = drone.GetComponent<PlayerController>().currentYRot;
        transform.rotation = Quaternion.Euler(pitchAngle, droneYaw, 0f);
    }
}