using UnityEngine;

public class MouseAndRotationTest : MonoBehaviour
{
    public Transform target;
    public Transform sphere;

    // Variables para rastrear la posición actual y anterior del mouse
    private Vector3 previousMousePosition;
    public Camera cam;
    private Vector3 currentMousePosition;

    // Velocidad de rotación interpolada
    public float rotationSpeed = 5f;

    void Start()
    {
        // Inicializar la posición del mouse
        previousMousePosition = Input.mousePosition;
    }

    void Update()
    {
        // Actualizar la posición actual del mouse
        currentMousePosition = Input.mousePosition;

        // Calcular el vector entre la posición actual y la anterior
        Vector3 direction = currentMousePosition - previousMousePosition;

        //if (direction != Vector3.zero) // Evitar rotación si no hay movimiento
        {
            // Obtener la normal del vector de dirección
            Vector3 normal = direction.normalized;

            // Convertir la normal a un vector en el espacio del mundo
            Vector3 worldDirection = cam.ScreenToWorldPoint(new Vector3(currentMousePosition.x, currentMousePosition.y, 10f));
            sphere.position = worldDirection;

            // Calcular la rotación objetivo
            Quaternion targetRotation = Quaternion.LookRotation(normal, Vector3.up);

            // Interpolar hacia la rotación objetivo
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Actualizar la posición anterior del mouse
        previousMousePosition = currentMousePosition;
    }
}