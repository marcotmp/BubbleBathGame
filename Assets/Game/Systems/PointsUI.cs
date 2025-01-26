using TMPro;
using UnityEngine;

public class PointsUI : MonoBehaviour
{
    public TMP_Text text;
    public float delay = 1;

    public void Start()
    {
        Destroy(gameObject, delay);
    }

    public void SetScore(int score)
    {
        text.text = "+ " + score.ToString();
    }
}
