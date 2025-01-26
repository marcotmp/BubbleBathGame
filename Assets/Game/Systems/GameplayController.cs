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

        try 
        {
            // play baby laugh sound
            laughSound.PlayWithBuffer(bubble.GetLaughFx());
        }
        catch (System.Exception e) 
        {
            Debug.Log($"laughSound.PlayWithBuffer bubble {bubble.sizeId} \n{e}");
        }

        try
        {
            // play score sound
            scoreSoundObject.Play();
        }
        catch (System.Exception e)
        {
            Debug.Log($"Score sound object\n{e}");
        }

    }
}