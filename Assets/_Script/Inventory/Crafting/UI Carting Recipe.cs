using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICartingRecipe : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public CraftingRecipe recipe;
    Button Create;
    public void crafting()
    {
        inventoryManager.crafting(recipe);
    }
    private void Awake()
    {
        Create = transform.GetChild(3).gameObject.GetComponent<Button>();
        Create.onClick.AddListener(Craft);
    }
    private void Craft()
    {
        inventoryManager.crafting(recipe);
    }
    void Start()
    {      
        if (recipe != null)
        {
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = recipe.ingredients[0].item.image;
            transform.GetChild(1).gameObject.GetComponent<Image>().sprite = recipe.ingredients[1].item.image;
            transform.GetChild(4).gameObject.GetComponent<Image>().sprite = recipe.resultItem.image;
            if (recipe.ingredients.Count > 2)
            {
                transform.GetChild(2).gameObject.GetComponent<Image>().sprite = recipe.ingredients[2].item.image;
                transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(recipe.ingredients[2].quantity.ToString());
            }
            transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(recipe.ingredients[0].quantity.ToString());
            transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(recipe.ingredients[1].quantity.ToString());
        }
    }

    
}
