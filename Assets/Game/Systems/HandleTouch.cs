using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class HandleTouch : MonoBehaviour
{
    public Transform handGroup;
    public Transform handModel;
    public float zDistance = 10;
    public float velocityScale = 100;
    public float rotationSpeed = 1;

    private Camera cam;
    private Vector3 oldWorldPosition;

    [Header("Debug Vars")]
    public Vector3 framePointerVelocity;
    public Transform crossVisual1;
    public Transform crossVisual2;
    public float maxMagnitudeDelta = 1;

    [Header("ForwardPoint")]
    public Vector3 normalVelocity;
    public float externalAngle = 0;

    private void Start()
    {
        cam = Camera.main;
        oldWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * zDistance);
    }

    private struct DataPoint
    {
        public Vector3 position;
        public Vector3 direction;
        public Vector3 normal;
        public Vector3 normalVelocity;
    }

    private Queue<DataPoint> queue = new();

    void Update()
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

        // Calcular la rotación objetivo

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, pointerDirectionNormal);

        // Interpolar hacia la rotación objetivo
        handModel.rotation = Quaternion.Slerp(handModel.rotation, targetRotation, Time.deltaTime * rotationSpeed);


        // Calculate vectors based on velocity
        var velocityMagnitude = framePointerVelocity.magnitude;
        normalVelocity = pointerDirectionNormal * velocityMagnitude * velocityScale;

        // debug pointer direction
        //crossVisual1.position = newHandPosition + pointerDirectionNormal;
        //crossVisual2.position = newHandPosition - pointerDirectionNormal;

        // hand position
        handGroup.position = newHandPosition;

        queue.Enqueue(new DataPoint() 
        {
            position = newHandPosition,
            direction = pointerDirection,
            normal = pointerDirectionNormal,
            normalVelocity = normalVelocity,
        });

        if (queue.Count > 100) 
        {
            queue.Dequeue();
        }

        // store this velocity as old velocity
        oldWorldPosition = newHandPosition;
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
        //Gizmos.DrawLine(target.position, target.position - normalVelocity);
        //Gizmos.DrawLine(target.position, target.position - normalVelocity);

        foreach (var item in queue)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(item.position, item.position + item.direction);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(item.position, item.position + item.normal);

            //Gizmos.color = Color.red;
            //Gizmos.DrawLine(item.position, item.position + item.normalVelocity);
        }
    }
}
