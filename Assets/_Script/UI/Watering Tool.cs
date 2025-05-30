using UnityEngine;
using UnityEngine.UI;

public class WateringTool : MonoBehaviour
{
    public float TotalWate=0;
    public GameObject UItotalwater;
    Image UIWater;
    private void Start()
    {
        UIWater = UItotalwater.GetComponent<Image>();
    }
    void Update()
    {   
        if(UItotalwater != null) UItotalWater();
        if(UIWater != null) UIWater.fillAmount = TotalWate / 100f;
    }
    void UItotalWater() {
        for (int i = 0; i < 7; i++)
        {
            InventorySlot slot = InventoryManager.instance.InventorySlots[i];
            InventoryItem itemItem = slot.GetComponentInChildren<InventoryItem>();
            if (itemItem != null && itemItem.itemInstance.item.name == "Watering tools") {
                UItotalwater.SetActive(true);
                UItotalwater.transform.position = new Vector3(slot.transform.position.x,UItotalwater.transform.position.y,0); 
                return;
            }
        }
        UItotalwater.SetActive(false);
    }
}
