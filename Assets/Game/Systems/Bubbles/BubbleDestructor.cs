using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BubbleDestructor : MonoBehaviour
{
    public float delay = 0.1f;
    public float popSize = 1.5f;
    public BubbleSpawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bubble bubble))
        {
            StartCoroutine(BubbleDestructorCoroutine(bubble));
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
            spawner.ReleaseBubble(bubble);
        }
    }
}
