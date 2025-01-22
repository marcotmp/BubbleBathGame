using UnityEngine;

public class MouseAndRotationTest : MonoBehaviour
{
    public Transform target;
    public Transform sphere;

    // Variables para rastrear la posici�n actual y anterior del mouse
    private Vector3 previousMousePosition;
    public Camera cam;
    private Vector3 currentMousePosition;

    // Velocidad de rotaci�n interpolada
    public float rotationSpeed = 5f;

    void Start()
    {
        // Inicializar la posici�n del mouse
        previousMousePosition = Input.mousePosition;
    }

    void Update()
    {
        // Actualizar la posici�n actual del mouse
        currentMousePosition = Input.mousePosition;

        // Calcular el vector entre la posici�n actual y la anterior
        Vector3 direction = currentMousePosition - previousMousePosition;

        //if (direction != Vector3.zero) // Evitar rotaci�n si no hay movimiento
        {
            // Obtener la normal del vector de direcci�n
            Vector3 normal = direction.normalized;

            // Convertir la normal a un vector en el espacio del mundo
            Vector3 worldDirection = cam.ScreenToWorldPoint(new Vector3(currentMousePosition.x, currentMousePosition.y, 10f));
            sphere.position = worldDirection;

            // Calcular la rotaci�n objetivo
            Quaternion targetRotation = Quaternion.LookRotation(normal, Vector3.up);

            // Interpolar hacia la rotaci�n objetivo
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Actualizar la posici�n anterior del mouse
        previousMousePosition = currentMousePosition;
    }
}