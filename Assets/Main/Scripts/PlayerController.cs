using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Fuerzas")]
    [SerializeField] private float movementForwardSpeed = 500f;
    [SerializeField] private float sideMovementAmount = 300f;
    [SerializeField] private float upForceUp = 450f;
    [SerializeField] private float upForceDown = -200f;
    [SerializeField] private float upForceHover = 98.1f;

    [Header("Inclinación")]
    [SerializeField] private float maxTiltForward = 20f;
    [SerializeField] private float maxTiltSideways = 20f;
    [SerializeField] private float tiltSmoothTime = 0.1f;

    [Header("Rotación con Mouse")]
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float rotationSmoothTime = 0.25f;

    [Header("Cámaras")]
    [SerializeField] private GameObject firstPersonCamera;
    [SerializeField] private GameObject thirdPersonCamera;

    [Header("Teclas")]
    [SerializeField] private KeyCode spaceKey = KeyCode.Space;
    [SerializeField] private KeyCode ctrlKey = KeyCode.LeftControl;

    // Health
    private float currentHealth;
    private float maxHealth = 100f;

    // Movimiento
    private float upForce;
    private float tiltAmountForward;
    private float tiltVelocityForward;
    private float tiltAmountSideways;
    private float tiltAmountVelocity;

    // Rotación
    private float wantedYRot;
    [HideInInspector] public float currentYRot;
    private float rotationYVelocity;

    // Componentes
    private Rigidbody rb;
    private bool isFirstPerson;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Bloquea y oculta el cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Cambio de cámara
        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;
            firstPersonCamera.SetActive(isFirstPerson);
            thirdPersonCamera.SetActive(!isFirstPerson);
        }

        // Liberar cursor con Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void FixedUpdate()
    {
        UpDown();
        ForwardBackMovement();   // W / S → adelante y atrás
        StrafeMovement();        // A / D → costado (con tilt lateral)
        Rotation();              // Mouse X → Yaw del drone

        rb.AddRelativeForce(Vector3.up * upForce);
        rb.rotation = Quaternion.Euler(tiltAmountForward, currentYRot, tiltAmountSideways);
    }

    private void UpDown()
    {
        if (Input.GetKey(spaceKey))
            upForce = upForceUp;
        else if (Input.GetKey(ctrlKey))
            upForce = upForceDown;
        else
            upForce = upForceHover;
    }

    // W / S: mueve hacia adelante/atrás e inclina el drone
    private void ForwardBackMovement()
    {
        float axis = Input.GetAxis("Vertical");
        rb.AddRelativeForce(Vector3.forward * axis * movementForwardSpeed);
        tiltAmountForward = Mathf.SmoothDamp(
            tiltAmountForward,
            maxTiltForward * axis,
            ref tiltVelocityForward,
            tiltSmoothTime
        );
    }

    // A / D: mueve en lateral e inclina el drone hacia los costados
    private void StrafeMovement()
    {
        float axis = Input.GetAxis("Horizontal");
        rb.AddRelativeForce(Vector3.right * axis * sideMovementAmount);
        tiltAmountSideways = Mathf.SmoothDamp(
            tiltAmountSideways,
            -maxTiltSideways * axis,
            ref tiltAmountVelocity,
            tiltSmoothTime
        );
    }

    // Mouse X acumula rotación Yaw suavemente
    private void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        wantedYRot += mouseX * mouseSensitivity;
        currentYRot = Mathf.SmoothDamp(
            currentYRot,
            wantedYRot,
            ref rotationYVelocity,
            rotationSmoothTime
        );
    }

    public void TakingDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        currentHealth = 0;
    }
}