using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Profiling;

public class BubbleMerger : MonoBehaviour
{
    public float maxScale = 2;
    public BubbleSpawner spawner { get; set; }
    [SerializeField] private float duration = 0.5f;

    public void MergeBubbles(Bubble bubble, Bubble otherBubble)
    {
        var bubbleIsBigger = bubble.sizeId > otherBubble.sizeId;

        // Get bigger and smaller
        Bubble bigger = bubbleIsBigger ? bubble : otherBubble;
        Bubble smaller = bubbleIsBigger ? otherBubble : bubble;

        bigger.Enable(false);
        smaller.Enable(false);

        // Destroy smaller bubble
        smaller.gameObject.SetActive(false);
        spawner.ReleaseBubble(smaller);

        bigger.SetSizeId(bigger.sizeId + 1);
        bigger.onScaleComplete = () =>
            // wait for scale up and 
            bigger.Enable(true);
        
    }

    internal void MergeBubbles2(Bubble bubble, Bubble otherBubble)
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
        // Destroy smaller bubble
        smaller.gameObject.SetActive(false);
        spawner.ReleaseBubble(smaller);

        sequence.Append(bigger.transform.DOScale(newBigScale, duration));
        sequence.OnComplete(() =>
        {
            bigger.Enable(true);

            //if (bigger.sizeId > 4)
            //    spawner.DestroyBubble(smaller);
        });

    }

    //public float mergeIntensity = 0.1f;

    //public void MoveCloser(Bubble bubble, Bubble otherBubble)
    //{
    //    // find other bubble direction
    //    var dir = otherBubble.transform.position - bubble.transform.position;
    //    var normal = dir.normalized;

    //    bubble.AddForceClamped(normal);
    //}
}
