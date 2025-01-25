using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class Bubble : MonoBehaviour
{
    public float defaultSize = 0.1f;
    public BubbleMerger merger;
    public BubbleSpawner spawner;
    [SerializeField] private Rigidbody rb;

    [Range(0, 4)]
    public int sizeId;

    public BubbleData bubbleData;
    public float scaleDuration;

    public bool CanMerge { get; set; } = true;

    public float mergeMinMagnitude = 0.1f;
    internal Action onScaleComplete;
    public float attractionFactor = 0.1f;


    [Header("Explode Settings")]
    public float popSize = 1.5f;
    public float delay = 0.1f;

    public bool exploding = false;

    public GameObject explodingSoundPrefab;

    private void Start()
    {
        transform.localScale = Vector3.one * bubbleData.GetScale(1);
        SetSizeId(0);
    }

    private void OnValidate()
    {
        transform.localScale = Vector3.one * bubbleData.GetScale(sizeId);
        SetSizeId(sizeId);        
    }

    public void SetSizeId(int sizeId)
    {
        // Explode if size is too big
        if (sizeId >= bubbleData.Length) 
        {
            Explode();
            return;
        }

        this.sizeId = sizeId;

        var data = bubbleData.GetDataList(sizeId);
        var tween = transform.DOScale(data.scale, scaleDuration);
        tween.onComplete = () => onScaleComplete();

        rb.mass = bubbleData.GetMass(sizeId);
    }

    public float GetScale()
    {
        var data = bubbleData.GetDataList(sizeId);
        return data.scale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CanMerge) return;

        if (other.TryGetComponent(out Bubble otherBubble))
        {
            //Debug.Log($"Colliding with Bubble Type {collision.gameObject.name}");
            var dir = otherBubble.transform.position - transform.position;
            var normal = dir.normalized;

            AddForce(normal * 0.1f);
        }
    }

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
            else
            {
                //Debug.Log($"Colliding with Bubble Type {collision.gameObject.name}");
                var normal = dir.normalized;

                AddForce(normal * attractionFactor);
            }
        }
    }

    internal void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    internal void AddForceClamped(Vector3 force)
    {
        var magnitude = rb.linearVelocity.magnitude;

        var vel = rb.linearVelocity + force;

        var newVel = vel * magnitude;
        rb.linearVelocity = newVel;
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
        return bubbleData.GetScore(sizeId);
    }

    internal bool IsFinalScale()
    {
        return sizeId == bubbleData.Length - 1;
    }



    public void Explode()
    {
        if (!exploding)
        {
            // disable bubble
            Enable(false);

            var bubbleScale = transform.localScale.x;

            Instantiate(explodingSoundPrefab);

            var tween = transform.DOScale(bubbleScale * popSize, delay);
            tween.onComplete = () => 
            {
                spawner.ReleaseBubble(this);
                onScaleComplete?.Invoke();
            };
        }

    }

    internal AudioClip GetLaughFx()
    {
        var data = bubbleData.GetDataList(sizeId);
        return data.laughSFX;
    }
}
