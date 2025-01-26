using System;
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
        if (!isWaiting)
        { 
            StartCoroutine(PlayCoroutine(clip, delay));
        }
    }

    private IEnumerator PlayCoroutine(AudioClip clip, float delay)
    {
        isWaiting = true;
        yield return new WaitForSeconds(delay);
        Play(clip);
        isWaiting = false;
    }

    internal void PlayOneShot(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public void PlayWithBuffer(AudioClip clip)
    {
        if (source.isPlaying)
        {
            bufferClip = clip;
        }
        else
        {
            Play(clip);
        }
    }

    private AudioClip bufferClip;

    private void Update()
    {
        if (!source.isPlaying && bufferClip != null)
        {
            Play(bufferClip);
            bufferClip = null;
        }
    }
}
