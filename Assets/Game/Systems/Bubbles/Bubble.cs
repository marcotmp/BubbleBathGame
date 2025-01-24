using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float defaultSize = 0.1f;
    public BubbleMerger merger;
    [SerializeField] private Rigidbody rb;
    public int sizeId;
    public BubbleData bubbleData;

    public bool CanMerge { get; set; } = true;

    private void Start()
    {
        SetSizeId(1);
    }

    private void OnValidate()
    {
        SetSizeId(1);
    }

    public void SetSizeId(int sizeId)
    {
        this.sizeId = sizeId;

        var data = bubbleData.dataList[sizeId];
        
        transform.localScale = Vector3.one * data.scale;
    }

    public float GetScale()
    {
        var data = bubbleData.dataList[sizeId];
        return data.scale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CanMerge) return;

        if (other.TryGetComponent(out Bubble otherBubble))
        {
            //Debug.Log($"Colliding with Bubble Type {collision.gameObject.name}");
            //merger.MergeBubbles(this, otherBubble);

            merger.MoveCloser(this, otherBubble);
        }
    }

    public float mergeMinMagnitude = 0.1f;

    private void OnTriggerStay(Collider other)
    {
        if (!CanMerge) return;

        if (other.TryGetComponent(out Bubble otherBubble))
        {
            //Debug.Log($"Colliding with Bubble Type {collision.gameObject.name}");

            var dir = otherBubble.transform.position - transform.position;
            //var normal = dir.normalized;

            if (dir.magnitude < mergeMinMagnitude)
                merger.MergeBubbles(this, otherBubble);
        }
    }

    internal void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    internal void Enable(bool enable)
    {
        CanMerge = enable;
        //GetComponent<Rigidbody>().isKinematic = !enable;
        //GetComponent<Collider>().enabled = enable;
    }

    internal void ResetVelocity()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, mergeMinMagnitude);
    }

    internal int GetScore()
    {
        return bubbleData.dataList[sizeId].score;
    }
}
