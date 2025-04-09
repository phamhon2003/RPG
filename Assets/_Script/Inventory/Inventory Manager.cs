using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public int countMaxstack = 99;
    public InventorySlot[] InventorySlots,InventorySlotWhenOpenChest;
    public GameObject inventoryItemprefab;
    public int selectdslot = -1;
    public List<Item> Listitem, ListTest;
    public CraftingRecipe CraftingRecipe;
    public Item ItemSelection;
    [SerializeField] Item itemadd;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        changeselectedSlot(0);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) addItem(itemadd, 10);
        if (Input.GetKeyDown(KeyCode.K)) SaveInventory();
        if (Input.GetKeyDown(KeyCode.L)) LoadInventory();
        if (Input.inputString != null)
        {
            bool inumber = int.TryParse(Input.inputString, out int number);

            if (inumber && number > 0 && number < 8)
            {
                changeselectedSlot(number - 1);
                Debug.Log(number);
            }
        }
        if (selectdslot >= 0 && selectdslot < InventorySlots.Length)
        {
            InventoryItem selectedItem = InventorySlots[selectdslot].GetComponentInChildren<InventoryItem>();
            ItemSelection = selectedItem != null ? selectedItem.itemInstance.item : null;
        }
    }
    void changeselectedSlot(int newvalue)
    {
        if (selectdslot >= 0)
        {
            InventorySlots[selectdslot].deSelect();
        }
        InventorySlots[newvalue].Select();
        selectdslot = newvalue;
    }
    public void addItem(Item item, int count = 1)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlot slot = InventorySlots[i];
            InventoryItem itemItem = slot.GetComponentInChildren<InventoryItem>();

            if (itemItem != null &&
                itemItem.itemInstance.item == item &&
                itemItem.itemInstance.count < countMaxstack &&
                itemItem.itemInstance.IsStackable())
            {
                //addcountitem(item);
                itemItem.itemInstance.count += count;
                itemItem.refreshcount();
                return;
            }
            if (itemItem == null)
            {
                SpawnNewItem(item, slot, count);
                //Listitem.Add(item);
                //addcountitem(item);
                return;
            }
        }
    }

    public void RemoveItem(Item item, int count = 1)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlot slot = InventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null &&
                itemInSlot.itemInstance.item == item)
            {
                if (itemInSlot.itemInstance.count > count)
                {
                    itemInSlot.itemInstance.count -= count;
                    itemInSlot.refreshcount();
                }
                else
                {
                    Destroy(itemInSlot.gameObject);
                }
                return;
            }
        }
    }
    public void crafting(CraftingRecipe recipe)
    {
        if (Cancraft(recipe))
        {
            removeIngredients(recipe);
            addItem(recipe.resultItem);

            foreach (var slot in InventorySlots)
            {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null)
                {
                    itemInSlot.refreshcount();
                }
            }
        }
    }
    bool Cancraft(CraftingRecipe recipe)
    {
        int matchCount = 0;
        foreach (CraftingIngredient material in recipe.ingredients)
        {
            foreach (var slot in InventorySlots)
            {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null &&
                    itemInSlot.itemInstance.item.name == material.item.name &&
                    itemInSlot.itemInstance.count >= material.quantity)
                {
                    matchCount++;
                    break;
                }
            }
        }
        return matchCount == recipe.ingredients.Count;
    }
    void removeIngredients(CraftingRecipe recipe)
    {
        foreach (CraftingIngredient material in recipe.ingredients)
        {
            RemoveItem(material.item, material.quantity);
        }
    }
    void SpawnNewItem(Item item, InventorySlot slot, int count = 1)
    {
        GameObject newitem = Instantiate(inventoryItemprefab, slot.transform);
        InventoryItem inventoryItem = newitem.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item, count);
    }
    public Item GetSelectItem()
    {
        InventorySlot slot = InventorySlots[selectdslot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        return itemInSlot != null ? itemInSlot.itemInstance.item : null;
    }

    public void SaveInventory()
    {
        List<SavedItemData> saveData = new List<SavedItemData>();

        foreach (var slot in InventorySlots)
        {
            InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
            if (item != null)
            {
                saveData.Add(new SavedItemData(item.itemInstance.item.name, item.itemInstance.count));
            }
        }

        string json = JsonUtility.ToJson(new SaveWrapper { items = saveData }, true);
        PlayerPrefs.SetString("InventoryData", json);
        PlayerPrefs.Save();
        Debug.Log("Inventory Saved: " + json);
    }

    public void LoadInventory()
    {
        string json = PlayerPrefs.GetString("InventoryData", "");
        Debug.Log("Trying to load inventory: " + json);
        if (string.IsNullOrEmpty(json)) return;
        Debug.Log("load");
        SaveWrapper wrapper = JsonUtility.FromJson<SaveWrapper>(json);

        foreach (var slot in InventorySlots)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }
        Debug.Log("load");
        foreach (var data in wrapper.items)
        {
            Item item = ListTest.Find(i => i.name == data.itemName);
            if (item != null)
            {
                addItem(item, data.count);
            }
        }
    }

    [System.Serializable]
    class SaveWrapper
    {
        public List<SavedItemData> items;
    }
}
