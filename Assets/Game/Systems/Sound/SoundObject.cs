using UnityEngine;

public class SoundObject : MonoBehaviour
{
    public AudioSource source;

    public void Play()
    {
        if (!source.isPlaying) 
        {
            source.Play();
        }
    }
}
