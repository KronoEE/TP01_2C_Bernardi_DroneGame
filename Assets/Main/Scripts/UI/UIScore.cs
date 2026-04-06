using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIScore : MonoBehaviour
{
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreSystem.onScoreChanged += UpdateScore;
    }
    private void OnDestroy()
    {
        scoreSystem.onScoreChanged -= UpdateScore;
    }
    private void Start()
    {
        UpdateScore(0);

    }
    private void UpdateScore(int newScore)
    {
        if (newScore >= 0)
            scoreText.text = $"Score: +{newScore}";
        else
            scoreText.text = $"Score: {newScore}";

        scoreText.color = newScore >= 0 ? Color.green : Color.red;
    }
}
