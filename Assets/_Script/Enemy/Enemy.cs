using UnityEngine;

public class Enemy : MonoBehaviour, action
{
    [SerializeField] float HP = 100f;
    Animator Enemyanimator;
    void Start()
    {
        Enemyanimator = GetComponent<Animator>();
    }
    void Update()
    {
        
    }
    public void Attack()
    {

    }
    public void takedamage(float Damage)
    {
        HP -= Damage;
        Enemyanimator.SetTrigger("Hurt");
        if (HP <= 0) { 
            Enemyanimator.SetTrigger("Death");
            Invoke("Death",0.6f);
        }
    }
    void Death()
    {
        gameObject.SetActive(false);
    }
}
