using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.STP;

public class Stone : MonoBehaviour, CanGethit
{   
    public int HP=3;
    SpriteRenderer Sprite;
    public Sprite Stone2,Stone1;
    public Item stone;
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP == 2)
        {
            Sprite.sprite = Stone2;
        }
        if (HP == 1)
        {
            Sprite.sprite = Stone1;
        }
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
    }   
}
