using UnityEngine;
using static UnityEditor.Progress;

public class CraftingRecipeMetaData
{

    CraftingRecipeItemAmount[] recipe;
    int craftedItemId;

    public CraftingRecipeMetaData(CraftingRecipeItemAmount[] recipe)
    {
        this.recipe = recipe;
    }
}

public class CraftingRecipeItemAmount
{
    public int id;
    public int amount;

    public CraftingRecipeItemAmount(int id, int amount)
    {
        this.id = id;
        this.amount = amount;
    }
}
