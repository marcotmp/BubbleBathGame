using UnityEngine;

public class HandleTouch : MonoBehaviour
{
    public Transform target;
    public float zDistance = 10;
    public Vector3 velocityPerFrame;

    private Camera cam;
    private Vector3 oldWorldPosition;


    private void Start()
    {
        cam = Camera.main;
        oldWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * zDistance);
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;
            mousePos.z = zDistance;
            Vector3 newWorldPosition = cam.ScreenToWorldPoint(mousePos);
            target.position = newWorldPosition;

            // Calculate velocity this frame
            velocityPerFrame = newWorldPosition - oldWorldPosition;

            // store this velocity as old velocity
            oldWorldPosition = newWorldPosition;
        }
    }
}
