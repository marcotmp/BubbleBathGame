using UnityEngine;

public class HandleTouch : MonoBehaviour
{
    public Transform target;
    public float zDistance = 10;
    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Get Mouse Position
        var mousePos = Input.mousePosition;
        mousePos.z = zDistance;
        Vector3 newHandPosition = cam.ScreenToWorldPoint(mousePos);
        target.position = newHandPosition;
    }
}
