using UnityEngine;

public class RangeRotator : MonoBehaviour
{
    public float startAngle = 0;
    public float endAngle = 90;
    public float duration = 1;
    private float rotationSpeed;

    public Transform barrel;

    public bool autoStart = true;

    public float Value => value;

    private float angle = 0;
    private float value = 0;
    private float direction;

    // Start is called before the first frame update
    void Start()
    {
        if (autoStart)
            StartAiming();
    }

    public void StartAiming()
    {
        direction = (endAngle - startAngle) > 0 ? 1 : -1;
        angle = startAngle;
        rotationSpeed = 1 / duration;
        value = 0;
    }

    void Update()
    {
        if (value <= 1)
        {
            value += rotationSpeed * Time.deltaTime;
            angle = Mathf.Lerp(startAngle, endAngle, value);
            barrel.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}