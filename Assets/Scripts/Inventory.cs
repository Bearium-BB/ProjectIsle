using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<InventorySlot> InventorySlots = new List<InventorySlot>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddInventory(Item item, int amount)
    {
        InventorySlots.Add(new InventorySlot(item, amount));
        foreach (InventorySlot slot in InventorySlots)
        {
            Debug.Log(slot.item.name + " " + slot.amount);
        }
    }
}


public class InventorySlot
{
    public InventorySlot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
    public Item item;
    public int amount;
}