using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerDataSO data;

    [Header("Bars")]
    [SerializeField] Fillbar healthBar;
    [SerializeField] Fillbar energyBar;

    [Header("Cameras")]
    [SerializeField] private GameObject firstPersonCamera;
    [SerializeField] private GameObject thirdPersonCamera;

    // Movement
    private float upForce;
    private float tiltAmountForward;
    private float tiltVelocityForward;
    private float tiltAmountSideways;
    private float tiltAmountVelocity;

    // Rotation
    private float wantedYRot;
    private float currentYRot;
    private float rotationYVelocity;

    // Components
    private Rigidbody rb;
    private bool isFirstPerson;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Start()
    {
        data.currentHealth = data.maxHealth;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;
            firstPersonCamera.SetActive(isFirstPerson);
            thirdPersonCamera.SetActive(!isFirstPerson);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void FixedUpdate()
    {
        UpDown();
        ForwardBackMovement();   
        StrafeMovement();        
        Rotation();              

        rb.AddRelativeForce(Vector3.up * upForce);
        rb.rotation = Quaternion.Euler(tiltAmountForward, currentYRot, tiltAmountSideways);
    }

    private void UpDown()
    {
        if (Input.GetKey(data.spaceKey))
            upForce = data.upForceUp;
        else if (Input.GetKey(data.ctrlKey))
            upForce = data.upForceDown;
        else
            upForce = data.upForceHover;
    }
    private void ForwardBackMovement()
    {
        float axis = Input.GetAxis("Vertical");
        rb.AddRelativeForce(Vector3.forward * axis * data.movementForwardSpeed);
        tiltAmountForward = Mathf.SmoothDamp(
            tiltAmountForward,
            data.maxTiltForward * axis,
            ref tiltVelocityForward,
            data.tiltSmoothTime
        );
    }
    private void StrafeMovement()
    {
        float axis = Input.GetAxis("Horizontal");
        rb.AddRelativeForce(Vector3.right * axis * data.sideMovementAmount);
        tiltAmountSideways = Mathf.SmoothDamp(
            tiltAmountSideways,
            -data.maxTiltSideways * axis, 
            ref tiltAmountVelocity,
            data.tiltSmoothTime 
        );
    }
    private void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        wantedYRot += mouseX * data.mouseSensitivity;
        currentYRot = Mathf.SmoothDamp(
            currentYRot,
            wantedYRot,
            ref rotationYVelocity,
            data.rotationSmoothTime
        );
    }
    public void TakingDamage(float damage)
    {
        data.currentHealth -= damage;
        healthBar.UpdateBars(data.maxHealth, data.currentHealth);
        if (data.currentHealth <= 0) Die();
    }
    private void Die()
    {
        Debug.Log("Player has died.");
        data.currentHealth = 0;
    }
}