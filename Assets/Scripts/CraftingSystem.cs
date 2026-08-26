using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    ServiceCraftingManager serviceCraftingManager;

    ServiceTexturerManager serviceTexturerManager;

    ItemManagerService itemManagerService;

    public Inventory inventory;

    public GameObject ui;

    public GameObject containerUi;

    public GameObject containerUiCraftingAmount;

    public GameObject uiCraftingAmount;

     List<GameObject> uiCraftingAmountList = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serviceCraftingManager = new ServiceCraftingManager();
        itemManagerService = new ItemManagerService();
        serviceTexturerManager = new ServiceTexturerManager();
        CraftingRecipeMetaData[] craftingRecipes = serviceCraftingManager.GetAllRecipe();

        for (int i = 0; i < craftingRecipes.Length; i++)
        {

            float uMin = 0;
            float uMax = 0;
            float vMin = 0;
            float vMax = 0;

            Item item = itemManagerService.GetItemById(craftingRecipes[i].craftedItemId);

            TextureAtlasTextureCoordinates textureCoordinatesIcon = serviceTexturerManager.GetTextureById(item.idTexture);

            GameObject gameObject = Instantiate(ui, containerUi.transform);

            RawImage rawImage = gameObject.GetComponent<RawImage>();

            int recipeIndex = i;

            gameObject.GetComponent<Button>().onClick.AddListener(() => DisplayCraftingRecipe(craftingRecipes[recipeIndex]));

            gameObject.GetComponent<RawImage>().texture = textureCoordinatesIcon.texture;

            serviceTexturerManager.GetTileUV(textureCoordinatesIcon.textureSizeX, textureCoordinatesIcon.textureSizeY, textureCoordinatesIcon.tileSizeX, textureCoordinatesIcon.tileSizeY, textureCoordinatesIcon.texturesCoordinatesX, textureCoordinatesIcon.texturesCoordinatesY, out uMin, out uMax, out vMin, out vMax);

            rawImage.uvRect = new Rect(
                uMin,
                vMin,
                uMax - uMin,
                vMax - vMin
            );

        }

        DisplayCraftingRecipe(craftingRecipes[0]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisplayCraftingRecipe(CraftingRecipeMetaData craftingRecipe)
    {
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
}
