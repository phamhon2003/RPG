using UnityEngine;

public class Enemy : MonoBehaviour, action
{
    [SerializeField] float HP = 100f;
    [SerializeField] Animator Enemyanimator;
    [SerializeField] private float MoveSpeed;
    bool isMoving;
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
    private void MoveToPos(Vector3 newPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, newPos, MoveSpeed * Time.deltaTime);
        if (transform.position != newPos)
        {
            if (!isMoving)
            {
                Enemyanimator.SetBool("isWalking", true);
                isMoving = true;
            }
        }
        if (newPos.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (newPos.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Solder"))
        {
            if (Vector3.Distance(transform.position, collision.transform.position) > 1.5f && Vector3.Distance(transform.position, collision.transform.position) <= 2.5f)
            {
                MoveToPos(collision.transform.position);
            }
            else if (Vector3.Distance(transform.position, collision.transform.position) <= 1.5f) {
                isMoving = false;
                Enemyanimator.SetBool("isWalking", false);
                //Invoke("Attack", 0.5f);          
            }
            else if (Vector3.Distance(transform.position, collision.transform.position) > 2.5f)
            {
                isMoving = false;
                Enemyanimator.SetBool("isWalking", false);
                //Invoke("Attack", 0.5f);          
            }
        }
    }
}
