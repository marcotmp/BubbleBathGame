using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BubbleSpawner : MonoBehaviour
{
    public Bubble bubblePrefab;
    public BubbleMerger merger;
    public Transform parent;
    public Transform spawnPoint;
    public float width = 1;
    public float delayBetweenCreate = 0.5f;
    public int maxCapacity = 10;
    private ObjectPool<Bubble> pool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pool = new ObjectPool<Bubble>(Create, OnGet, OnRelease, OnDestroyObject, true, 10, maxCapacity);
        merger.pool = pool;
        StartCoroutine(CreateCoroutine());
    }

    private IEnumerator CreateCoroutine()
    {
        while (true)
        {
            var instance = pool.Get();
            var halfWidth = width / 2;
            instance.transform.position = spawnPoint.position + new Vector3(Random.Range(-halfWidth, halfWidth), 0, 0);
            instance.SetSize(instance.defaultSize);
            instance.gameObject.SetActive(true);
            yield return new WaitForSeconds(delayBetweenCreate);
        }
    }

    private static int i = 0;

    private Bubble Create()
    {
        var bubbleInstance = Instantiate(bubblePrefab);
        bubbleInstance.GetComponent<Rigidbody>().MovePosition(spawnPoint.position);
        bubbleInstance.transform.position = spawnPoint.position;
        bubbleInstance.transform.parent = parent;
        bubbleInstance.gameObject.SetActive(false);
        bubbleInstance.merger = merger;
        bubbleInstance.name = "Bubble " + i++; 
        return bubbleInstance;
    }

    private void OnGet(Bubble bubble)
    {
        bubble.GetComponent<Rigidbody>().MovePosition(spawnPoint.position);
        bubble.transform.position = spawnPoint.position;

        bubble.ResetVelocity();
        bubble.Enable(true);
    }

    private void OnRelease(Bubble bubble)
    {
        bubble.gameObject.SetActive(false);
    }

    private void OnDestroyObject(Bubble bubble)
    {
        Destroy(bubble);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(spawnPoint.position, new Vector3(width, 0.1f, 0.1f));
    }

    public void PopBubble(Bubble bubble)
    {
        pool.Release(bubble);
    }
}
