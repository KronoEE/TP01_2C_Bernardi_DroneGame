using System.Collections;
using System.Linq;
using UnityEngine;

public class EmotionManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] bool bIsFatigued;
    [SerializeField] bool bIsFearing;
    // Emotions
    private bool isFatigued;
    private bool isFeared;
    private float fatiguePercentaje = 0;
    private float fearPercentaje = 0;

    private void Awake()
    {
        CampFire.OnCampFireEntered += HandleCampFireEntered;
        CampFire.OnCampFireExited += HandleCampFireExited;
    }
    private void Start()
    {
        fatiguePercentaje = Mathf.Clamp(fatiguePercentaje, 0, 100);
        fearPercentaje = Mathf.Clamp(fearPercentaje, 0, 100);
        StartCoroutine(IncrementEmotions());
    }
    private void OnDestroy()
    {
        CampFire.OnCampFireEntered -= HandleCampFireEntered;
        CampFire.OnCampFireExited -= HandleCampFireExited;
    }

    private void HandleCampFireEntered()
    {
        if (player != null)
        {
            StartCoroutine(DecreaseEmotions());
            StopCoroutine(IncrementEmotions());
        }
    }
    private void HandleCampFireExited()
    {
        if (player != null)
        {
            StartCoroutine(IncrementEmotions());
            StopCoroutine(DecreaseEmotions());
        }
    }
    private IEnumerator IncrementEmotions()
    {
        while (true)
        {
            if (bIsFatigued)
            {
                if (fatiguePercentaje >= 100)
                {
                    isFatigued = true;
                    player.GetTired();
                }
                else
                {
                    fatiguePercentaje+=10;
                }
            }
            if (bIsFearing)
            {
                if (fearPercentaje >= 100)
                {
                    isFeared = true;
                    player.GetFear();
                }
                else
                {
                    fearPercentaje += 10;
                }
            }
            yield return new WaitForSeconds(0.5f);
            if (isFeared && isFatigued)
            {
                player.emotions.First(x => x.EmotionType == PlayerController.EmotionType.Calm).bIsActive = false;
                player.UpdateEmotions();
                StopCoroutine(IncrementEmotions());
                break;
            }
        }
    }
    private IEnumerator DecreaseEmotions()
    {
        while (true)
        {
            if (bIsFearing)
            {
                if (fearPercentaje == 50)
                {
                    isFeared = false;
                    player.emotions.First(x => x.EmotionType == PlayerController.EmotionType.Fear).bIsActive = false;
                }
                fearPercentaje -= 10;
            }
            if (bIsFatigued)
            {
                if (fatiguePercentaje == 50)
                {
                    isFatigued = false;
                    player.emotions.First(x => x.EmotionType == PlayerController.EmotionType.Tired).bIsActive = false;
                }
                fatiguePercentaje -= 10;
            }
            yield return new WaitForSeconds(0.2f);
            if (!isFeared && !isFatigued)
            {
                player.GetCalm();
                player.UpdateEmotions();
            }
            if (fatiguePercentaje == 0 && fearPercentaje == 0)
            {
                StopCoroutine(DecreaseEmotions());
                break;
            }
        }
    }
}
