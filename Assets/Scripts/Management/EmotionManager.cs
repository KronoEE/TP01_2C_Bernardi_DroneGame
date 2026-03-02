using System.Collections;
using System.Linq;
using UnityEngine;

public class EmotionManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] bool bIsStressed;
    [SerializeField] bool bIsFearing;
    // Emotions
    private bool isStressed;
    private bool isFeared;
    private float StressPercentaje = 0;
    private float fearPercentaje = 0;

    private void Awake()
    {
        CampFire.OnCampFireEntered += HandleCampFireEntered;
        CampFire.OnCampFireExited += HandleCampFireExited;
    }
    private void Start()
    {
        StressPercentaje = Mathf.Clamp(StressPercentaje, 0, 100);
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
            if (bIsStressed)
            {
                if (StressPercentaje >= 100)
                {
                    isStressed = true;
                    player.GetTired();
                }
                else
                {
                    StressPercentaje+=10;
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
            if (isFeared && isStressed)
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
            if (bIsStressed)
            {
                if (StressPercentaje == 50)
                {
                    isStressed = false;
                    player.emotions.First(x => x.EmotionType == PlayerController.EmotionType.Tired).bIsActive = false;
                }
                StressPercentaje -= 10;
            }
            yield return new WaitForSeconds(0.2f);
            if (!isFeared && !isStressed)
            {
                player.GetCalm();
                player.UpdateEmotions();
            }
            if (StressPercentaje == 0 && fearPercentaje == 0)
            {
                StopCoroutine(DecreaseEmotions());
                break;
            }
        }
    }
}
