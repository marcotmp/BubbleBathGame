using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandRotator : MonoBehaviour
{
    public Transform pointer;
    public float rotationSpeed = 1;

    public SphereDetector detector;

    public int maxAverage = 4;

    private ClosestFilter filter = new();
    private Vector3 averagePosition;
    private Vector3 directionNormal;
    private List<RaycastHit> closestHit;

    private void FixedUpdate()
    {
        detector.Detect();

        var hits = detector.hits.ToList();

        if (hits.Count > 0)
        {
            ProcessHits2(hits);
        }
    }

    private void ProcessHits2(List<RaycastHit> hits)
    {
        // find 3 closest hit
        closestHit = filter.GetClosestHits(hits, pointer.position, maxAverage);

        Vector3 sumPosition = Vector3.zero;
        foreach (var hit in closestHit)
        {
            sumPosition += hit.transform.position;
        }
        averagePosition = sumPosition / hits.Count;

        // rotate pointer to closest hit
        var closestDirection = averagePosition - pointer.position;
        directionNormal = closestDirection.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionNormal);

        //pointer.rotation = targetRotation;
        pointer.rotation = Quaternion.Slerp(pointer.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void ProcessHits(List<RaycastHit> hits)
    {
        // find closest hit
        var closestHit = filter.GetClosestHit(hits, pointer.position);

        // rotate pointer to closest hit
        var closestDirection = closestHit.transform.position - pointer.position;
        var directionNormal = closestDirection.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionNormal);

        //pointer.rotation = targetRotation;
        pointer.rotation = Quaternion.Slerp(pointer.rotation, targetRotation, Time.deltaTime * rotationSpeed);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(averagePosition, 0.1f);

        Gizmos.DrawRay(transform.position, directionNormal * 5);

        if (closestHit != null)
        {
            foreach (var hit in closestHit)
            {
                Gizmos.color = Color.green;
                if (hit.transform != null)
                    Gizmos.DrawWireSphere(hit.transform.position, 0.1f);
            }
        }
    }
}

public class ClosestFilter
{
    private float shortestDistance;
    private int shortestIndex = 0;

    public RaycastHit GetClosestHit(List<RaycastHit> hits, Vector3 targetPosition)
    {
        if (hits.Count == 0) return default;

        shortestDistance = float.MaxValue;
        shortestIndex = 0;

        for (int i = 0; i < hits.Count; i++)
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

    internal List<RaycastHit> GetClosestHits(List<RaycastHit> hits, Vector3 position, int qty = 3)
    {
        var list = new List<RaycastHit>(hits);
        var finalList = new List<RaycastHit>();

        for (int i = 0; i < qty; i++)
        {
            if (list.Count > 0)
            {
                var hit = GetClosestHit(list, position);
                finalList.Add(hit);
                list.Remove(hit);
            }
        }

        return finalList;
    }
}
