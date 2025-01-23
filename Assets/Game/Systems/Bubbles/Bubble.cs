using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float defaultSize = 0.1f;
    public BubbleMerger merger;
    public bool CanMerge { get; set; } = true;

    private void Start()
    {
        SetSize(defaultSize);
    }

    private void OnValidate()
    {
        SetSize(defaultSize);
    }

    public void SetSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!CanMerge) { return; }

        //Debug.Log($"Colliding with {collision.gameObject.name}");
        if (collision.gameObject.TryGetComponent(out Bubble otherBubble))
        {
            //Debug.Log($"Colliding with Bubble Type {collision.gameObject.name}");
            merger.MergeBubbles(this, otherBubble);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Bubble {name} is affected by " + other.name);

        // from now on, be affected by hand
    }
}
