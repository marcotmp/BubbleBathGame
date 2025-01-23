using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Pool;

public class BubbleMerger : MonoBehaviour
{
    public ObjectPool<Bubble> pool { get; set; }
    [SerializeField] private float duration = 0.5f;

    internal void MergeBubbles(Bubble bubble, Bubble otherBubble)
    {

        var scale = bubble.transform.localScale.magnitude;

        bubble.CanMerge = false;
        otherBubble.CanMerge = false;
        var contactPoint = (bubble.transform.position + otherBubble.transform.position) / 2;

        var sequence = DOTween.Sequence();
        bubble.transform.DOMove(contactPoint, duration);
        sequence.Append(otherBubble.transform.DOMove(contactPoint, duration));
        sequence.AppendCallback(() =>
        {
            // Destroy smaller bubble
            pool.Release(otherBubble);
        });
        sequence.Append(bubble.transform.DOScale(scale * 1.01f, duration));
        sequence.OnComplete(() => 
        {
            bubble.CanMerge = true;
        });
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
