using TMPro;
using UnityEngine;

public class GetWater : MonoBehaviour
{
    [SerializeField] bool _GetWater=false;
    Vector3 offSet = new Vector3(0, 2, 0);
    Transform _Player;
    [SerializeField] TextMeshProUGUI _Collect;
    // Update is called once per frame
    private void Start()
    {
       
        
    }
    void Update()
    {
        if(InventoryManager.instance.ItemSelection != null &&  _GetWater)
        {
            PlayerController player= _Player.GetComponent<PlayerController>();
            if (InventoryManager.instance.ItemSelection.Name == "Watering tools")
            {
                _Collect.gameObject.SetActive(true);
                _Collect.transform.position = Camera.main.WorldToScreenPoint(_Player.transform.position + offSet);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Gamemanager.instance._WateringTool.TotalWate = 100;
                }
            }
            else if (InventoryManager.instance.ItemSelection.Name == "fishing rod" && player._Canfishing && !player._Isfishing)
            {
                _Collect.gameObject.SetActive(true);
                _Collect.transform.position = Camera.main.WorldToScreenPoint(_Player.transform.position + offSet);
            }else
                _Collect.gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _GetWater = true;        
            _Player =collision.transform;
        }
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _GetWater = false;
            _Collect.gameObject.SetActive(false);
            _Player = null;
        }
    }
}
