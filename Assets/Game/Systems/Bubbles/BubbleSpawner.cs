using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BubbleSpawner : MonoBehaviour
{
    public Bubble bubblePrefab;
    public BubbleMerger merger;
    public Transform parent;
    public int maxCapacity = 10;
    private ObjectPool<Bubble> pool;

    private static int i = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //pool = new ObjectPool<Bubble>(Create, OnGet, OnRelease, OnDestroyObject, true, 10, maxCapacity);
        merger.spawner = this;
    }

    private Bubble Create()
    {
        var bubbleInstance = Instantiate(bubblePrefab);
        bubbleInstance.transform.parent = parent;
        bubbleInstance.merger = merger;
        bubbleInstance.spawner = this;
        bubbleInstance.name = "Bubble " + i++;
        return bubbleInstance;
    }

    private void OnGet(Bubble bubble)
    {
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

    public void ReleaseBubble(Bubble bubble)
    {
        //pool.Release(bubble);
        Destroy(bubble.gameObject);
    }

    public Bubble GetBubble()
    {
        //return pool.Get();
        return Create();
    }
}
