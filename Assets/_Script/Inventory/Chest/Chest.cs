using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InventoryManager;

public class Chest : MonoBehaviour
{
    [Header("Chest Settings")]
    public float interactionDistance = 2f;
    public InventorySlot[] chestSlots;
    public Transform playerTransform;
    public bool  PlayerDetectedchest=false,isopenchest=false;
    public GameObject inventoryItemPrefab;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
           
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) SaveChest();
        
        if(PlayerDetectedchest)
            if (!isopenchest)
            {

                if (Input.GetKeyDown(KeyCode.F)) OpenChest();
                UIManager.Instance.ShowOpenButton(this, transform.position);
            }
            else if (isopenchest)
            {
                UIManager.Instance.HideOpenButton();
            }
           
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerDetectedchest = true;            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        UIManager.Instance.HideOpenButton();
        PlayerDetectedchest = false;
    }

    public void OpenChest()
    {
        if (isopenchest) return;
        isopenchest = true;
        Debug.Log("mo ruong:");
        UIManager.Instance.ShowChestUI(); 
        LoadChest();
    }
    //public void ExitChest()
    //{   
    //    isopenchest=false;
    //    UIManager.Instance.HideChestUI();
    //}
    public bool AddItem(Item item, int count = 1)
    {
        for (int i = 0; i < chestSlots.Length; i++)
        {
            InventorySlot slot = chestSlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null &&
                itemInSlot.itemInstance.item == item &&
                itemInSlot.itemInstance.count < 99 &&
                itemInSlot.itemInstance.IsStackable())
            {
                itemInSlot.itemInstance.count += count;
                itemInSlot.refreshcount();
                return true;
            }

            if (itemInSlot == null)
            {
                GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
                InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
                inventoryItem.InitialiseItem(item, count); // Đánh dấu vật phẩm từ chest
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(Item item, int count = 1)
    {
        for (int i = 0; i < chestSlots.Length; i++)
        {
            InventorySlot slot = chestSlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.itemInstance.item == item)
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
    public void SaveChest()
    {   if (!isopenchest) return;
        List<SavedItemData> saveData = new List<SavedItemData>();

        // Duyệt qua các slot
        foreach (var slot in chestSlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                saveData.Add(new SavedItemData(itemInSlot.itemInstance.item.name, itemInSlot.itemInstance.count));
            }
        }

        // Bọc dữ liệu và chuyển thành JSON
        ChestSaveWrapper wrapper = new ChestSaveWrapper { items = saveData };
        string json = JsonUtility.ToJson(wrapper, true);

        // Tạo key theo tên rương
        string saveKey = "ChestData_" + gameObject.name;
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();

        Debug.Log($"[SAVE] Đã lưu dữ liệu rương '{gameObject.name}':\n{json}");
    }

    public void LoadChest()
    {
        StartCoroutine(DelayedLoadChest());
        
    }
    private IEnumerator DelayedLoadChest()
    {
        string saveKey = "ChestData_" + gameObject.name;
        string json = PlayerPrefs.GetString(saveKey, "");

        if (string.IsNullOrEmpty(json))
        {
            Debug.Log($"[LOAD] Không có dữ liệu để tải cho rương '{gameObject.name}'.");
            yield break;
        }

        ChestSaveWrapper wrapper = JsonUtility.FromJson<ChestSaveWrapper>(json);

        // Xóa item cũ trước khi load mới
        foreach (var slot in chestSlots)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject); 
            }
        }

        // Chờ 1 frame để đảm bảo object đã bị huỷ hoàn toàn
        yield return null;

        // Load item vào rương
        foreach (var data in wrapper.items)
        {
            Item item = InventoryManager.instance.ListTest.Find(i => i.name == data.itemName);
            if (item != null)
            {
                AddItem(item, data.count);
            }
            else
            {
                Debug.LogWarning($"[LOAD] Không tìm thấy item: {data.itemName} trong InventoryManager.");
            }
        }

        Debug.Log($"[LOAD] Đã tải lại dữ liệu rương '{gameObject.name}':\n{json}");
    }
    [System.Serializable]
    public class ChestSaveWrapper
    {
        public List<SavedItemData> items;
    }
}


