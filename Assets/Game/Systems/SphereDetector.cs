using UnityEngine;

public class SphereDetector : MonoBehaviour
{
    public float radius = 1;
    public LayerMask layerMask;
    public RaycastHit[] hits;

    private float maxDistance;

    public void Detect()
    {
        hits = Physics.SphereCastAll(transform.position, radius, Vector3.forward, maxDistance, layerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
