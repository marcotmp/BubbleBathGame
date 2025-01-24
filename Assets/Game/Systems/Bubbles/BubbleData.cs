using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BubbleData", menuName = "Bubbles/BubbleData")]
public class BubbleData : ScriptableObject
{
    public List<SizeData> dataList;
}

[System.Serializable]
public struct SizeData
{
    public int id;
    public float scale;
    public int score;
}
