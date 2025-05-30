using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public Text countText;
    [HideInInspector] public int Count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    public ItemInstance itemInstance;
    
    
    public void InitialiseItem(Item item, int count = 1)
    {
        itemInstance = new ItemInstance(item, count);
        image.sprite = itemInstance.GetSprite();
        refreshcount();
    }

    public void refreshcount()
    {
        if (itemInstance != null)
        {
            if (itemInstance.count > 1)
            {
                countText.text = itemInstance.count.ToString();
                countText.enabled = true;
            }
            else
            {
                countText.enabled = false;
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        
    }
    
}

