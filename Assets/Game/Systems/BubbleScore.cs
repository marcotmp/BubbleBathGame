using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BubbleScore", menuName = "Bubbles/BubbleScore")]
public class BubbleScore : ScriptableObject
{
    private int score = 0;
    public event Action onScoreUpdated;

    public void AddScore(int score)
    {
        this.score += score;
        onScoreUpdated?.Invoke();
    }

    public int GetScore()
    {
        return score;
    }
}
