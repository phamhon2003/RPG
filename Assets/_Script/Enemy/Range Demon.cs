using System.Collections;
using UnityEngine;

public class RagenDemon : RangeAttackBase
{
    Demon _Demon;
    void Start()
    {
        _Damage = 7;
        _Demon = GetComponentInParent<Demon>();
    }
    void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }
        if (target!=null)
        {
            _Demon.enableWander = false;
            MoveCloserToSolder();
        }
        else
        {
            _Demon.enableWander = true;
            //StopMoving(); 
        }
    }
    void MoveCloserToSolder()
    {
        Vector3 targetPos = target.position;
        distance = Vector3.Distance(transform.position, targetPos);
        if (distance > 3f)
        {

            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
            _Demon.MoveToPos(targetPos);
        }
        else
        {     
            _Demon.isMoving = false;
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
            while (_Demon.isMoving && attackCooldown <= 0)
            {
                yield return null;
            }
            Transform Target = target;
            _Demon.flip(target.position);
            attackCooldown = 1f;
            _Demon.Enemyanimator.SetTrigger("attack");
            yield return new WaitForSeconds(0.5f);
            _Demon.Fire(Target);
            attackCooldown = 0.5f;
            yield return new WaitForSeconds(1f);
        }
        attackCoroutine = null;
    }
}
