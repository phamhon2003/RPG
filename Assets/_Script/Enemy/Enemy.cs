using UnityEngine;

public class Enemy : MonoBehaviour, action
{
    public float HP = 100f;
    public Animator Enemyanimator;
    [SerializeField] private float MoveSpeed;
    public bool isMoving;
    void Start()
    {
        Enemyanimator = GetComponent<Animator>();
    }
    void Update()
    {
        
    }
    public void Attack()
    {
        Enemyanimator.SetTrigger("attack"); 
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
        if (targetPos.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (targetPos.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

}
