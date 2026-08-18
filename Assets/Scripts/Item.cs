using System;
using UnityEngine;

[Serializable]
public class Item
{
    public int id;
    public int maxStack;
    public string name;
}

[System.Serializable]
public class ItemList
{
    public Item[] items;
}
