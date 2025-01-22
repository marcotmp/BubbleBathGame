using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TweenTest : MonoBehaviour
{
    public Transform[] objs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        for (int i = 0; i < objs.Length; i+=2)
        {
            Merge(objs[i], objs[i + 1], i * 0.5f);
        }
    }

    private void Merge(Transform objA, Transform objB, float duration)
    {
        var contactPoint = (objA.position + objB.position) / 2;
        StartCoroutine(MergeCoroutine(objA, objB, contactPoint, duration));
    }

    private IEnumerator MergeCoroutine(Transform objA, Transform objB, Vector3 contactPoint, float duration)
    {
        objA.DOMove(contactPoint, duration);
        objB.DOMove(contactPoint, duration);
        objB.DOScale(duration * 0.5f, duration);
        yield return new WaitForSeconds(duration);
    }
}
