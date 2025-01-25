using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public BubbleDestructor handDestructor;
    public BubbleScore scoreData;
    public SoundObject scoreSoundObject;
    public SoundObject laughSound;

    private void Start()
    {
        scoreData.ResetScore();
        handDestructor.bubbleDestroyed.AddListener(OnScore);
    }

    private void OnScore(Bubble bubble)
    {
        // calculate score based on bubble size
        var score = bubble.GetScore();
        scoreData.AddScore(score);

        // play score sound
        scoreSoundObject.Play();

        // play baby laugh sound
    }
}