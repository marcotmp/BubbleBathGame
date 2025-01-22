using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class HandleTouch : MonoBehaviour
{
    public Transform target;
    public float zDistance = 10;
    public float velocityScale = 100;

    private Camera cam;
    private Vector3 oldWorldPosition;

    [Header("Debug Vars")]
    public Vector3 framePointerVelocity;
    public Transform crossVisual1;
    public Transform crossVisual2;
    public float maxMagnitudeDelta = 1;

    [Header("ForwardPoint")]
    public Vector3 normalVelocity;

    private void Start()
    {
        cam = Camera.main;
        oldWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * zDistance);
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        {
            // Get Mouse Position
            var mousePos = Input.mousePosition;
            mousePos.z = zDistance;
            Vector3 newHandPosition = cam.ScreenToWorldPoint(mousePos);

            // Calculate velocity this frame
            framePointerVelocity = newHandPosition - oldWorldPosition;

            // get normal vector of the direction
            var pointerDirection = framePointerVelocity.normalized;
            var pointerDirectionNormal = Vector3.Cross(pointerDirection, cam.transform.forward);

            // Rotate hand

            // Calculate the angle between the current forward and the target direction
            float targetAngle = Mathf.Atan2(pointerDirectionNormal.y, pointerDirectionNormal.x) * Mathf.Rad2Deg;

            // if not moving, do not rotate
            if (oldWorldPosition.x != newHandPosition.x && oldWorldPosition.y != newHandPosition.y)
                target.rotation = Quaternion.Euler(0, 0, targetAngle + 90);



            // Calculate vectors based on velocity
            var velocityMagnitude = framePointerVelocity.magnitude;
            normalVelocity = pointerDirectionNormal * velocityMagnitude * velocityScale;

            // debug pointer direction
            crossVisual1.position = newHandPosition + pointerDirectionNormal;
            crossVisual2.position = newHandPosition - pointerDirectionNormal;

            // hand position
            target.position = newHandPosition;

            // store this velocity as old velocity
            oldWorldPosition = newHandPosition;
        }
    }

    [Header("RayDetection")]
    public float maxDistance = 100;
    public LayerMask layerMask;
    public Transform collisionPoint;

    public AnimationCurve curve;
    public float forceScale = 0.1f;

    private void FixedUpdate()
    {
        // check collisions
        CheckCollision(normalVelocity);
        CheckCollision(-normalVelocity);
    }

    private void CheckCollision(Vector3 normalVelocity)
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
                    rb.AddForce(force);
                }

                // Visualize the raycast in the Scene view
                Debug.DrawRay(rayOrigin, rayDirection * hitInfo.distance, Color.green);
            }
            else
            {
                // Visualize the raycast when it doesn't hit anything
                Debug.DrawRay(rayOrigin, rayDirection * maxDistance, Color.red);
            }

        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(target.position, target.position - normalVelocity);
        Gizmos.DrawLine(target.position, target.position - normalVelocity);
    }
}
