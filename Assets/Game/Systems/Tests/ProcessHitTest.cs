using System.Collections.Generic;
using UnityEngine;

public class ProcessHitTest : MonoBehaviour
{
    public Transform pointer;
    [Range(0, 1)]
    public float dotRange = 1;
    public struct LineData
    {
        public Vector3 position;
        public bool isHit;
        internal Vector3 otherNormal;
        internal Vector3 pointerNormal;
        internal float dot;
    }


    public Queue<LineData> dataQueue = new();

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
