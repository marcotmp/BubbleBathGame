using System.Collections.Generic;
using UnityEngine;

public class HandRotator1 : MonoBehaviour
{
    public Transform pointer;
    public GameObject inverseEffector;
    public float rotationSpeed = 1;

    private Vector3 velocity;
    private Vector3 oldPosition;
    private float handVelocityMagnitude;
    private Vector3 pointerDirectionNormal;
    public bool isDebugOn = false;

    private struct DataPoint
    {
        public Vector3 position;
        public Vector3 direction;
        public Vector3 normal;
        public Vector3 normalVelocity;
    }

    private Queue<DataPoint> queue = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inverseEffector.SetActive(true);
    }

    private void Update()
    {
        // Calculate hand velocity this frame
        velocity = transform.position - oldPosition;
        handVelocityMagnitude = velocity.magnitude;

        // get normal vector of the direction
        var pointerDirection = velocity.normalized;
        pointerDirectionNormal = Vector3.Cross(pointerDirection, transform.forward);

        // Calcular la rotación objetivo
        var alwaysUpNormal = pointerDirectionNormal;
        // Garantizar que el vector resultante apunte hacia arriba
        if (Vector3.Dot(pointerDirectionNormal, Vector3.up) < 0)
        {
            alwaysUpNormal = -pointerDirectionNormal;
        }

        // rotate pointer to closest hit
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, alwaysUpNormal);

        //pointer.rotation = targetRotation;
        pointer.rotation = Quaternion.Slerp(pointer.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // store this velocity as old velocity
        oldPosition = transform.position;

        queue.Enqueue(new DataPoint()
        {
            position = transform.position,
            direction = pointerDirection,
            normal = alwaysUpNormal,
            normalVelocity = velocity.normalized,
        });

        if (queue.Count > 100)
        {
            queue.Dequeue();
        }
    }

    private void OnDrawGizmos()
    {

        if (!isDebugOn) return;

        foreach (var item in queue)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(item.position, item.position + item.direction);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(item.position, item.position + item.normal);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(item.position, item.position + item.normalVelocity);
        }
    }
}
