using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class CraftingIngredient
{
    public Item item;  
    public int quantity;  
}

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<CraftingIngredient> ingredients;  
    public Item resultItem;  
    public int resultQuantity;  
}
