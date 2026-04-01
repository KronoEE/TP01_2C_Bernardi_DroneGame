using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    [Header("Forces")]
    public float movementForwardSpeed = 500f;
    public float sideMovementAmount = 300f;
    public float upForceUp = 450f;
    public float upForceDown = -200f;
    public float upForceHover = 98.1f;

    [Header("Inclination")]
    public float maxTiltForward = 20f;
    public float maxTiltSideways = 20f;
    public float tiltSmoothTime = 0.1f;

    [Header("Mouse Rotation")]
    public float mouseSensitivity = 3f;
    public float rotationSmoothTime = 0.25f;

    [Header("Player Attributes")]
    public float currentHealth;
    public float maxHealth = 100f;

    [Header("Keys")]
    public KeyCode spaceKey = KeyCode.Space;
    public KeyCode ctrlKey = KeyCode.LeftControl;
}
