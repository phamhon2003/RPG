using UnityEngine;


public class Orc : MonoBehaviour, action
{
    public float HP = 100f;
    public Animator Enemyanimator;
    [SerializeField] private float MoveSpeed;
    public bool isMoving;

    [Header("Wander Action")]
    [SerializeField] public bool enableWander = true;
    [SerializeField] private float wanderMoveDuration = 2f;
    [SerializeField] private float wanderWaitDuration = 3f;

    private float wanderTimer = 0f;
    private bool isWandering = false;
    private Vector3 wanderTarget;
    private Vector3 spawnPos;

    [SerializeField] UIHPenemy _UIHPenemy;
  
    void Start()
    {
        _UIHPenemy = GetComponentInChildren<UIHPenemy>();
        EnenyManager.Instance.activeEnemies.Add(this.transform);
        Enemyanimator = GetComponent<Animator>();
        spawnPos = transform.position;
    }
    private void FixedUpdate()
    {
        SeparateFromOtherEnemies();
        if (enableWander)
        {
            WanderBehavior();
        }
    }
    public void Attack()
    {
        Enemyanimator.SetTrigger("attack"); 
    }
    public void takedamage(float Damage)
    {
        HP -= Damage;
        _UIHPenemy.SetHealth(HP/100);
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
    public void MoveToPos(Vector3? newPos)
    {
        if (newPos == null)
        {
            Enemyanimator.SetBool("isWalking", false);
            isMoving = false;
            return;
        }
        Vector3 targetPos = newPos.Value;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, MoveSpeed * Time.deltaTime);
        if (transform.position != newPos)
        {
            if (!isMoving)
            {
                Enemyanimator.SetBool("isWalking", true);
                isMoving = true;
            }
        }
        else
        {
            Enemyanimator.SetBool("isWalking", false);
            isMoving = false;
        }
        flip(targetPos);
    }
    public void flip(Vector3 Pos)
    {
        if (Pos.x < transform.position.x)
        {
            _UIHPenemy.fillImage.fillOrigin = 1;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y,transform.localScale.z);
        }
        else if (Pos.x > transform.position.x)
        {
            _UIHPenemy.fillImage.fillOrigin = 0;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

    }
    void SeparateFromOtherEnemies()
    {
        float separationRadius = 0.5f;
        Vector2 pushForce = Vector2.zero;
        int nearbyCount = 0;

        foreach (Transform other in EnenyManager.Instance.activeEnemies)
        {
            if (other == this) continue;

            float dist = Vector2.Distance(transform.position, other.position);
            if (dist < separationRadius)
            {
                Vector2 away = (Vector2)(transform.position - other.position);
                if (away != Vector2.zero)
                {
                    pushForce += away.normalized / dist;
                    nearbyCount++;
                }
            }
        }

        if (nearbyCount > 0)
        {
            pushForce /= nearbyCount;
            transform.position += (Vector3)(pushForce * Time.deltaTime * 1.5f); // tốc độ đẩy nhẹ
        }
    }
    void WanderBehavior()
    {
        if (!isWandering)
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0f)
            {
                isWandering = true;
                wanderTimer = wanderMoveDuration;

                // Chọn vị trí ngẫu nhiên quanh vùng spawn
                Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(1f, 2f);
                wanderTarget = spawnPos + new Vector3(offset.x, offset.y, 0f);
            }
            else
            {   
                MoveToPos(null); // đứng yên
            }
        }
        else
        {
            MoveToPos(wanderTarget);
            wanderTimer -= Time.deltaTime;

            // Nếu gần đến hoặc hết thời gian thì dừng lại
            if (Vector3.Distance(transform.position, wanderTarget) < 0.2f || wanderTimer <= 0f)
            {
                isWandering = false;
                wanderTimer = wanderWaitDuration;
            }
        }
    }
}
