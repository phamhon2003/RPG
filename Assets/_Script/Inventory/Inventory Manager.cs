using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public int countMaxstack=99;
    public InventorySlot[] InventorySlots;
    public GameObject inventoryItemprefab;
    public int selectdslot = -1;
    public List<Item> Listitem;
    public CraftingRecipe CraftingRecipe;
    public Item ItemSelection;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        int count = 0; 
        foreach (Item item in Listitem)
        {
            if (item.count > 0)
            {
                InventorySlot slot = InventorySlots[count];
                InventoryItem itemItem = slot.GetComponentInChildren<InventoryItem>();
                SpawnNewItem(item,slot);
                count++;
            }
        }   
        changeselectedSlot(0);
    }
    private void Update()
    {
        if(Input.inputString != null)
        {
            bool inumber = int.TryParse(Input.inputString, out int number);
   
            if (inumber && number > 0 && number < 8) {
                changeselectedSlot(number-1);
                Debug.Log(number);
            }
        }
        if (InventorySlots[selectdslot].GetComponentInChildren<InventoryItem>() != null)
        {
            ItemSelection = InventorySlots[selectdslot].GetComponentInChildren<InventoryItem>().item;
        }
        else
        {
            ItemSelection = null;
        }    
    }
    void changeselectedSlot(int newvalue)
    {   if (selectdslot >= 0)
        {
            InventorySlots[selectdslot].deSelect();
        }
        InventorySlots[newvalue].Select();
        selectdslot = newvalue;
    }
    public void addItem(Item item)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlot slot = InventorySlots[i];
            InventoryItem itemItem = slot.GetComponentInChildren<InventoryItem>();

            if (itemItem != null &&
                itemItem.item==item &&
                itemItem.Count<countMaxstack&&
                itemItem.item.stackable==true){
                //itemItem.Count++;
                addcountitem(item);
                itemItem.refreshcount();                
                return;
            }
            if (itemItem == null && item.count<=0) {
                SpawnNewItem(item, slot);
                //Listitem.Add(item);
                addcountitem(item);
                return;
            }
        }
    }
    public void crafting(CraftingRecipe recipe)
    {
        if (Cancraft(recipe))
        {
            addItem(recipe.resultItem);
            removeIngredients(recipe);
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                InventorySlot slot = InventorySlots[i];
                InventoryItem itemItem = slot.GetComponentInChildren<InventoryItem>();
                if (itemItem != null)
                {
                    itemItem.refreshcount();
                }
            }
        }
       
    }
    bool Cancraft(CraftingRecipe recipe)
    {
        int count = 0;
        foreach (CraftingIngredient material in recipe.ingredients)
        {
            foreach (Item item in Listitem)
            {
                if (material.item.name == item.name && material.quantity <= item.count)
                {
                    count++;                  
                }
            }
        }
        if (count == recipe.ingredients.Count)
        {   
            return true;
            
        }
        return false;
    }
    void removeIngredients(CraftingRecipe recipe)
    {
        foreach (CraftingIngredient material in recipe.ingredients)
        {
            foreach (Item item in Listitem)
            {
                if (material.item.name == item.name && material.quantity < item.count)
                {                  
                    item.count -= material.quantity;
                }
            }
        }
    }
    void addcountitem(Item addcountitem)
    {
        foreach (Item item2 in Listitem)
        {
            if (addcountitem.name == item2.name)
            {
                item2.count++;
            }
        }
    }
    void SpawnNewItem(Item item,InventorySlot slot)
    {
        GameObject newitem = Instantiate(inventoryItemprefab,slot.transform);
        InventoryItem inventoryItem= newitem.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
    public Item GetSelectItem()
    {
        InventorySlot slot = InventorySlots[selectdslot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null) { 
            return itemInSlot.item;
        }
        return null;
    }
}
