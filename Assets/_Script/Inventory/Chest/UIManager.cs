using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class UIManager : MonoBehaviour
{
    [Header("PlayerUI")]
    [SerializeField] PlayerController Player;
    [SerializeField] Image HP, Food;

    public static UIManager Instance { get; private set; }
    public bool IsOpenIventoryItem;
    public Button ShowInventory;

    [Header("Chest UI")]
    public GameObject chestPanel,InventoryPanel;
    //public Transform chestSlotsContainer;
    public Vector3 buttonOffset = new Vector3(0, 1.5f, 0);
    [System.NonSerialized] public Chest currentOpenChest; 
    public Button exitChestButton;
    public TextMeshProUGUI TextopenChest,_E;

    [Header("Collect UI")]
    public GameObject TextCollectUI;

    [Header("Craft UI")]
    public CraftingRecipe RecipeSelected;
    public Button CreateButton;

    [Header("Fishing UI")]
    public GameObject FishingUI;
    Fishing _fishing;

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
        _fishing= GetComponent<Fishing>(); 
        _fishing.enabled = false;
        exitChestButton.onClick.AddListener(ExitChest);
        CreateButton.onClick.AddListener(Craft);
    }
    private void Update()
    {
        if (InventoryPanel.activeSelf)
        {
            IsOpenIventoryItem = true;
        }
        else {
            IsOpenIventoryItem = false;
        }
        HP.fillAmount = Player._HP / 100;
        Food.fillAmount = Player._Food / 100;
    }
    
 
    public void ShowOpenButton(Chest chest, Vector3 Pos)
    {
        if (TextopenChest != null)
        {
            currentOpenChest = chest;
            TextopenChest.gameObject.SetActive(true);
            TextopenChest.transform.position = Camera.main.WorldToScreenPoint(Pos + buttonOffset);
        }
    }
    public void ExitChest()
    {
        IsOpenIventoryItem = false;
        if(currentOpenChest != null)
            currentOpenChest.isopenchest = false;
        UIManager.Instance.HideChestUI();
    }
    public void HideOpenButton()
    {
        if (TextopenChest != null)
        {
            TextopenChest.gameObject.SetActive(false);
        }
    }

    public void UpdateButtonPosition(Vector3 Pos)
    {
        if (TextopenChest != null)
        {
            
        }
    }
    public void ShowChestUI()
    {
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
        if (TextopenChest != null)
        {
            TextCollectUI.SetActive(true);
            TextCollectUI.transform.position = Camera.main.WorldToScreenPoint(Pos + buttonOffset);
        }
    }
    private void Craft()
    {
        if (RecipeSelected != null)
            InventoryManager.instance.crafting(RecipeSelected);
    }
    public void StartFishing()
    {
        FishingUI.SetActive(true);
        _fishing.enabled = true;
    }
    public void StopFishing()
    {
        FishingUI.SetActive(false);
        _fishing.enabled = false;
        Player._Isfishing = false;
        Player._Isreeling = false;
        Player.MyAnimator.SetTrigger("Caught");
        Player.MyAnimator.SetBool("Isreeling", false);
    }
    public void ShowTextE(Vector3 Pos)
    {
        if (_E != null)
        {
            _E.gameObject.SetActive(true);
            _E.transform.position = Camera.main.WorldToScreenPoint(Pos + buttonOffset);
        }
    }
    public void HideTextE()
    {
        
        if (_E != null)
            _E.gameObject.SetActive(false);
    }
}
