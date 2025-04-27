using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Threading;

public class UICrafting : MonoBehaviour
{
    public CraftingRecipe recipe;
    public Button BntGetRecipe;
    public List<Image> Material;
    public Image result;
    public List<TextMeshProUGUI> Quatity;
   
    private void Awake()
    {
        BntGetRecipe.onClick.AddListener(Getrecipe);     
        if (recipe != null )
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = recipe.resultItem.image;
    }
    private void Getrecipe()
    {
        if (recipe != null)
        {
            result.sprite = recipe.resultItem.image;
            for (int i = 0; i < recipe.ingredients.Count; i++)
            {
                Material[i].gameObject.transform.parent.gameObject.SetActive(true);
                Material[i].sprite = recipe.ingredients[i].item.image;
                Quatity[i].SetText(recipe.ingredients[i].quantity.ToString());
            }
            for (int i = recipe.ingredients.Count; i < 4; i++)
            {
                Material[i].gameObject.transform.parent.gameObject.SetActive(false);
            }
            UIManager.Instance.RecipeSelected=recipe;
        }
    }
    
}
