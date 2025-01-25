using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "BubbleData", menuName = "Bubbles/BubbleData")]
public class BubbleData : ScriptableObject
{
    public float initialMass = 0.02f;
    public float initialScale = 0.1f;
    [SerializeField] private List<SizeData> dataList;

    public int Length => dataList.Count;

    public SizeData GetDataList(int id) 
    {
        if (id < dataList.Count)
            return dataList[id];
        return default; 
    }

    public float GetMass(int id)
    {
        var mass = initialMass;

        if (id < dataList.Count)
            mass = initialMass * dataList[id].massFactor;

        return mass;
    }

    public float GetScale(int id)
    {
        var scale = initialScale;
        
        if (id < dataList.Count)
            scale = dataList[id].scale;

        return scale;
    }

    internal int GetScore(int id)
    {
        if (id < dataList.Count)
            return dataList[id].score;

        return 0;
    }
}

[System.Serializable]
public struct SizeData
{
    public int id;
    public float scale;
    public int score;
    public float massFactor;
}
