using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Barco : MonoBehaviour
{

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

            var t = boat.DOMove(p1.position, boatDuration);
            t.SetEase(Ease.Linear);

            yield return new WaitForSeconds(boatDuration);

            // turn off the boat
            boat.gameObject.SetActive(false);
            yield return new WaitForSeconds(inactiveDuration);
        }
    }
}
