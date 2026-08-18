using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{

    List<CraftingRecipeMetaData> recipes = new List<CraftingRecipeMetaData>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        recipes.Add(new CraftingRecipeMetaData(
        new CraftingRecipeItemAmount[]
        {
            new CraftingRecipeItemAmount(1, 5),
            new CraftingRecipeItemAmount(2, 2)
        }
        ));

        recipes.Add(new CraftingRecipeMetaData(
            new CraftingRecipeItemAmount[]
            {
                new CraftingRecipeItemAmount(1, 3),
                new CraftingRecipeItemAmount(3, 4)
            }
        ));

        recipes.Add(new CraftingRecipeMetaData(
            new CraftingRecipeItemAmount[]
            {
                new CraftingRecipeItemAmount(1, 10),
                new CraftingRecipeItemAmount(4, 5),
                new CraftingRecipeItemAmount(5, 2)
            }
        ));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
