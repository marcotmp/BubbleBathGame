using System.Collections;
using UnityEngine;

public class ShootRepeater : MonoBehaviour
{
    public float minDelay = 0.1f;
    public float maxDelay = 1f;
    public BubbleCannon bubbleCannon;
    private IEnumerator coroutine;

    public void StartShooting()
    {
        Debug.Log("ShootRepeater StartShooting");

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = Shoot();
        StartCoroutine(coroutine);
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            Debug.Log("ShootRepeater Shoot");

            bubbleCannon.Shoot();
            var delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    public void StopShooting()
    {
        Debug.Log("ShootRepeater EndShooting");
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
}
