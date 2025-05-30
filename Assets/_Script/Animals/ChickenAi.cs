using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;

public class ChickenAI : MonoBehaviour
{
    public string CowID;
    public float Grown = 0.5f;
    public enum State { Idle, Eat, Rest, Walk }
    [SerializeField] private State currentState;
    private Animator animator;
    private Rigidbody2D rb;
    private float _eggCooldown = 600f;
    private float _lastEggTime = 0f;
  
    public float walkSpeed = 1.5f;
    public float runSpeed = 3.5f;
    public float stateDuration = 5f;
   // public float detectionRadius = 2f;
    private bool isFacingRight = true;
    public float obstacleCheckDistance = 1.5f ;
    private Nest _Nest;
    PlacedObjectData data;
    [SerializeField] private LayerMask obstacleLayer;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StateMachine());
    }
    private void Update()
    {
        if (Grown <= 1)
        {
            Grown = Grown + 0.002f * Time.deltaTime;
            transform.localScale = Vector3.one * Grown;
        }
        if (currentState == State.Rest) CheckNearNest();
    }
    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case State.Idle:
                    animator.Play("Idle");
                    yield return new WaitForSeconds(stateDuration);
                    //if (Random.Range(0f, 1f) < 0.4f) // 20% cơ hội
                    //{
                    //    ChangeState(State.Eat);
                    //    break;
                    //}
                    if (Random.Range(0f, 1f) < 0.2f) // 20% cơ hội
                    {
                        ChangeState(State.Rest);
                        break;
                    }
                    yield return new WaitForSeconds(Random.Range(1f, 3f)); 
                    ChangeState(State.Walk);                
                    break;

                case State.Eat:
                    animator.Play("Eat");
                    yield return new WaitForSeconds(1f);
                    ChangeState(State.Idle);
                    break;

                case State.Rest:
                    
                    yield return new WaitForSeconds(1f);
                    if (_Nest != null && !_Nest.Ischicken)
                    {
                        _Nest.Ischicken=true;
                        yield return StartCoroutine(MoveToPos(_Nest.transform.position));
                        animator.Play("Rest");
                        yield return new WaitForSeconds(stateDuration);
                        if(Time.time - _lastEggTime >= _eggCooldown && !_Nest.Haveegg&& Grown>=1)
                        {   
                            _Nest.GetEgge();
                            _lastEggTime = Time.time;
                        }
                        _Nest.Ischicken = false;
                    }                  
                    ChangeState(State.Idle);
                    break;
                case State.Walk:
                    animator.Play("Walk");
                    Vector2 targetPosition = GetRandomPosition();
                    yield return StartCoroutine(MoveToPos(targetPosition));
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
    private IEnumerator MoveToPos(Vector2 destination)
    {
        while (Vector2.Distance(transform.position, destination) > 0.1f)
        {
            Vector2 moveDirection = (destination - (Vector2)transform.position).normalized;
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
    }

   
    
    private bool IsObstacleAhead()
    {
        Vector2 moveDirection = rb.linearVelocity.normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, obstacleCheckDistance, obstacleLayer);
        Debug.DrawRay(transform.position, moveDirection * obstacleCheckDistance, Color.red); 
        return hit.collider != null && !hit.collider.isTrigger;
    }
    private void CheckNearNest()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 5f);
        _Nest=null;
        foreach (var hit in hits)
        {
            Nest Nest = hit.GetComponent<Nest>();
            if (Nest != null )
            {
                _Nest = Nest;
                break;
            }
        }
        
    }
    void OnDisable()
    {
        save();
    }
    void save()
    {
        foreach (PlacedObjectData objdata in SaveManager.Instance.placedObjects)
        {
            if (objdata.uniqueID == CowID)
            {
                data = objdata;
            }
        }
        if (data == null)
        {
            data = new PlacedObjectData
            {
                prefabName = gameObject.name,
                position = transform.position,
                uniqueID = System.Guid.NewGuid().ToString(),
                grown = Grown
            };
            SaveManager.Instance.placedObjects.Add(data);

        }
        else
        {
            data.grown = Grown;
            SaveManager.Instance.UpdateSeedData(data);
        }
        SaveManager.Instance.SaveDataInstatiate();
    }
    void OnApplicationQuit()
    {
        save();
    } 
}