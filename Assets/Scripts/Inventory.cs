using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Inventory : MonoBehaviour
{
    InventorySlot[] inventorySlots;

    public int sizeOfInventory = 9;

    ItemManagerService itemManagerService = new ItemManagerService();

    public InventoryUIContainer[] inventoryUIContainers;

    public ServiceTexturerManager serviceTexturerManager;


    public GameObject UI;
    public GameObject ParentUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serviceTexturerManager = new ServiceTexturerManager();

        inventorySlots = new InventorySlot[sizeOfInventory];
        inventoryUIContainers = new InventoryUIContainer[sizeOfInventory];

        for (int i = 0; i < inventoryUIContainers.Length; i++)
        {
            inventoryUIContainers[i] = Instantiate(UI, ParentUI.transform).GetComponent<InventoryUIContainer>();
        }

        for (int i = 0; i < 50; i++) 
        {

            Item item = itemManagerService.GetRandomItem();

            if (item.id == 3 || item.id == 4)
            {
                continue;
            }

            AddInventory(item, UnityEngine.Random.Range(0,5));
        }

        foreach (InventorySlot test in inventorySlots)
        {
            if (test == null)
            {
                continue;
            }
            Debug.Log(test.item.name + " " + test.amount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddInventory(Item item, int amount)
    {
        while (amount > 0)
        {
            InventorySlot slot = inventorySlots
                .Where(x => x != null &&
                            x.item.id == item.id &&
                            x.amount < x.item.maxStack)
                .OrderByDescending(x => x.amount)
                .FirstOrDefault();

            if (slot != null)
            {
                int space = slot.item.maxStack - slot.amount;
                int amountToAdd = Mathf.Min(amount, space);

                slot.amount += amountToAdd;
                amount -= amountToAdd;
            }
            else
            {
                int index = inventorySlots
                    .Select((slot, index) => new { slot, index })
                    .Where(x => x.slot == null)
                    .Select(x => x.index)
                    .DefaultIfEmpty(-1)
                    .First();

                if (index == -1)
                {
                    Debug.Log("Inventory is full!");
                    return;
                }

                int amountToAdd = Mathf.Min(amount, item.maxStack);

                inventorySlots[index] = new InventorySlot(item, amountToAdd);

                amount -= amountToAdd;
            }
        }

        for (int i = 0; i < inventoryUIContainers.Length; i++)
        {
            if (inventorySlots[i] != null)
            {
                inventoryUIContainers[i].amount.text = inventorySlots[i].amount.ToString();
                inventoryUIContainers[i].name.text = inventorySlots[i].item.name;

                float uMin = 0;
                float uMax = 0;
                float vMin = 0;
                float vMax = 0;

                TextureAtlasTextureCoordinates textureCoordinates = serviceTexturerManager.GetTextureById(inventorySlots[i].item.idTexture);

                inventoryUIContainers[i].image.texture = textureCoordinates.texture;

                serviceTexturerManager.GetTileUV(textureCoordinates.textureSizeX, textureCoordinates.textureSizeY, textureCoordinates.tileSizeX, textureCoordinates.tileSizeY, textureCoordinates.texturesCoordinatesX, textureCoordinates.texturesCoordinatesY, out uMin, out uMax, out vMin, out vMax);

                inventoryUIContainers[i].image.uvRect = new Rect(
                    uMin,
                    vMin,
                    uMax - uMin,
                    vMax - vMin
                );
            }
        }
    }

    public bool HasItem(int itemId, int amount)
    {
        int totalAmount = inventorySlots
            .Where(slot => slot != null && slot.item.id == itemId)
            .Sum(slot => slot.amount);

        return totalAmount >= amount;
    }

    public bool RemoveItem(int itemId, int amount)
    {
        if (!HasItem(itemId, amount))
        {
            return false;
        }

        int remaining = amount;

        // Remove from slots until we've removed the requested amount
        for (int i = 0; i < inventorySlots.Length && remaining > 0; i++)
        {
            InventorySlot slot = inventorySlots[i];

            if (slot == null || slot.item.id != itemId)
            {
                continue;
            }

            int amountToRemove = Mathf.Min(slot.amount, remaining);

            slot.amount -= amountToRemove;
            remaining -= amountToRemove;

            // Remove the slot completely if it's empty
            if (slot.amount <= 0)
            {
                inventorySlots[i] = null;
            }
        }

        UpdateInventoryUI();

        return true;
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < inventoryUIContainers.Length; i++)
        {
            if (inventorySlots[i] == null)
            {
                inventoryUIContainers[i].amount.text = "";
                inventoryUIContainers[i].name.text = "";
                inventoryUIContainers[i].image.texture = null;

                continue;
            }

            inventoryUIContainers[i].amount.text =
                inventorySlots[i].amount.ToString();

            inventoryUIContainers[i].name.text =
                inventorySlots[i].item.name;

            TextureAtlasTextureCoordinates textureCoordinates =
                serviceTexturerManager.GetTextureById(
                    inventorySlots[i].item.idTexture
                );

            inventoryUIContainers[i].image.texture =
                textureCoordinates.texture;

            serviceTexturerManager.GetTileUV(
                textureCoordinates.textureSizeX,
                textureCoordinates.textureSizeY,
                textureCoordinates.tileSizeX,
                textureCoordinates.tileSizeY,
                textureCoordinates.texturesCoordinatesX,
                textureCoordinates.texturesCoordinatesY,
                out float uMin,
                out float uMax,
                out float vMin,
                out float vMax
            );

            inventoryUIContainers[i].image.uvRect = new Rect(
                uMin,
                vMin,
                uMax - uMin,
                vMax - vMin
            );
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
