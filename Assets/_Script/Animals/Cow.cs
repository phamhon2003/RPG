using System.Collections;
using UnityEngine;

public class Cow : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveTime = 2f; 
    public float waitTime = 1f;
    public float sleepTime = 5f;
    public float sleepChance = 0.2f; 

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveDirection;
    public float raycastDistance = 1.5f;
    private bool isMoving = false;
    private bool isSleeping = false;
    private Vector2 previousDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(MoveRandomly());
    }
    
    IEnumerator MoveRandomly()
    {
        while (true)
        {
            if (!isSleeping)
            {
                ChooseRandomDirection();
                isMoving = true;

                float elapsedTime = 0f;
                while (elapsedTime < moveTime && isMoving)
                {
                    if (isMoving && CheckObstacle())
                    {
                        isMoving = false;
                        rb.linearVelocity = Vector2.zero;
                        SetIdleAnimation();
                        break;
                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                if (isMoving)
                {
                    rb.linearVelocity = Vector2.zero;
                    SetIdleAnimation();
                }
                if (Random.value < sleepChance)
                {
                    StartCoroutine(Sleep());
                }
            }          
            yield return new WaitForSeconds(waitTime);
        }
    }

    void ChooseRandomDirection()
    {
        int randomDir = Random.Range(0, 4);
        while ((Vector2)DirectionFromInt(randomDir) == previousDirection)
        {
            randomDir = Random.Range(0, 4);
        }

        moveDirection = DirectionFromInt(randomDir);
        previousDirection = moveDirection;

        switch (randomDir)
        {
        case 0:           
               animator.Play("Walk_Up");
               break;
        case 1:             
                    animator.Play("Walk_Down");
               break;
        case 2:                
               animator.Play("Walk_Left");          
               break;     
        case 3:              
               animator.Play("Walk_Right");
               break;
        }
        rb.linearVelocity = moveDirection * moveSpeed;     
    }
    Vector2 DirectionFromInt(int dir)
    {
        switch (dir)
        {
            case 0: return Vector2.up;
            case 1: return Vector2.down;
            case 2: return Vector2.left;
            case 3: return Vector2.right;
            default: return Vector2.zero;
        }
    }

    bool CheckObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, moveDirection, raycastDistance);
        Debug.DrawRay(rb.position, moveDirection * raycastDistance, Color.red); // Vẽ tia ray mỗi lần kiểm tra
        return hit.collider != null && !hit.collider.isTrigger;
    }
    void SetIdleAnimation()
    {
        if (moveDirection == Vector2.up)
            animator.Play("Idle Up");
        else if (moveDirection == Vector2.down)
            animator.Play("Idle Down");
        else if (moveDirection == Vector2.left)
            animator.Play("Idle Left");
        else if (moveDirection == Vector2.right)
            animator.Play("Idle right");
    }
    IEnumerator Sleep()
    {
        isSleeping = true;
        if (moveDirection == Vector2.left || moveDirection == Vector2.up)
        {
            animator.Play("Sleep_Left");
        }       
        else if (moveDirection == Vector2.right || moveDirection == Vector2.down)
        {
            animator.Play("Sleep Right");
        }

        rb.linearVelocity = Vector2.zero; 

        yield return new WaitForSeconds(sleepTime);      
        SetIdleAnimation();
        yield return new WaitForSeconds(waitTime);
        isSleeping = false;
    }
}
