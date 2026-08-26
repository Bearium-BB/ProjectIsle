using System;
using UnityEngine;

[Serializable]
public class CraftingRecipeMetaData
{

    public CraftingRecipeItemAmount[] recipe;
    public int id;
    public int craftedItemId;
}

[Serializable]
public class CraftingRecipeItemAmount
{
    public int itemId;
    public int amount;
}

[Serializable]
public class CraftingRecipeMetaDataArray
{
    public CraftingRecipeMetaData[] recipes;
}
