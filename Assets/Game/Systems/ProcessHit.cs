using System.Collections.Generic;
using UnityEngine;

public class ProcessHit : MonoBehaviour
{
    public Transform pointer;
    public float rotationSpeed = 1;

    public SphereDetector detector;

    [Range(0, 1)]
    public float dotRange = 1;

    public Queue<LineData> dataQueue = new();

    private ClosestFilter filter = new();

    public struct LineData
    {
        public Vector3 position;
        public bool isHit;
        internal Vector3 otherNormal;
        internal Vector3 pointerNormal;
        internal float dot;
    }

    private void FixedUpdate()
    {
        detector.Detect();

        var hits = detector.hits;

        if (hits.Length > 0)
        {
            // find closest hit
            var closestHit = filter.GetClosestHit(hits, pointer.position);

            // rotate pointer to closest hit
            var closestDirection = closestHit.transform.position - pointer.position;
            var directionNormal = closestDirection.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionNormal);

            //pointer.rotation = targetRotation;
            pointer.rotation = Quaternion.Slerp(pointer.rotation, targetRotation, Time.deltaTime * rotationSpeed);


            //foreach (var hit in detector.hits)
            //{
            //    ProcessHit1(hit);
            //}
        }
    }

    private void ProcessHit1(RaycastHit hit)
    {
        var isHit = false;

        // calculate pointer normal
        var pointerNormal = pointer.up;

        // calculate other normal
        var difference = hit.transform.position - pointer.position;
        var otherNormal = difference.normalized;

        var dot = Vector3.Dot(pointerNormal, otherNormal);
        isHit = dot > dotRange;

        var lineData = new LineData()
        {
            isHit = isHit,
            dot = dot,
            position = hit.collider.transform.position,
            pointerNormal = pointerNormal,
            otherNormal = otherNormal
        };

        dataQueue.Enqueue(lineData);

        if (dataQueue.Count > 10)
        {
            dataQueue.Dequeue();
        }
    }

    private void OnDrawGizmos()
    {
        // draw a line between this and the hit object
        foreach (LineData data in dataQueue)
        {
            Gizmos.color = data.isHit ? Color.red : Color.white;

            Gizmos.DrawLine(transform.position, data.position);


            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + data.pointerNormal * 0.25f);
            //Gizmos.DrawLine(data.position, data.position + data.otherNormal * 0.25f);
        }
    }
}

public class ClosestFilter
{
    private float shortestDistance;
    private int shortestIndex = 0;

    public RaycastHit GetClosestHit(RaycastHit[] hits, Vector3 targetPosition)
    {
        shortestDistance = float.MaxValue;
        shortestIndex = 0;

        for (int i = 0; i < hits.Length; i++)
        {
            // compare pointer position with hit position
            var hit = hits[i];
            var distanceVector = targetPosition - hit.transform.position;
            var distance = distanceVector.magnitude;

            if (distance < shortestDistance)
            {
                shortestDistance = distanceVector.magnitude;
                shortestIndex = i;
            }
        }

        return hits[shortestIndex];
    }
}
