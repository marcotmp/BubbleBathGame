using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public Vector3 gravity = new Vector3(0, 0.5f);

    private Rigidbody rb;
    private Vector3 defaultGravity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultGravity = Physics.gravity;
    }

    private void FixedUpdate()
    {
        // Apply custom gravity
        rb.AddForce(-defaultGravity + gravity, ForceMode.Acceleration);
    }
}
