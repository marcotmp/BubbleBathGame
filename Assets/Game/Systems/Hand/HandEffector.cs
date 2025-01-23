using UnityEngine;

public class HandEffector : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;
    public float radius;
    private Vector3 framePointerVelocity;
    private Vector3 normalVelocity;
    private Vector3 oldWorldPosition;
    public float velocityScale = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = radius;
    }

    private void OnValidate()
    {
        if (sphereCollider != null)
        {
            sphereCollider.radius = radius;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bubble bubble))
        {
            Debug.Log("Hand touching bubble " + bubble.name);

            // add ball to list of affected balls
            // apply air force

            var touchPoint = other.ClosestPoint(bubble.transform.position);
            var distance = other.transform.position - bubble.transform.position;


        }
    }

    private void OnTriggerExit(Collider other)
    {
        // remove ball from list of affected balls
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate hand velocity this frame
        framePointerVelocity = transform.position - oldWorldPosition;

        // calculate normal
        var pointerDirection = framePointerVelocity.normalized;
        var pointerDirectionNormal = Vector3.Cross(pointerDirection, Camera.main.transform.forward);

        // calculate alwaysUpNormal
        // Calcular la rotación objetivo
        var alwaysUpNormal = pointerDirectionNormal;
        // Garantizar que el vector resultante apunte hacia arriba
        if (Vector3.Dot(pointerDirectionNormal, Vector3.up) < 0)
        {
            alwaysUpNormal = -pointerDirectionNormal;
        }


        // do rotation


        // Calculate vectors based on velocity
        var velocityMagnitude = framePointerVelocity.magnitude;
        normalVelocity = pointerDirectionNormal * velocityMagnitude * velocityScale;

        oldWorldPosition = transform.position;
    }

    private void FixedUpdate()
    {
        CheckCollision();
    }

    [Header("RayDetection")]
    public float maxDistance = 100;
    public LayerMask layerMask;
    public Transform collisionPoint;

    public AnimationCurve curve;
    public float forceScale = 0.1f;

    public float currentForceMagnitude = 0;

    private void CheckCollision() 
    {
        // if hand is moving, process collision
        if (framePointerVelocity.magnitude > 0)
        {
            Vector3 rayOrigin = oldWorldPosition;
            Vector3 rayDirection = normalVelocity;

            var isDetected = Physics.Raycast(
                rayOrigin,
                rayDirection,
                out RaycastHit hitInfo,
                maxDistance,
                layerMask);
            if (isDetected)
            {
                Debug.Log($"Hit Object: {hitInfo.collider.gameObject.name}");
                Debug.Log($"Hit Point: {hitInfo.point}");

                collisionPoint.position = hitInfo.point;

                var objTransform = hitInfo.collider.transform;

                if (hitInfo.collider.TryGetComponent(out Rigidbody rb))
                {
                    var touchDirection = objTransform.position - hitInfo.point;
                    var touchDirectionNormalized = touchDirection.normalized;
                    var force = touchDirectionNormalized /** normalVelocity.magnitude*/ * forceScale;

                    currentForceMagnitude = force.magnitude;

                    // calculate distance percentage
                    var distanceHandToBubble = hitInfo.point - transform.position;
                    var distancePercentage = distanceHandToBubble.magnitude / maxDistance;
                    // end


                    var adjustedForceMagnitude = curve.Evaluate(distancePercentage) * currentForceMagnitude;
                    var adjustedForceVector = force.normalized * adjustedForceMagnitude;

                    rb.AddForce(adjustedForceVector);
                }

                // Visualize the raycast in the Scene view
                //Debug.DrawRay(rayOrigin, rayDirection * hitInfo.distance, Color.green);
            }
            else
            {
                // Visualize the raycast when it doesn't hit anything
                //Debug.DrawRay(rayOrigin, rayDirection * maxDistance, Color.red);
            }

        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, radius);
    }
}
