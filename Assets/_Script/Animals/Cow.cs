
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cow : MonoBehaviour
{
    public string CowID;
    public float Grown =0.5f;
    public enum CowGender { Male, Female }
    public CowGender gender;
    public float moveSpeed = 2f;
    public float moveTime = 2f; 
    public float waitTime = 1f;
    public float sleepTime = 5f;
    public float sleepChance = 0.2f;
    [SerializeField] private float hunger = 100f;
    [SerializeField] private float hungerDecreaseRate = 1f;
    private Vector2 previousDirection;
    private Vector2 moveDirection;
    public float raycastDistance = 1.5f;
    
    [SerializeField] protected float breedCooldown = 30f; // thời gian giữa các lần sinh
    protected float lastBreedTime = -999f;

    [Header("States")]
    private bool isMoving = false;
    private bool isSleeping = false;
    [SerializeField] public bool isReadyToBreed = false;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected Coroutine moveAndEatCoroutine, Sleepcoroutine, MoveRandom;
    [SerializeField] protected GameObject _babyCowPrefab;
    protected Grass targetGrass;

    PlacedObjectData data;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        MoveRandom=StartCoroutine(MoveRandomly());
    }
    protected virtual void Update()
    {   if(hunger>=0) hunger -= hungerDecreaseRate * Time.deltaTime;
        if (Grown <= 1&& hunger > 10)
        {
            Grown = Grown + 0.001f * Time.deltaTime;
            transform.localScale = Vector3.one * Grown;
        }
        if (hunger < 10f )
        {
            FindClosestGrass();
        }
        if (targetGrass != null)
        {
            if (moveAndEatCoroutine == null)
                moveAndEatCoroutine = StartCoroutine(MoveAndEatRoutine());
        }
    }
    protected IEnumerator MoveRandomly()
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
                    if (Sleepcoroutine != null)
                        StopCoroutine(Sleepcoroutine);
                    Sleepcoroutine=  StartCoroutine(Sleep());
                }
            }          
            yield return new WaitForSeconds(waitTime);
        }
    }

    protected void ChooseRandomDirection()
    {
        int randomDir = Random.Range(0, 4);
        while (DirectionFromInt(randomDir) == previousDirection)
        {
            randomDir = Random.Range(0, 4);
        }

        moveDirection = DirectionFromInt(randomDir);
        previousDirection = moveDirection;
        SetAnimWalk(randomDir);
        rb.linearVelocity = moveDirection * moveSpeed;     
    }
    protected void SetAnimWalk(int dir) {
        switch (dir)
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

    protected bool CheckObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, moveDirection, raycastDistance);
        Debug.DrawRay(rb.position, moveDirection * raycastDistance, Color.red); // Vẽ tia ray mỗi lần kiểm tra
        return hit.collider != null && !hit.collider.isTrigger;
    }
    protected void SetIdleAnimation()
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
        Sleepcoroutine=null;
    }
    protected void FindClosestGrass()
    {   

        Collider2D[] allGrass = Physics2D.OverlapCircleAll(transform.position, 5f);
        targetGrass = null;  
        foreach (Collider2D grassCollider in allGrass)
        {
            Grass grass = grassCollider.GetComponent<Grass>();
            if (grass != null && grass._IsPlayerPlaced)
            {      
                targetGrass = grass;
                break;
            }
        }
      
    }

    protected IEnumerator MoveAndEatRoutine()
    {
        if (targetGrass == null) yield break;
        if (!targetGrass.isActiveAndEnabled) yield break;
        if (MoveRandom != null)
        {
            StopCoroutine(MoveRandom);
            MoveRandom = null;
        }
        Vector3 targetPos = targetGrass.transform.position;
        List<Node> path = Pathfinding.FindPath((Vector2)transform.position,(Vector2) targetPos);
        foreach (Node node in path)
        {
            Vector2 MoveToPos = GridSystem.Instance.GridToWorld(node.gridPos);
            if (targetGrass == null)
            {
                rb.linearVelocity = Vector2.zero;
                moveAndEatCoroutine = null;
                SetIdleAnimation();
                if (MoveRandom == null)
                    MoveRandom = StartCoroutine(MoveRandomly());
                yield break;
            }
            yield return StartCoroutine(MoveToPosition(MoveToPos));
        }
        rb.linearVelocity = Vector2.zero;
        SetIdleAnimation();
        yield return new WaitForSeconds(2f);

        if (targetGrass != null)
        {
            targetGrass.Consume();
            hunger += 100;
        }

        moveAndEatCoroutine=null;
        if (MoveRandom == null)
            MoveRandom = StartCoroutine(MoveRandomly());
    }
    protected IEnumerator MoveToPosition(Vector2 targetPos)
    { 
        while (Mathf.Abs(transform.position.x - targetPos.x) > 0.05f)
        {           
            float directionX = targetPos.x - transform.position.x;
            moveDirection = directionX > 0 ? Vector2.right : Vector2.left;

            rb.linearVelocity = moveDirection * moveSpeed;
            SetAnimWalk(directionX > 0 ? 3 : 2);

            yield return null;
        }
        while (Mathf.Abs(transform.position.y - targetPos.y) > 0.05f)
        {          
            float directionY = targetPos.y - transform.position.y;
            moveDirection = directionY > 0 ? Vector2.up : Vector2.down;

            rb.linearVelocity = moveDirection * moveSpeed;
            SetAnimWalk(directionY > 0 ? 0 : 1);

            yield return null;
        }
        
    }
    void OnDisable()
    {
        save();
    }
    void OnApplicationQuit()
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
}

