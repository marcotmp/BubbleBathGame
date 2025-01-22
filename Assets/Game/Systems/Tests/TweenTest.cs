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

        var sequence = DOTween.Sequence();
        objB.DOMove(contactPoint, duration);
        sequence.Append(objA.DOMove(contactPoint, duration));
        // use join to add parallel tweens to the previous appended objects
        sequence.AppendCallback(() => 
        {
            Destroy(objA.gameObject);
        });
        sequence.Append(objB.DOScale(duration * 0.5f, duration));
        //sequence.OnComplete(() =>
        //{
        //});

    }
}
