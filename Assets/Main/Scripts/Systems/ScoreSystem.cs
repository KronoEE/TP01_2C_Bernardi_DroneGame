using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance { get; private set; }

    public event Action<int> onScoreChanged;

    private int currentScore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        onScoreChanged?.Invoke(currentScore);
        Debug.Log($"Score: {currentScore}");
    }
    public int GetScore() => currentScore;
}