using UnityEngine;
using System.Collections;
using static RangeAttackBoss;
using DG.Tweening;

public class Boss : MonoBehaviour, action
{
    public float HP = 1000;
    public Animator Bossanimator;
    [SerializeField] private float MoveSpeed;
    public bool isMoving;
    private Vector3 spawnPos;
    [SerializeField] float _bulletspeed, _bulletdamege, _lifetime;
    [SerializeField] FireBall fireBall;
    [SerializeField] HandBoss _ArmBoss;
    [SerializeField] Electro _Electro;

    [SerializeField] UIHPenemy _UIHPenemy;
    [SerializeField] Transform Player;
    void Start()
    {
        Bossanimator = GetComponent<Animator>();
        spawnPos = transform.position;
    }
    private void Update()
    {   if (Player == null) return;
        if (Vector3.Distance(transform.position, Player.position) < 12f)
        {
            _UIHPenemy.transform.parent.gameObject.SetActive(true);
        }
        else { _UIHPenemy.transform.parent.gameObject.SetActive(false); }
    }
    public void Attack()
    {
        Bossanimator.SetTrigger("attack");
    }
    public void takedamage(float Damage)
    {
        HP -= Damage;
        _UIHPenemy.SetHealth(HP / 1000);
        if (HP <= 0)
        {
            Bossanimator.Play("death");
            Invoke("Death", 0.6f);
        }
    }
    void Death()
    {
        gameObject.SetActive(false);
    }
    public void MoveToPos(Vector3? newPos)
    {
        if (newPos == null)
        {
            Bossanimator.SetBool("isWalking", false);
            isMoving = false;
            return;
        }
        Vector3 targetPos = newPos.Value;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, MoveSpeed * Time.deltaTime);
        if (transform.position != newPos)
        {
            if (!isMoving)
            {
                Bossanimator.SetBool("isWalking", true);
                isMoving = true;
            }
        }
        else
        {
            Bossanimator.SetBool("isWalking", false);
            isMoving = false;
        }
        flip(targetPos);
    }
    public void flip(Vector3 Pos)
    {
        if (Pos.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (Pos.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

    }
    public void Spawn8Fireballs()
    {   
        Vector2 bossPos = transform.position;
        float initialAngle = Random.Range(0f, 360f);

        for (int i = 0; i < 8; i++)
        {
            float angle = initialAngle + i * 45f; 
            float radians = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
            Vector2 spawnPos = bossPos + direction * 2f;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle+90f);
            FireBall fireball = Instantiate<FireBall>(fireBall, spawnPos, rotation);
            fireball.Init(_bulletspeed, _bulletdamege, _lifetime, direction);
        }
    }
    public void ArmBoss()
    {   
        float bossPosX = (transform.localScale.x == 1) ? transform.position.x + 2 : transform.position.x - 2;
        Quaternion rotation = (transform.localScale.x == 1) ? Quaternion.Euler(0f, 0f, 180f): Quaternion.Euler(0f, 0f, 0f);
        Vector2 bossPos = new Vector2(bossPosX, transform.position.y);
        HandBoss ArmBoss = Instantiate<HandBoss>(_ArmBoss, bossPos, rotation);
        ArmBoss.Init(_bulletspeed, _bulletdamege, _lifetime, transform.right);
    }
    public IEnumerator SpawnRandomLightning(Transform target)
    {
        for (int i = 0; i < 4; i++)
        {
            Electro electro =Instantiate<Electro>(_Electro, transform.position, Quaternion.identity);
            electro.init(target);
            yield return new WaitForSeconds(2f);
        }
    }
}


