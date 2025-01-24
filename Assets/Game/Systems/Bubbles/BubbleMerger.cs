using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Pool;

public class BubbleMerger : MonoBehaviour
{
    public float maxScale = 2;
    public BubbleSpawner spawner { get; set; }
    [SerializeField] private float duration = 0.5f;

    internal void MergeBubbles(Bubble bubble, Bubble otherBubble)
    {
        Bubble bigger;
        Bubble smaller;

        if (bubble.transform.localScale.magnitude > otherBubble.transform.localScale.magnitude)
        {
            bigger = bubble;
            smaller = otherBubble;
        }
        else
        {
            bigger = otherBubble;
            smaller = bubble;
        }

        var newBigScale = Mathf.Min(bigger.transform.localScale.magnitude * 1.01f, maxScale);

        bigger.Enable(false);
        smaller.Enable(false);

        var contactPoint = (bigger.transform.position + smaller.transform.position) / 2;

        var sequence = DOTween.Sequence();
        //bigger.transform.DOMove(contactPoint, duration);
        //sequence.Append(smaller.transform.DOMove(contactPoint, duration));
        //sequence.AppendCallback(() =>
        {
            // Destroy smaller bubble
            smaller.gameObject.SetActive(false);
            spawner.ReleaseBubble(smaller);
        }
        //);
        sequence.Append(bigger.transform.DOScale(newBigScale, duration));
        sequence.OnComplete(() =>
        {
            bigger.Enable(true);

            //if (bigger.sizeId > 4)
            //    spawner.DestroyBubble(smaller);
        });

    }

    public float mergeIntensity = 0.1f;

    public void MoveCloser(Bubble bubble, Bubble otherBubble)
    {
        // find other bubble direction
        var dir = otherBubble.transform.position - bubble.transform.position;
        var normal = dir.normalized;

        //bubble.GetComponent<SphereCollider>().enabled = false;

        bubble.AddForce(normal * mergeIntensity);
    }
}
