using UnityEngine;

public class Trees : MonoBehaviour, CanGethit
{
    public int HP = 3;
    SpriteRenderer Sprite;
    public Sprite Tree1;
    public Item wood;
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP == 1)
        {
            Sprite.sprite = Tree1;
        }
        
    }
    public void Gethit()
    {
        Invoke("Takedame", 0.6f);
    }
    public void Takedame()
    {
        if (HP >= 0)
        {
            HP -= 1;
            InventoryManager.instance.addItem(wood);
        }
    }
}
