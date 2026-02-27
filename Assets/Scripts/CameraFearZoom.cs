using UnityEngine;
using Cinemachine;

public class CameraFearZoom : MonoBehaviour
{
    CinemachineVirtualCamera vcam;

    [Header("Zoom Settings")]
    public float normalFOV = 40f;
    public float fearFOV = 20f;
    public float changeSpeed = 5f;

    float targetFOV;

    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        targetFOV = normalFOV;
    }

    void Update()
    {
        float currentFOV = vcam.m_Lens.FieldOfView;

        vcam.m_Lens.FieldOfView = Mathf.Lerp(
            currentFOV,
            targetFOV,
            Time.deltaTime * changeSpeed
        );
    }

    public void SetFear(bool fear)
    {
        targetFOV = fear ? fearFOV : normalFOV;
    }
}