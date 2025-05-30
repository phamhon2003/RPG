using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangeOrc : RangeAttackBase
{
    
    Orc _Orc; 
    void Start() 
    {
        _Damage = 5;
        _Orc = GetComponentInParent<Orc>();
    }
    void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }
        if (target!= null)
        {
            _Orc.enableWander = false;
            MoveCloserToSolder();
        }
        else
        {
            _Orc.enableWander = true; 
            //StopMoving(); 
        }
    }
    void MoveCloserToSolder()
    {
        Vector3 targetPos = target.position; 
        distance = Vector3.Distance(transform.position, targetPos);
        if (distance > 1f)
        {
           
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
            _Orc.MoveToPos(targetPos);
        }
        else
        {
            _Orc.Enemyanimator.SetBool("isWalking", false);
            _Orc.isMoving = false;
            if (attackCoroutine == null)
                attackCoroutine = StartCoroutine(AttackEverySecond());
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            target = collision.transform;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }
    protected override IEnumerator AttackEverySecond()
    {      
        while (target!=null)
        {
            while (_Orc.isMoving && attackCooldown<=0)
            {
                yield return null;
            }

            _Orc.flip(target.position);
            attackCooldown = 1f;
            _Orc.Enemyanimator.SetTrigger("attack");
            yield return new WaitForSeconds(0.5f);
            target.GetComponent<action>().takedamage(_Damage);
            attackCooldown = 0.5f;
            yield return new WaitForSeconds(1f);          
        }
        attackCoroutine=null;
    }
}
