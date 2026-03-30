using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float horizontalSpeed;
    public float verticalSpeed;

    public Transform player;
    private float yaw;
    private float pitch;
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, -80f, 80f);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
}