using UnityEngine;
using UnityEngine.VFX;

public class WindAmountController : MonoBehaviour
{
    public VisualEffect vfx;

    public float amount = 3;
    [Range(5, 20)] public float speed = 10;
    void Update()
    {
        vfx.SetFloat("Speed", speed);
        vfx.SetFloat("Amount", amount);
    }
}
