using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private List<Collider2D> enemiesInTrigger = new List<Collider2D>();
    float _Damage=15f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInTrigger.Add(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInTrigger.Remove(collision); 
        }
    }
    public void Attack()
    {
        if (enemiesInTrigger.Count>0) 
        {
            Invoke("DelayedDamage",0.5f);
        }
    }
    void DelayedDamage()
    {
        foreach (Collider2D enemy in enemiesInTrigger)
        {
            enemy.GetComponent<action>().takedamage(_Damage);
        }
    }
}
