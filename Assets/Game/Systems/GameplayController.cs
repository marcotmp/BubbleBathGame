using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public BubbleDestructor handDestructor;
    public BubbleScore scoreData;
    public SoundObject scoreSoundObject;
    public SoundObject laughSound;
    public PointsUI pointsUIPrefab;

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

        var instance = Instantiate(pointsUIPrefab);
        instance.SetScore(score);
        instance.transform.position = new Vector3(
            bubble.transform.position.x,
            bubble.transform.position.y,
            instance.transform.position.z);

        // Using this try catch to discover a bug in the webgl build
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