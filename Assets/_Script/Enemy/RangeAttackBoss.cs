using System.Collections;
using UnityEngine;

public class RangeAttackBoss : RangeAttackBase
{
    private bool isAttacking = false;
    Boss _Boss;
    public enum AttackType { Melee, Lazer, Electro, Shoot ,FireBall}
    [SerializeField] GameObject _laser;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        _Boss = GetComponentInParent<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking )//&& TargetInTrigger[0] != null)
        {
            //StartCoroutine(AttackEverySecond());
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(ExecuteAttack(AttackType.Shoot));
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(ExecuteAttack(AttackType.FireBall));
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(ExecuteAttack(AttackType.Electro));
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(ExecuteAttack(AttackType.Lazer));
        }
    }
    AttackType GetRandomSkill()
    {
        return (AttackType)Random.Range(1, 5); 
    }
    protected override IEnumerator AttackEverySecond()
    {
        isAttacking = true;
        Vector3 targetPos = target.position;
        distance = Vector2.Distance(transform.position, targetPos);

        AttackType chosenAttack;

        if (distance <= 2f)
        {
            int roll = Random.Range(0, 2);
            chosenAttack = (roll == 0) ? AttackType.Melee : GetRandomSkill();
        }
        else
        {
            chosenAttack = GetRandomSkill();
        }

        yield return StartCoroutine(ExecuteAttack(chosenAttack));
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
    IEnumerator ExecuteAttack(AttackType type)
    {
        switch (type)
        {
            case AttackType.Melee:
                _Boss.Bossanimator.Play("melee");
                break;
            case AttackType.Lazer:
                _laser.SetActive(true);
                _laser.GetComponent<Laser>().rotate(target);
                _Boss.Bossanimator.Play("laser_cast");
                yield return new WaitForSeconds(1.567f);
                break;
            case AttackType.Electro:
                _Boss.Bossanimator.Play("glow");
                StartCoroutine(_Boss.SpawnRandomLightning(target));
                yield return new WaitForSeconds(8f);
                break;
            case AttackType.Shoot:
                _Boss.Bossanimator.Play("shoot");
                yield return new WaitForSeconds(1f);
                _Boss.ArmBoss();
                //_Boss.Bossanimator.Play("idle");
                break;
            case AttackType.FireBall:
                _Boss.Bossanimator.Play("immune");
                yield return new WaitForSeconds(1f);
                _Boss.Spawn8Fireballs();
                
                break;
        }
        _Boss.Bossanimator.Play("idle");
        yield return null;
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
}
