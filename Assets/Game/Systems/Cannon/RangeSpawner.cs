using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RangeSpawner : MonoBehaviour
{
    public UnityEvent onAction;
    public Transform target;
    public Vector2 range;

    public float initialDelay = 0;
    public float minDelay = 1;
    public float maxDelay = 1;
    private Vector3 size;

    private void Start()
    {
        StartCoroutine(DoLoop());
    }

    private IEnumerator DoLoop()
    {
        yield return new WaitForSeconds(initialDelay);
        while (true) 
        {
            Spawn();
            var delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    private void Spawn()
    {
        size = new Vector3(
            Random.Range(-range.x, range.x),
            Random.Range(-range.y, range.y)
        );

        //var finalPos = transform.TransformPoint(size);
        //target.position = finalPos;

        target.position = transform.position + size;

        onAction?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix.SetTRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, range * 2);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size * 2);
    }
}
