using UnityEngine;

public class BubbleCannon : MonoBehaviour
{
    public float speedFactor = 1;
    public float maxSpeedFactor = 1;
    public int minSizeId = 0;
    public int maxSizeId = 1;
    [SerializeField] private BubbleSpawner spawner;

    private void Start()
    {
        spawner = FindFirstObjectByType<BubbleSpawner>();
    }

    [ContextMenu("Shoot")]
    public void Shoot()
    {
        var bubble = spawner.GetBubble();
        bubble.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        bubble.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        bubble.GetComponent<Rigidbody>().position = transform.position;
        bubble.transform.position = transform.position;

        bubble.Enable(true);
        bubble.gameObject.SetActive(true);

        var finalSpeedFactor = Random.Range(speedFactor, maxSpeedFactor);

        int sizeId = Random.Range(minSizeId, maxSizeId);
        bubble.SetSizeId(sizeId);

        bubble.AddForce(transform.right * finalSpeedFactor);
    }
}
