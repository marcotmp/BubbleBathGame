using UnityEngine;

public class RangeRotator2 : MonoBehaviour
{
    public Transform target;
    public Transform parentTransform;

    public float minAngle;
    public float maxAngle;

    public void Rotate()
    {
        var angle = Random.Range(minAngle, maxAngle);

        //Vector3 v = Quaternion.AngleAxis(angle, parentTransform.forward) * parentTransform.right;

        //target.rotation = Quaternion.Euler(v);        

        target.rotation = Quaternion.Euler(0, 0, angle);
    }
}
