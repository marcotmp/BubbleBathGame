using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float defaultSize = 0.1f;
    public BubbleMerger merger;
    [SerializeField] private Rigidbody rb;

    public bool CanMerge { get; set; } = true;

    private void Start()
    {
        SetSize(defaultSize);
    }

    private void OnValidate()
    {
        SetSize(defaultSize);
    }

    private void OnEnable()
    {
        Debug.Log("Enabled");
    }

    private void OnDisable()
    {
        
    }

    public void SetSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!CanMerge) return;

        //Debug.Log($"Colliding with {collision.gameObject.name}");
        if (collision.gameObject.TryGetComponent(out Bubble otherBubble))
        {
            //Debug.Log($"Colliding with Bubble Type {collision.gameObject.name}");
            merger.MergeBubbles(this, otherBubble);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Bubble {name} is affected by " + other.name);
    }

    internal void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    internal void Enable(bool enable)
    {
        CanMerge = enable;
        GetComponent<Rigidbody>().isKinematic = !enable;
        GetComponent<Collider>().enabled = enable;
    }

    internal void ResetVelocity()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
