using Unity.VisualScripting;
using UnityEngine;

public class demo : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;
    public void PickUpitem(int id)
    {
        inventoryManager.addItem(itemsToPickup[id]);
    }
    public void GetSelectedItem()
    {
        Item reveiceItem = inventoryManager.GetSelectItem();
        if (reveiceItem != null)
        {
            Debug.Log("receive item " + reveiceItem);
        }
        else {
            Debug.Log("not receive item");
        }
    }
}
