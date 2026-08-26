using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    ServiceCraftingManager serviceCraftingManager;

    ServiceTexturerManager serviceTexturerManager;

    ItemManagerService itemManagerService;

    int selectedCraftedItemId = -1;

    public Inventory inventory;

    public GameObject ui;

    public GameObject containerUi;

    public GameObject containerUiCraftingAmount;

    public GameObject uiCraftingAmount;

    List<GameObject> uiCraftingAmountList = new List<GameObject>();

    CraftingRecipeMetaData[] craftingRecipes;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serviceCraftingManager = new ServiceCraftingManager();
        itemManagerService = new ItemManagerService();
        serviceTexturerManager = new ServiceTexturerManager();

        craftingRecipes = serviceCraftingManager.GetAllRecipe();

        PopulateCraftingRecipeUI();

        DisplayCraftingRecipe(craftingRecipes[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisplayCraftingRecipe(CraftingRecipeMetaData craftingRecipe)
    {
        selectedCraftedItemId = craftingRecipe.id;

        for (int i = 0; i < uiCraftingAmountList.Count; i++)
        {
            Destroy(uiCraftingAmountList[i]);
        }

        uiCraftingAmountList.Clear();

        Debug.Log(craftingRecipe.recipe.Length);

        foreach (CraftingRecipeItemAmount recipeItemAmount in craftingRecipe.recipe)
        {

            Item requiredItem = itemManagerService.GetItemById(recipeItemAmount.itemId);

            TextureAtlasTextureCoordinates itemTextureCoordinates =
                serviceTexturerManager.GetTextureById(requiredItem.idTexture);

            GameObject requirementIconObject =
                Instantiate(uiCraftingAmount, containerUiCraftingAmount.transform);

            uiCraftingAmountList.Add(requirementIconObject);

            RawImage requirementIcon =
                requirementIconObject.GetComponent<RawImage>();

            requirementIconObject.GetComponentInChildren<TMP_Text>().text = recipeItemAmount.amount.ToString();

            requirementIcon.texture = itemTextureCoordinates.texture;

            serviceTexturerManager.GetTileUV(
                itemTextureCoordinates.textureSizeX,
                itemTextureCoordinates.textureSizeY,
                itemTextureCoordinates.tileSizeX,
                itemTextureCoordinates.tileSizeY,
                itemTextureCoordinates.texturesCoordinatesX,
                itemTextureCoordinates.texturesCoordinatesY,
                out float uMin,
                out float uMax,
                out float vMin,
                out float vMax
            );

            requirementIcon.uvRect = new Rect(
                uMin,
                vMin,
                uMax - uMin,
                vMax - vMin
            );


        }
        
    }

    private void PopulateCraftingRecipeUI()
    {
        for (int i = 0; i < craftingRecipes.Length; i++)
        {
            float uMin;
            float uMax;
            float vMin;
            float vMax;

            // Get the item that this recipe creates
            Item craftedItem = itemManagerService.GetItemById(
                craftingRecipes[i].craftedItemId
            );

            // Get the texture information for the crafted item
            TextureAtlasTextureCoordinates textureCoordinates =
                serviceTexturerManager.GetTextureById(craftedItem.idTexture);

            // Create the crafting recipe UI element
            GameObject recipeUI = Instantiate(ui, containerUi.transform);

            // Get the UI components
            Button recipeButton = recipeUI.GetComponent<Button>();
            RawImage recipeIcon = recipeUI.GetComponent<RawImage>();

            // Store the index so the button references the correct recipe
            int recipeIndex = i;

            // Display the crafting recipe when clicked
            recipeButton.onClick.AddListener(() =>
                DisplayCraftingRecipe(craftingRecipes[recipeIndex])
            );

            // Set the item texture
            recipeIcon.texture = textureCoordinates.texture;

            // Calculate the UV coordinates for the item in the texture atlas
            serviceTexturerManager.GetTileUV(
                textureCoordinates.textureSizeX,
                textureCoordinates.textureSizeY,
                textureCoordinates.tileSizeX,
                textureCoordinates.tileSizeY,
                textureCoordinates.texturesCoordinatesX,
                textureCoordinates.texturesCoordinatesY,
                out uMin,
                out uMax,
                out vMin,
                out vMax
            );

            // Display only the item's section of the texture atlas
            recipeIcon.uvRect = new Rect(
                uMin,
                vMin,
                uMax - uMin,
                vMax - vMin
            );
        }
    }

    public void CraftItem()
    {
        if (selectedCraftedItemId == -1)
        {
            Debug.Log("No crafting recipe selected.");
            return;
        }

        CraftingRecipeMetaData selectedRecipe =
            craftingRecipes.FirstOrDefault(
                recipe => recipe.id == selectedCraftedItemId
            );

        if (selectedRecipe == null)
        {
            Debug.LogError("Could not find selected crafting recipe.");
            return;
        }

        // Check that we have all required resources
        foreach (CraftingRecipeItemAmount recipeItem in selectedRecipe.recipe)
        {
            if (!inventory.HasItem(
                recipeItem.itemId,
                recipeItem.amount))
            {
                Item requiredItem =
                    itemManagerService.GetItemById(recipeItem.itemId);

                Debug.Log(
                    $"Not enough {requiredItem.name} to craft."
                );

                return;
            }
        }

        // Remove the required resources
        foreach (CraftingRecipeItemAmount recipeItem in selectedRecipe.recipe)
        {
            inventory.RemoveItem(
                recipeItem.itemId,
                recipeItem.amount
            );
        }

        // Get the item that the recipe creates
        Item craftedItem =
            itemManagerService.GetItemById(
                selectedRecipe.craftedItemId
            );

        // Add the crafted item to the inventory
        inventory.AddInventory(craftedItem, selectedRecipe.amount);

        Debug.Log($"Crafted {craftedItem.name}!");
    }

}
