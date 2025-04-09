using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Item item;
    private bool isPlayerInRange = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isPlayerInRange)
        {
            Debug.Log("F");
            UIManager.Instance.TextCollectUI.SetActive(false);
            InventoryManager.instance.addItem(item);
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.DirectionCollectUI(transform.position);
            isPlayerInRange = true;
           
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {   
            isPlayerInRange = false;
            UIManager.Instance.TextCollectUI.SetActive(false);
        }
    }
}
