using UnityEngine;
using UnityEngine.UI;

public class TradeUi : MonoBehaviour
{
    public Button _Button; 
    [SerializeField] Item item;
    [SerializeField] int Gold;
    void Start()
    {
        _Button = GetComponent<Button>();
        _Button.onClick.AddListener(Trade);
    }

    public void Trade()
    {       
        if (Checkitem())
        {
            InventoryManager.instance.RemoveItem(item,1);
            Gamemanager.instance.Gold += Gold;
            Gamemanager.instance.SetTextGold();      
        };
    }
    bool Checkitem()
    {
        for (int i = 0; i < InventoryManager.instance.InventorySlots.Length; i++)
        {
            InventorySlot slot = InventoryManager.instance.InventorySlots[i];
            InventoryItem itemItem = slot.GetComponentInChildren<InventoryItem>();
            if (itemItem != null&& itemItem.itemInstance.item == item && itemItem.itemInstance.count>0) return true;
        }
        return false;
    }
}
