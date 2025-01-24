using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text text;
    public BubbleScore channel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        channel.onScoreUpdated += UpdateScore;
        UpdateScore();
    }

    private void UpdateScore()
    {
        text.text = "Score: " + channel.GetScore();
    }

    private void OnDestroy()
    {
        channel.onScoreUpdated -= UpdateScore;
    }
}
