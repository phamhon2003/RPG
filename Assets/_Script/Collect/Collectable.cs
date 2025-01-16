using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Item item;
    private void OnTriggerEnter2D(Collider2D collision)
    {
         
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            Destroy(gameObject);
            InventoryManager.instance.addItem(item);
        }
    }
}
