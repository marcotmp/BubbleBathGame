using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public BubbleDestructor handDestructor;
    public BubbleScore scoreData;

    private void Start()
    {
        handDestructor.bubbleDestroyed.AddListener(OnScore);
    }

    private void OnScore(Bubble bubble)
    {
        // calculate score based on bubble size
        var score = bubble.GetScore();
        scoreData.AddScore(score);

        // play baby laugh sound
    }
}