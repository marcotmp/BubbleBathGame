using UnityEngine;

public class RandomSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] audioList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var clipId = Random.Range(0, audioList.Length);
        if (source != null )
        {
            source.clip = audioList[clipId];
            source.Play();
        }
    }
}
