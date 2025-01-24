using UnityEngine;

public class BubbleCannon : MonoBehaviour
{
    public float speedFactor = 1;
    public float maxSpeedFactor = 1;
    public float size = 0.1f;
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

        //bubble.SetSize(size);
        bubble.SetSize(finalSpeedFactor);

        bubble.AddForce(transform.right * finalSpeedFactor);
    }
}
