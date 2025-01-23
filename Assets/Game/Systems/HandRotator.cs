using UnityEngine;

public class HandRotator : MonoBehaviour
{
    public Transform pointer;
    public float rotationSpeed = 1;

    public SphereDetector detector;

    private ClosestFilter filter = new();

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
