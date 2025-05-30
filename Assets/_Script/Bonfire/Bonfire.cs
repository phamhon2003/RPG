using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] Sprite _HaveWood, _OutOfWood,_CookedFood;
    Transform _Fỉre;
    [SerializeField] float _Wood;
    SpriteRenderer _Meat;
    [SerializeField] bool _IsPlayer,_Iscooking=false;
    float _CookingTime=10f;
    float _CurrentCookTime;
    public List<Item> _ListItemCookedFood;
    private Item _ItemCookedFood;
    void Start()
    {
        _Fỉre = transform.GetChild(0);
        _Meat = transform.GetChild(1).GetComponent<SpriteRenderer>();
        _Wood = 100f;
    }

   
    void Update()
    {
        if (_IsPlayer&& _Meat.sprite == null && !_Iscooking&& InventoryManager.instance.ItemSelection != null) 
        {   if(InventoryManager.instance.ItemSelection.IsFood==true) UIManager.Instance.ShowTextE(transform.position);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_IsPlayer &&  !_Iscooking)
            {
                if (_Meat.sprite == null)
                {
                    if (InventoryManager.instance.ItemSelection != null)
                    {
                        switch (InventoryManager.instance.ItemSelection.Name)
                        {
                            case "Beef":
                                StartCooking(InventoryManager.instance.ItemSelection);
                                break;
                            case "Chicken":
                                StartCooking(InventoryManager.instance.ItemSelection);
                                break;
                            case "Fish":
                                StartCooking(InventoryManager.instance.ItemSelection);
                                break;
                            case "Wood":
                                _Wood += 50f;
                                InventoryManager.instance.RemoveItem(InventoryManager.instance.ItemSelection, 1);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    if (_ItemCookedFood != null)
                    {
                        InventoryManager.instance.addItem(_ItemCookedFood, 1);
                        _Meat.sprite = null;
                    }
                }
            } 
        }
        if(_Iscooking && _Meat.sprite != null&&_Wood>0&& _Iscooking)
        {
            _CookingTime -= Time.deltaTime;
            _Wood -= Time.deltaTime;
            if (_CookingTime <= 0) 
            {
                StopCooking();
                _Meat.sprite = _CookedFood;            
            }
        }
    }
    private void StartCooking(Item item)
    {   
        _Meat.sprite = item.image;
        GetCookedFood(item.name);
        _Fỉre.gameObject.SetActive(true);
        _Iscooking = true;
        _CookingTime = 10f;
        InventoryManager.instance.RemoveItem(item,1);
    }
    private void StopCooking()
    {
        _Fỉre.gameObject.SetActive(false);
        _Iscooking=false;
    }
    private void GetCookedFood(string Namefood)
    {
        foreach(Item item in _ListItemCookedFood)
        {
            if ("Cooked" + Namefood==item.Name)
            {
                _ItemCookedFood = item;
                _CookedFood =item.image;
                break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {  
            _IsPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _IsPlayer = false;
            UIManager.Instance.HideTextE();
        }
    }
}
