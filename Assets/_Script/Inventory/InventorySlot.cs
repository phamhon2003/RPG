
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventorySlot : MonoBehaviour,IDropHandler
{
    public Image image;
    public Color SelectedColor,NotSelectedColor;
    private void Awake()
    {
        deSelect();

    }
    public void Select()
    {
        image.color = SelectedColor;
    }
    public void deSelect()
    {
        image.color = NotSelectedColor;
    }
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (transform.childCount == 0)
        {        
            inventoryItem.parentAfterDrag = transform;
            return;
        }
        if(transform.childCount !=0)
        {
            InventoryItem inventoryItemOnDrop = transform.GetChild(0).GetComponent<InventoryItem>();
            if (inventoryItem.itemInstance.item == inventoryItemOnDrop.itemInstance.item)
            {
                Debug.Log("childcount");
                inventoryItemOnDrop.itemInstance.count += inventoryItem.itemInstance.count;
                inventoryItemOnDrop.refreshcount();
                Destroy(inventoryItem.gameObject);
            }
        }
    }
}
