using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;


public class UICartingRecipe : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public CraftingRecipe recipe;
    public Button BntCreate,BntGetRecipe;
    public List<Image> Material;
    public Image result;
    public List<TextMeshProUGUI> Quatity;
    public void crafting()
    {
        inventoryManager.crafting(recipe);
    }
    private void Awake()
    {
        BntGetRecipe.onClick.AddListener(Getrecipe);
        BntCreate.onClick.AddListener(Craft);
        GetComponent<Image>().sprite=recipe.resultItem.image;
    }
    private void Getrecipe()
    {
        if (recipe != null)
        {
            result.sprite = recipe.resultItem.image;
            for (int i = 0; i < recipe.ingredients.Count; i++)
            {
                Material[i].sprite = recipe.ingredients[i].item.image;
                Quatity[i].SetText(recipe.ingredients[i].quantity.ToString());
            }
        }
    }
    private void Craft()
    {
        inventoryManager.crafting(recipe);
    }    
}
