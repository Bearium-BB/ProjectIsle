using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropList : MonoBehaviour
{

    public List<DropListData> data = new List<DropListData>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessDropList();
    }

    public List<Item> ProcessDropList()
    {
        List<Item> drops = new List<Item>();

        for (int i = 0; i < data.Count; i++)
        {
            float roll = Random.Range(0f, 1f);

            if (roll <= data[i].chanceOfGettingItem)
            {
                drops.Add(data[i].item);
            }
        }

        return drops;
    }
}


[Serializable]
public class DropListData
{
    public Item item;
    public float chanceOfGettingItem;
    public int amount;
}