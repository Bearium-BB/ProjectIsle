using System;
using UnityEngine;

[Serializable]
public class Item
{
    public int id;
    public int idTexture;
    public int maxStack;
    public string name;
}

[Serializable]
public class ItemList
{
    public Item[] items;
}
