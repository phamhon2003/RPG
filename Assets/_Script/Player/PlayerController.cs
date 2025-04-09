using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour, action
{  
    [SerializeField] Tilemap interactabbleMap;
    [SerializeField] float MoveSpeed,Damage;
    float SpeedX, SpeedY;
    private Vector2 Movement;
    Rigidbody2D rb;
    Animator MyAnimator;
    bool facingRight = true;
    bool iswalks;
    bool canmove = true,cooldowntimecandig=true;
    bool stonedetection, treedetection;
    RaycastHit2D hit;
    public LayerMask targetLayer; // L?p mà Raycast có th? ch?m vào
    public GameObject khoai,weapon;
    public Transform aim;
    public WeaponController weaponController;
    private Coroutine moveCoroutine;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
    }
   
    
    private void Update()
    {
        if (Input.inputString!=null && InventoryManager.instance.ItemSelection!=null) {
            bool inumber = int.TryParse(Input.inputString, out int number);

            if (inumber && InventoryManager.instance.ItemSelection.Name == "Bow")
            {
                weapon.SetActive(true);
            }
            if (inumber && InventoryManager.instance.ItemSelection.Name != "Bow")
            {
                weapon.SetActive(false);
            }
        }
        ShootRay();
        InteractFarm();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (InventoryManager.instance.ItemSelection != null)
            {
                switch (InventoryManager.instance.ItemSelection.Name)
                {
                    case "digging tools":
                        if (!iswalks)
                        {
                            if (stonedetection && cooldowntimecandig)
                            {
                                if (hit.collider != null)
                                {
                                    Mining();
                                    hit.collider.GetComponent<CanGethit>().Gethit();
                                }
                            }
                        }
                        break;
                    case "Ax":
                        if (!iswalks)
                        {
                            Debug.Log(treedetection);                         
                            if (treedetection && cooldowntimecandig)
                            {
                                if (hit.collider != null)
                                {
                                    Axe();
                                    hit.collider.GetComponent<CanGethit>().Gethit();
                                }
                            }
                        }
                        break;
                    case "Bow":
                        weaponController.Fire();
                        break;
                    case "Sword":
                        Attack();
                        GetComponentInChildren<Sword>().Attack();
                        break;
                    default:
                        //Debug.Log("stone");
                        //Debug.Log("stone");
                        break;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.localScale.x > 0 ? Vector2.right * 2f : Vector2.left * 2f, Color.red);
        Move();
        
    }

    void interacmap(Vector3Int Pos)
    {
            
            //Vector3Int tilePosition = interactabbleMap.WorldToCell(positon);
            if (TimapsManager.instance.IsInteractable(Pos))
            {            
                TimapsManager.instance.settileinterac(Pos);
                Dig();
            }
        
    }
    

    void Move()
    {
        if (canmove == true)
        {
            SpeedX = Input.GetAxisRaw("Horizontal");
            SpeedY = Input.GetAxisRaw("Vertical");
            MyAnimator.SetFloat("SpeedX", SpeedX);
            MyAnimator.SetFloat("SpeedY", SpeedY);
            Movement = new Vector2(SpeedX, SpeedY).normalized * MoveSpeed;
            rb.linearVelocity = Movement;
            if (SpeedX > 0 && !facingRight)
            {
                flip();
            }
            if (SpeedX < 0 && facingRight)
            {
                flip();
            }
        }
        if (SpeedX == 0 && SpeedY ==0)
        {
            iswalks = false;
        }else if (SpeedX !=0 || SpeedY !=0)
        {
            iswalks = true;
        }
        if (Movement != Vector2.zero && moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null; 
        }

    }
    
    void Mining()
    {   
        MyAnimator.SetTrigger("mining");
        canmove = false;
        cooldowntimecandig = false;
        Invoke("timecandig", 0.6f);
        
    }
    void InteractFarm()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3Int Pos = TimapsManager.instance.getpostile(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (InventoryManager.instance.ItemSelection != null && TimapsManager.instance.HightlightTile)
            {
                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine);
                }               
                moveCoroutine = StartCoroutine(MoveToClickPosition(TimapsManager.instance.GetCenterTile(), () =>
                {
                    HandleInteraction(Pos);
                }));
            }
        }
    }
    void HandleInteraction(Vector3Int Pos)
    {
        switch (InventoryManager.instance.ItemSelection.Name)
        {
            case "Shovel":
                if (SpeedX == 0 && SpeedY == 0)
                {
                    interacmap(Pos);
                    GetComponentInChildren<Harvest>().CanHarvest();
                }
                break;
            case "Khoai":
                //if (Gamemanager.instance.TimapsManager.cantrongkhoai(getpostile()) && Gamemanager.instance.TimapsManager.checkpos(Gamemanager.instance.TimapsManager.getpos(getpostile())))

                    Instantiate(khoai,TimapsManager.instance.GetCenterTile(), Quaternion.identity);
                    TimapsManager.instance.addpos(TimapsManager.instance.GetCenterTile());
                    
                break;
            default:
                break;
        }
    }
    IEnumerator MoveToClickPosition(Vector2 targetPosition, System.Action onComplete)
    {
        while (Vector2.Distance(rb.position, targetPosition) > 0.1f)
        {
            if (rb.position.x < targetPosition.x && !facingRight)
            {
                flip();
            }
            if (rb.position.x > targetPosition.x && facingRight)
            {
                flip();
            }
            Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, MoveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
            yield return new WaitForFixedUpdate(); 
        }      
        moveCoroutine = null;
        onComplete?.Invoke();
    }
    void Dig()
    {
        MyAnimator.SetTrigger("Dig");
    }
    void Axe()
    {
        MyAnimator.SetTrigger("Axe");
        canmove = false;
        cooldowntimecandig = false;
        Invoke("timecandig", 0.6f);

    }
    
    void timecandig()
    {
        canmove = true;
        cooldowntimecandig = true;
    }
    void flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        facingRight = !facingRight;
    }
    void ShootRay()
    {
        
         hit = Physics2D.Raycast(transform.position, transform.localScale.x > 0 ? Vector2.right : Vector2.left , 2f , targetLayer); ;

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Stone"))
            {
                
                stonedetection = true;
            }else if (!hit.collider.CompareTag("Stone"))
            {
                stonedetection = false;
            }
            if (hit.collider.CompareTag("Tree"))
            {
                treedetection = true;
            }
            else if (!hit.collider.CompareTag("Tree"))
            {
                treedetection = false;
            }
        }
        else
        {
           // Debug.Log("Không trúng");
        }
      
    }
    public void Attack()
    {
        MyAnimator.SetTrigger("attack");
    }
    public void takedamage(float Damage)
    {
        
    }
}
