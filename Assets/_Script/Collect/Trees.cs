using UnityEngine;
using System.Collections;
public class Trees : MonoBehaviour, CanGethit
{
    public int HP = 3;
    SpriteRenderer Sprite;
    public Sprite Tree1;
    public Item wood;
    Animator anim;
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
        
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
            StartCoroutine(HitEffect());
            InventoryManager.instance.addItem(wood);
        }
        if (HP == 1)
        {
            Sprite.sprite = Tree1;
            if(anim != null)
            {
                anim.enabled=false;
            }
        }
    }
    public IEnumerator HitEffect()
    {
       
        Vector3 originalPosition = transform.position;
        transform.position += new Vector3(0.1f, 0, 0);
        yield return new WaitForSeconds(0.1f);
        transform.position = originalPosition;
    }
}
