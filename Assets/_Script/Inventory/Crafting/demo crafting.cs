using UnityEngine;

public class democrafting : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public CraftingRecipe recipe;
    public void crafting()
    {
        inventoryManager.crafting(recipe);
    }

}
