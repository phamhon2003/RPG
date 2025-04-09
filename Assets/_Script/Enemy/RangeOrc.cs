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
            MoveCloserToSolder();
            Solder solder = _SolderInTrigger[0].GetComponent<Solder>();
            if (solder != null && solder.HP <= 0)
            {
                _SolderInTrigger.Remove(_SolderInTrigger[0]);
            }
        }
        else
        {
            StopMoving(); 
        }
    }
    void StopMoving()
    {
        enemy.Enemyanimator.SetBool("isWalking", false);
        enemy.isMoving = false;
    }
    void MoveCloserToSolder()
    {
        Vector3 direction, newPos;
        Vector3 targetPos = _SolderInTrigger[0].transform.position; 
        distance = Vector3.Distance(transform.position, targetPos);
        direction = (targetPos - transform.position).normalized;
        newPos = transform.position + direction * (distance - 1.5f);
        enemy.MoveToPos(newPos);
        
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Solder") || collision.CompareTag("Player"))
        {
            if (attackCoroutine == null )
            {

                attackCoroutine = StartCoroutine(AttackEverySecond());

            }

        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Solder") || other.CompareTag("Player"))
        {
            if (Solderintrigger())
            {
                if (other == _SolderInTrigger[0])
                {
                    StopCoroutine(attackCoroutine);
                    attackCoroutine = null;
                }
            }
            _SolderInTrigger.Remove(other);
        }
    }
    public IEnumerator AttackEverySecond()
    {      
        while (_SolderInTrigger.Count > 0)
        {

            while (enemy.isMoving)
            {
                yield return null;
            }

            if (enemy.transform.position.x > _SolderInTrigger[0].transform.position.x)
            {
                enemy.transform.localScale = new Vector3(-Mathf.Abs(enemy.transform.localScale.x), enemy.transform.localScale.y, enemy.transform.localScale.z);
            }
            else if (enemy.transform.position.x < _SolderInTrigger[0].transform.position.x)
            {
                enemy.transform.localScale = new Vector3(Mathf.Abs(enemy.transform.localScale.x), enemy.transform.localScale.y, enemy.transform.localScale.z);
            }     
            enemy.Enemyanimator.SetTrigger("attack");
            yield return new WaitForSeconds(0.5f);
            _SolderInTrigger[0].GetComponent<action>().takedamage(_Damage);
            attackCooldown = 0.5f;
            yield return new WaitForSeconds(1f);          
        }
        attackCoroutine=null;
    }
}
