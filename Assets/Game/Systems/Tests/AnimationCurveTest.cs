using UnityEngine;

public class AnimationCurveTest : MonoBehaviour
{

    public Transform obj;

    public AnimationCurve curve;

    public float scale;

    [Range(0, 1)]
    public float slider;

    // Update is called once per frame
    void Update()
    {
        var result = curve.Evaluate(slider) * scale;

        obj.position = new Vector3(0, result, 0);
    }
}
