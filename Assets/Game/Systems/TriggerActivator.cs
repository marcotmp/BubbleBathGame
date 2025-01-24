using UnityEngine;
using UnityEngine.Events;

public class TriggerActivator : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Activator Enter");
        onTriggerEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Activator Exit");
        onTriggerExit?.Invoke();
    }
}
