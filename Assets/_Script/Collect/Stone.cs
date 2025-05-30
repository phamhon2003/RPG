using Unity.VisualScripting;
using UnityEngine;

public class Stone : MonoBehaviour, CanGethit
{   
    public int HP=3;
    public Item stone;
    void Start()
    {
        //Sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Gethit()
    {
        Invoke("Takedame",0.6f);
    }
    public void Takedame()
    {
        if(HP >= 0)
        {
            HP -= 1;
            InventoryManager.instance.addItem(stone);
        }
        if (HP ==0 )
        {
            gameObject.SetActive(false);
        }
    }   
}
