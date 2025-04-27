using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class RangeOrc : MonoBehaviour
{
    private List<Collider2D> _SolderInTrigger = new List<Collider2D>();
    [SerializeField] float _Damage;
    Enemy enemy;
    private Coroutine attackCoroutine;
    private float attackCooldown = 0f;
    float distance;
    void Start() 
    {
        enemy = GetComponentInParent<Enemy>();
    }

    
    void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }
        if (Solderintrigger())
        {
            enemy.enableWander = false;
            MoveCloserToSolder();
            Solder solder = _SolderInTrigger[0].GetComponent<Solder>();
            if (solder != null && solder.HP <= 0)
            {
                _SolderInTrigger.Remove(_SolderInTrigger[0]);
            }
        }
        else
        {
            enemy.enableWander = true; 
            //StopMoving(); 
        }
    }
    void StopMoving()
    {
        enemy.Enemyanimator.SetBool("isWalking", false);
        enemy.isMoving = false;
    }
    void MoveCloserToSolder()
    {
        Vector3 targetPos = _SolderInTrigger[0].transform.position; 
        distance = Vector3.Distance(transform.position, targetPos);
        if (distance > 1f)
        {
           
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
            enemy.MoveToPos(targetPos);
        }
        else
        {
            enemy.Enemyanimator.SetBool("isWalking", false);
            enemy.isMoving = false;
            if (attackCoroutine == null)
                attackCoroutine = StartCoroutine(AttackEverySecond());
        }
        
    }
    bool Solderintrigger()
    {
        return _SolderInTrigger.Count > 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Solder")|| collision.CompareTag("Player"))
        {

            _SolderInTrigger.Add(collision);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Solder") || other.CompareTag("Player"))
        {
            _SolderInTrigger.Remove(other);
        }
    }
    public IEnumerator AttackEverySecond()
    {      
        while (_SolderInTrigger.Count > 0)
        {
            while (enemy.isMoving && attackCooldown<=0)
            {
                yield return null;
            }

            enemy.flip(_SolderInTrigger[0].transform.position);
            attackCooldown = 1f;
            enemy.Enemyanimator.SetTrigger("attack");
            yield return new WaitForSeconds(0.5f);
            _SolderInTrigger[0].GetComponent<action>().takedamage(_Damage);
            attackCooldown = 0.5f;
            yield return new WaitForSeconds(1f);          
        }
        attackCoroutine=null;
    }
}
