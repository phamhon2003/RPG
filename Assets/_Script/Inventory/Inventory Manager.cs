using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public int countMaxstack=5;
    public InventorySlot[] InventorySlots;
    public GameObject inventoryItemprefab;
    public int selectdslot = -1;
    public List<Item> Listitem;

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
        if(Input.inputString != null)
        {
            bool inumber = int.TryParse(Input.inputString, out int number);
   
            if (inumber && number > 0 && number < 8) {
                changeselectedSlot(number-1);
                Debug.Log(number);
            }
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
        foreach (Item item2 in Listitem) {
            if(item.name == item2.name)
            {
                item2.count++;
            }
        }

        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlot slot = InventorySlots[i];
            InventoryItem itemItem = slot.GetComponentInChildren<InventoryItem>();
            if (itemItem != null &&
                itemItem.item==item &&
                itemItem.Count<countMaxstack&&
                itemItem.item.stackable==true){
                itemItem.Count++;
                itemItem.refreshcount();
                return;
            }
        }
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlot slot = InventorySlots[i];
            InventoryItem itemItem = slot.GetComponentInChildren<InventoryItem>();
            if (itemItem == null) {
                SpawnNewItem(item,slot);
                return;
            }
        }
        return ;
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
