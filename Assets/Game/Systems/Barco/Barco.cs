using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Barco : MonoBehaviour
{
    public AudioSource boatSound;
    public Transform boat;
    public Transform p0;
    public Transform p1;

    public float boatDuration = 3;
    public float inactiveDuration = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DoLogic());        
    }

    private IEnumerator DoLogic()
    {
        while (true)
        {
            boat.position = p0.position;
            boat.gameObject.SetActive(true);

            // start sound loop
            boatSound.loop = true;
            boatSound.Play();

            var t = boat.DOMove(p1.position, boatDuration);
            t.SetEase(Ease.Linear);

            yield return new WaitForSeconds(boatDuration);

            // turn off the boat
            boat.gameObject.SetActive(false);

            // end sound loop
            boatSound.Stop();

            yield return new WaitForSeconds(inactiveDuration);
        }
    }
}
