using UnityEngine;

public class Potato : MonoBehaviour, CanGethit
{
    float Hp = 0;
    SpriteRenderer Sprite;
    public Sprite potato,potato1,potato2,potato3;
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {   
 
        if (Hp <= 18)
        {
            Hp = Hp + 0.2f * Time.deltaTime;
        }
        if (Hp > 3 && Hp <= 6)
        {
            Sprite.sprite = potato;
        }
        if (Hp > 6 && Hp <= 9)
        {
            Sprite.sprite = potato1;
        }
        if (Hp > 9 && Hp <= 12)
        {
            Sprite.sprite = potato2;
        }
        if (Hp > 12 && Hp <= 15)
        {
            Sprite.sprite = potato3;
        }
        
    }
    public void Gethit()
    {
        Gamemanager.instance.TimapsManager.movepos(transform.position);
        Destroy(gameObject);
    }
}
