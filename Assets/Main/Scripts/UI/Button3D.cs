using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Button3D : MonoBehaviour
{
    [SerializeField] private UnityEvent onClickAction;

    [Header("Visual Feedback")]
    [SerializeField] private float pressScale = 0.9f;
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private float hoverSpeed = 8f;

    [Header("Audio")]
    [SerializeField] private bool playHoverSound = true;

    private Vector3 originalScale;
    private Color originalColor;
    private Camera mainCamera;
    private Renderer rend;
    private bool isHovered;
    private AudioManager audioManager;
    private void Start()
    {
        originalScale = transform.localScale;
        mainCamera = Camera.main ?? FindFirstObjectByType<Camera>();
        rend = GetComponentInChildren<Renderer>();

        if (rend != null)
            originalColor = rend.material.color;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Update()
    {
        bool overUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

        Vector3 targetScale = (isHovered && !overUI) ? originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * hoverSpeed);

        if (Input.GetMouseButtonDown(0) && isHovered && !overUI) // 👈
            OnClick();
    }
    private void OnMouseEnter()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        isHovered = true;

        if (rend != null)
            rend.material.color = hoverColor;

        if (playHoverSound && audioManager != null)
            audioManager.PlayUI(audioManager.HoverUi);
    }
    private void OnMouseExit()
    {
        isHovered = false;
        if (rend != null)
            rend.material.color = originalColor;
    }
    public void OnClick()
    {
        audioManager.PlayUI(audioManager.ButtonUI);
        transform.localScale = originalScale * pressScale;
        Invoke(nameof(ExecuteAction), 0.1f);
    }
    private void ExecuteAction()
    {
        transform.localScale = originalScale;
        onClickAction?.Invoke();
    }
}

