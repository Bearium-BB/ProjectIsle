using System.IO;
using UnityEngine;

public class ServiceCraftingManager
{
    CraftingRecipeMetaData[] craftingRecipes;

    public ServiceCraftingManager()
    {
        string pathTextureData = Path.Combine(
            Application.streamingAssetsPath,
            "craftingRecipe.json"
        );
        string jsonData = File.ReadAllText(pathTextureData);

        CraftingRecipeMetaDataArray data = JsonUtility.FromJson<CraftingRecipeMetaDataArray>(jsonData);

        craftingRecipes = data.recipes;
    }

    public CraftingRecipeMetaData[] GetAllRecipe()
    {
        return craftingRecipes;
    }

}
