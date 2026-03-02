using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/Player")]

public class PlayerDataSO : ScriptableObject
{
    [Header("---------- Layers ----------")]
    public LayerMask layerMask;
    [Header("---------- Key Bindings ----------")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode attackKey = KeyCode.E;
    public KeyCode sprintKey = KeyCode.LeftShift;
    [Header("---------- Jump Settings ----------")]
    public float reboundForce = 6f;
    public float lengthRayCast = 1f;
    public int maxJumpForce = 5;
    [Header("---------- Player Settings ----------")]
    public float velocity = 2f;
    public float runVelocity = 7f;
    public int maxHealth = 100;
    public float fatigueSpeed = 3f;
    [Header("---------- Player Coroutines ----------")]
    public float sprintTime = 5f;
}
