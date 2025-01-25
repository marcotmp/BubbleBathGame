using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BubbleDestructor : MonoBehaviour
{
    public float delay = 0.1f;
    public float popSize = 1.5f;
    public BubbleSpawner spawner;
    public UnityEvent<Bubble> bubbleDestroyed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bubble bubble))
        {
            bubble.onScaleComplete = () =>
            {
                // notify player point
                bubbleDestroyed?.Invoke(bubble);
            };
            bubble.Explode();
            //StartCoroutine(BubbleDestructorCoroutine(bubble));
        }
    }

    private IEnumerator BubbleDestructorCoroutine(Bubble bubble)
    {
        if (bubble.CanMerge)
        {
            // disable bubble
            bubble.Enable(false);

            var bubbleScale = bubble.transform.localScale.x;

            bubble.transform.DOScale(bubbleScale * popSize, delay);
            yield return new WaitForSeconds(delay);

            if (CompareTag("Hand"))
            {
                //notify player point
                bubbleDestroyed?.Invoke(bubble);
            }

            spawner.ReleaseBubble(bubble);
        }
    }
}
