using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class BubbleMerger : MonoBehaviour
{
    public float maxScale = 2;
    public ObjectPool<Bubble> pool { get; set; }
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
        bigger.transform.DOMove(contactPoint, duration);
        sequence.Append(smaller.transform.DOMove(contactPoint, duration));
        sequence.AppendCallback(() =>
        {
            // Destroy smaller bubble
            smaller.gameObject.SetActive(false);
            pool.Release(smaller);
        });
        sequence.Append(bigger.transform.DOScale(newBigScale, duration));
        sequence.OnComplete(() =>
        {
            bigger.Enable(true);
        });

    }
}
