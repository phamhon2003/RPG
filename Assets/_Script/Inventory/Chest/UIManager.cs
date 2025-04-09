using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Chest UI")]
    public GameObject chestPanel,InventoryPanel;
    public Transform chestSlotsContainer;
    public Vector3 buttonOffset = new Vector3(0, 1.5f, 0);
    [System.NonSerialized] public Chest currentOpenChest; 
    public Button openChestButton, exitChestButton;
    
    [Header("Collect UI")]
    public GameObject TextCollectUI;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
        openChestButton.onClick.AddListener(() => currentOpenChest?.OpenChest());
        exitChestButton.onClick.AddListener(() => currentOpenChest?.ExitChest());
    }
    public void ShowOpenButton(Chest chest)
    {
        if (openChestButton != null)
        {
            currentOpenChest = chest;
            openChestButton.gameObject.SetActive(true);
        }
    }

    public void HideOpenButton()
    {
        if (openChestButton != null)
        {
            openChestButton.gameObject.SetActive(false);
        }
    }

    public void UpdateButtonPosition(Vector3 Pos)
    {
        if (openChestButton != null)
        {
            openChestButton.transform.position = Camera.main.WorldToScreenPoint(Pos + buttonOffset);
        }
    }
    public void ShowChestUI()
    {
        if (chestPanel == null)
        {
            Debug.LogError("Chest Panel chưa được gán trong Inspector!");
            return;
        }
        chestPanel.SetActive(true);
        InventoryPanel.SetActive(true);      
    }

    public void HideChestUI()
    {
        if (chestPanel != null)
        {
            chestPanel.SetActive(false);
            InventoryPanel.SetActive(false);
        }
        currentOpenChest = null;
    }
    public void DirectionCollectUI(Vector3 Pos)
    {
        if (openChestButton != null)
        {
            TextCollectUI.SetActive(true);
            TextCollectUI.transform.position = Camera.main.WorldToScreenPoint(Pos + buttonOffset);
        }
    }
}
