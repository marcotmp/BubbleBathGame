using System.Collections;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    public AudioSource source;
    private bool isWaiting = false;

    public void Play()
    {
        if (!source.isPlaying) 
        {
            source.Play();
        }
    }

    public void Play(AudioClip clip)
    {
        if (!source.isPlaying)
        {
            source.clip = clip;
            source.Play();
        }
    }

    public void PlayWithDelay(AudioClip clip, float delay)
    {
        Debug.Log($"{name} is trying to play clip with delay " + delay);
        if (!isWaiting)
        { 
            StartCoroutine(PlayCoroutine(clip, delay));
        }
    }

    private IEnumerator PlayCoroutine(AudioClip clip, float delay)
    {
        Debug.Log($"{name} is waiting clip with delay " + delay);
        isWaiting = true;
        yield return new WaitForSeconds(delay);
        Debug.Log($"{name} is playing clip after delay " + delay);
        Play(clip);
        isWaiting = false;
    }
}
