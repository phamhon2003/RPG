using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChickenAI : MonoBehaviour
{
    public enum State { Idle, Eat, Rest, Walk }
    [SerializeField] private State currentState;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 targetPosition;

    public float walkSpeed = 1.5f;
    public float runSpeed = 3.5f;
    public float stateDuration = 5f;
    public float detectionRadius = 2f;
    private bool isFacingRight = true;
    public float obstacleCheckDistance = 1.5f ;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case State.Idle:
                    animator.Play("Idle");
                    if (Random.Range(0f, 1f) < 0.4f) // 20% cơ hội
                    {
                        ChangeState(State.Eat);
                        break;
                    }
                    yield return new WaitForSeconds(Random.Range(1f, 3f)); // Thời gian idle ngẫu nhiên
                    ChangeState(State.Walk);                
                    break;

                case State.Eat:
                    animator.Play("Eat");
                    yield return new WaitForSeconds(1f);
                    ChangeState(State.Idle);
                    break;

                case State.Rest:
                    animator.Play("Rest");
                    yield return new WaitForSeconds(stateDuration);
                    ChangeState(State.Idle);
                    break;
                case State.Walk:
                    animator.Play("Walk");
                    targetPosition = GetRandomPosition();
                    while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
                    {                  
                        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;
                        rb.linearVelocity = moveDirection * walkSpeed;
                        if ((moveDirection.x < 0 && !isFacingRight) || (moveDirection.x > 0 && isFacingRight))
                        {
                            Flip();
                        }
                        if (IsObstacleAhead())
                        {
                            break;
                        }
                        yield return null;
                    }
                    rb.linearVelocity = Vector2.zero;
                    ChangeState(State.Idle);
                    break;
            }
            yield return null;
        }
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    private void ChangeState(State newState)
    {
        currentState = newState;
    }

    private Vector2 GetRandomPosition()
    {
        return (Vector2)transform.position + Random.insideUnitCircle * 4f;
    }

   
    private void Update()
    {
       
    }
    
    private bool IsObstacleAhead()
    {
        Vector2 moveDirection = rb.linearVelocity.normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, obstacleCheckDistance);
        Debug.DrawRay(transform.position, moveDirection * obstacleCheckDistance, Color.red); 
        return hit.collider != null && !hit.collider.isTrigger;
    }
}