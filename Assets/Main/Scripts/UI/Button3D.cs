using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour
{
    [SerializeField] private UnityEvent onClickAction;

    [Header("Visual Feedback")]
    [SerializeField] private float pressScale = 0.9f;
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private float hoverSpeed = 8f;

    private Vector3 originalScale;
    private Color originalColor;
    private Camera mainCamera;
    private Renderer rend;
    private bool isHovered;
    private void Start()
    {
        originalScale = transform.localScale;
        mainCamera = Camera.main ?? FindFirstObjectByType<Camera>();
        rend = GetComponentInChildren<Renderer>();

        if (rend != null)
            originalColor = rend.material.color;
    }
    private void Update()
    {
        Vector3 targetScale = isHovered ? originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * hoverSpeed);

        if (Input.GetMouseButtonDown(0) && isHovered)
            OnClick();
    }
    private void OnMouseEnter()
    {
        isHovered = true;
        if (rend != null)
            rend.material.color = hoverColor;
    }
    private void OnMouseExit()
    {
        isHovered = false;
        if (rend != null)
            rend.material.color = originalColor;
    }
    private void OnClick()
    {
        transform.localScale = originalScale * pressScale;
        Invoke(nameof(ExecuteAction), 0.1f);
    }
    private void ExecuteAction()
    {
        transform.localScale = originalScale;
        onClickAction?.Invoke();
    }
}

