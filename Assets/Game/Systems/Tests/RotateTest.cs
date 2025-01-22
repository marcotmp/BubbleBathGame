using UnityEngine;

public class RotateTest : MonoBehaviour
{
    public Transform handModel;
    public float angle;
    public float rotationSpeed;

    [Header("Debug")]
    public float interpolatedAngle;
    [Range(0, 1)]
    public float interpolatedValue = 1;

    // Update is called once per frame
    void Update()
    {
        // Direction to the target
        Vector3 directionToTarget = transform.position - handModel.position;

        // Calculate the desired rotation to face the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly rotate towards the target
        handModel.rotation = Quaternion.RotateTowards(
            handModel.rotation,     
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

    }

    private void Rotate2() 
    { 
        //var currentAngle = handModel.rotation.z;
        //currentAngle += Time.deltaTime * rotationSpeed;

    }

    private void Rotate1()
    {
        //var interpolatedAngle = Mathf.MoveTowards(handModel.rotation.z, angle, Time.deltaTime * rotationSpeed);
        interpolatedAngle = Mathf.Lerp(handModel.rotation.z, angle, interpolatedValue);

        var targetRotation = Quaternion.Euler(0, 0, interpolatedAngle);

        //handModel.rotation = targetRotation;

        handModel.Rotate(0, 0, Time.deltaTime * rotationSpeed);



    }
}
