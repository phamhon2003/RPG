using UnityEngine;
using UnityEngine.UI;
public class BuyUI : MonoBehaviour
{
    public Button _Button;
    [SerializeField] Item item;
    [SerializeField] int Gold;
    void Start()
    {
        _Button = GetComponent<Button>();
        _Button.onClick.AddListener(Buy);
    }

    public void Buy()
    {
        if (Gamemanager.instance.Gold >= Gold)
        {   
            Gamemanager.instance.Gold -= Gold;
            Gamemanager.instance.SetTextGold();
            InventoryManager.instance.addItem(item, 1);
        };
    }
}
