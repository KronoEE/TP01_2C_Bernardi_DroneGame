using UnityEngine;

public class AnimationsUI : MonoBehaviour
{
    [SerializeField] private GameObject CalmLogo;
    [SerializeField] private GameObject FatigueLogo;
    [SerializeField] private GameObject FearLogo;
    [SerializeField] private GameObject PainLogo;
    [SerializeField] private GameObject StressLogo;
    [SerializeField] private PlayerController playerController;

    private float logoPositionX = 50f;
    private float logoPositionY = -50f;
    private void Awake()
    {
        playerController.OnEmotionChanged += HandleEmotionChanged;
    }
    private void OnDestroy()
    {
        playerController.OnEmotionChanged -= HandleEmotionChanged;
    }
    private void HandleEmotionChanged()
    {
        int count = 0;
        for (int i = 0; i < playerController.emotions.Count; i++)
        {
            switch (playerController.emotions[i].EmotionType)
            {
                case PlayerController.EmotionType.Calm:
                    if (playerController.emotions[i].bIsActive)
                    {
                        CalmLogo.SetActive(true);
                        CalmLogo.GetComponent<RectTransform>().anchoredPosition = new Vector3(-100, logoPositionY - 90 * count, 0);
                        LeanTween.moveX(CalmLogo.GetComponent<RectTransform>(), logoPositionX, 3f).setEase(LeanTweenType.easeOutBounce);
                        count++;
                    }
                    else
                    {
                        LeanTween.moveX(CalmLogo.GetComponent<RectTransform>(), -100, 1f).setEase(LeanTweenType.easeInBack).setOnComplete(() => CalmLogo.SetActive(false));
                    }
                    break;
                case PlayerController.EmotionType.Stressed:
                    if (playerController.emotions[i].bIsActive)
                    {
                        StressLogo.SetActive(true);
                        StressLogo.GetComponent<RectTransform>().anchoredPosition = new Vector3(-100, logoPositionY - 90 * count, 0);
                        LeanTween.moveX(StressLogo.GetComponent<RectTransform>(), logoPositionX, 1f).setEase(LeanTweenType.easeOutBounce);
                        count++;
                    }
                    else
                    {
                        LeanTween.moveX(StressLogo.GetComponent<RectTransform>(), -100, 1f).setEase(LeanTweenType.easeInBack).setOnComplete(() => StressLogo.SetActive(false));
                    }
                    break;
                case PlayerController.EmotionType.Fear:
                    if (playerController.emotions[i].bIsActive)
                    {
                        FearLogo.SetActive(true);
                        FearLogo.GetComponent<RectTransform>().anchoredPosition = new Vector3(-100, logoPositionY - 90 * count, 0);
                        LeanTween.moveX(FearLogo.GetComponent<RectTransform>(), logoPositionX, 1f).setEase(LeanTweenType.easeOutBounce);
                        count++;
                    }
                    else
                    {
                        LeanTween.moveX(FearLogo.GetComponent<RectTransform>(), -100, 1f).setEase(LeanTweenType.easeInBack).setOnComplete(() => FearLogo.SetActive(false));
                    }
                    break;
                case PlayerController.EmotionType.Pain:
                    if (playerController.emotions[i].bIsActive)
                    {
                        PainLogo.SetActive(true);
                        PainLogo.GetComponent<RectTransform>().anchoredPosition = new Vector3(-100, logoPositionY - 90 * count, 0);
                        LeanTween.moveX(PainLogo.GetComponent<RectTransform>(), logoPositionX, 1f).setEase(LeanTweenType.easeOutBounce);
                        count++;
                    }
                    else
                    {
                        LeanTween.moveX(PainLogo.GetComponent<RectTransform>(), -100, 1f).setEase(LeanTweenType.easeInBack).setOnComplete(() => PainLogo.SetActive(false));
                    }
                    break;
                case PlayerController.EmotionType.Tired:
                    if (playerController.emotions[i].bIsActive)
                    {
                        FatigueLogo.SetActive(true);
                        FatigueLogo.GetComponent<RectTransform>().anchoredPosition = new Vector3(-100, logoPositionY - 90 * count, 0);
                        LeanTween.moveX(FatigueLogo.GetComponent<RectTransform>(), logoPositionX, 1f).setEase(LeanTweenType.easeOutBounce);
                        count++;
                    }
                    else
                    {
                        LeanTween.moveX(FatigueLogo.GetComponent<RectTransform>(), -100, 1f).setEase(LeanTweenType.easeInBack).setOnComplete(() => FatigueLogo.SetActive(false));
                    }
                    break;
            }
        }
    }
}