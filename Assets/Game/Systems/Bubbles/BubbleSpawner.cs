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

    private ObjectPool<Bubble> pool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pool = new ObjectPool<Bubble>(Create, OnGet, OnRelease, OnDestroyObject);
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
            yield return new WaitForSeconds(1);
        }
    }

    private Bubble Create()
    {
        var obj = Instantiate(bubblePrefab);
        obj.GetComponent<Rigidbody>().MovePosition(spawnPoint.position);
        obj.transform.position = spawnPoint.position;
        obj.transform.parent = parent;
        obj.gameObject.SetActive(false);
        obj.merger = merger;
        return obj;
    }

    private void OnGet(Bubble obj)
    {
        obj.GetComponent<Rigidbody>().MovePosition(spawnPoint.position);
        obj.transform.position = spawnPoint.position;
        //obj.gameObject.SetActive(true);
    }

    private void OnRelease(Bubble obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyObject(Bubble obj)
    {
        Destroy(obj);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(spawnPoint.position, new Vector3(width, 0.1f, 0.1f));
    }
}
