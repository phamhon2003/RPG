using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeAttackSolder : MonoBehaviour
{
    private List<Collider2D> enemiesInTrigger = new List<Collider2D>();
    [SerializeField] Bullet _bullet;
    [SerializeField] float _bulletspeed = 4, _bulletdamege=5, _lifetime=4;
    Solder solder;
    [SerializeField] float distance;

    private void Start()
    {
        solder= GetComponentInParent<Solder>();
    }
    private Coroutine attackCoroutine;
    private void Update()
    {
        if (enemyintrigger())
        {
            distance = Vector3.Distance(transform.position, enemiesInTrigger[0].transform.position);
            if (distance >= 3.5f && solder.MovingToPos == false)
            { 
                MoveCloserToEnemy();
            }
            
            if (enemiesInTrigger[0].GetComponent<Enemy>().HP<=0)
            {
                enemiesInTrigger.Remove(enemiesInTrigger[0]);
            }
        }       
    }
    void MoveCloserToEnemy()
    {
        Vector3 direction = (enemiesInTrigger[0].transform.position - transform.position).normalized;
        Vector3 newPos = transform.position + direction * (distance - 3.5f);
        solder.StartMoveToPos(newPos);
    }
    bool enemyintrigger()
    {
        if ( enemiesInTrigger.Count > 0)
        {
            return true;
        }
        return false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if (collision.CompareTag("Enemy"))
        {
         
            enemiesInTrigger.Add(collision);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (attackCoroutine == null)
            {

                attackCoroutine = StartCoroutine(FireEverySecond());

            }
            
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemyintrigger())
            {
                if (other == enemiesInTrigger[0])
                {
                    StopCoroutine(attackCoroutine);
                    attackCoroutine = null;
                }
            }
            enemiesInTrigger.Remove(other);
        }        
    }
    public void fire()
    {
        if (enemiesInTrigger.Count != 0)
        {
            Vector2 direction = ((Vector2)enemiesInTrigger[0].transform.position - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Bullet b = Instantiate<Bullet>(_bullet, this.transform.position, rotation);
            b.Init(_bulletspeed, _bulletdamege, _lifetime, direction);
        }
    }
    public IEnumerator FireEverySecond()
    {
        while (enemiesInTrigger.Count > 0) 
        {
            
            while (solder.isMoving || distance > 3.51f)
            {
                yield return null;
            }
            
            if (solder.transform.position.x > enemiesInTrigger[0].transform.position.x)
            {
                solder.transform.localScale = new Vector3(-Mathf.Abs(solder.transform.localScale.x), solder.transform.localScale.y, solder.transform.localScale.z);
            }
            else if (solder.transform.position.x < enemiesInTrigger[0].transform.position.x)
            {
                solder.transform.localScale = new Vector3(Mathf.Abs(solder.transform.localScale.x), solder.transform.localScale.y, solder.transform.localScale.z);
            }
            solder.Solderanimator.SetTrigger("Attack3");
            yield return new WaitForSeconds(0.5f);
            fire();
            yield return new WaitForSeconds(1f);
            
        }      
    }
}
