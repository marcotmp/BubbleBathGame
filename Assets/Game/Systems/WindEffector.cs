using UnityEngine;

public class WindEffector : MonoBehaviour
{
    public float forceScale = 1;
    public float maxDistance = 1;
    public AnimationCurve curve;

    private Vector3 oldPosition;

    private Vector3 velocity;
    private float handVelocityMagnitude;
    private Vector3 pointerDirectionNormal;

    //private Queue<Vector3> queue = new();

    private void Update()
    {
        // Calculate hand velocity this frame
        velocity = transform.position - oldPosition;
        handVelocityMagnitude = velocity.magnitude;

        // get normal vector of the direction
        var pointerDirection = velocity.normalized;
        pointerDirectionNormal = Vector3.Cross(pointerDirection, transform.forward);

        //queue.Enqueue(velocity);

        //if (queue.Count > 10)
        //    queue.Dequeue();

    }

    private void OnTriggerEnter(Collider other)
    {
        ProcessContact(other);
    }

    private void OnTriggerStay(Collider other)
    {
        ProcessContact(other);
    }

    private void ProcessContact(Collider other)
    { 
        // affect all the objects touched based on the velocity 
        // convert velocity into a force

        if (other.TryGetComponent(out Bubble bubble))
        {
            // find wind normal
            var normal = transform.up.normalized;

            // find distance between wind effector and bubble
            var distance = transform.position - bubble.transform.position;
            var distanceMagnitude = distance.magnitude;

            // distance factor is greater the less distance
            var distanceFactor = distanceMagnitude / maxDistance;
            var inverseDistanceFactor = 1 - distanceFactor;
            var finalDistanceFactor = curve.Evaluate(inverseDistanceFactor);

            var force = normal * handVelocityMagnitude * forceScale * finalDistanceFactor;

            bubble.AddForce(force);
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

    //private void OnDrawGizmos()
    //{
    //    foreach (var item in queue)
    //    {
    //        var normal = item.normalized;
    //        Gizmos.DrawLine(transform.position, transform.position + normal);
    //    }
    //}
}
