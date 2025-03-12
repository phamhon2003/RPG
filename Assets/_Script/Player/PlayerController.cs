using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour, action
{  
    [SerializeField] Tilemap interactabbleMap;
    [SerializeField] float MoveSpeed;
    float SpeedX, SpeedY;
    private Vector2 Movement,positionray;
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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
    }
   
    
    private void Update()
    {
        if (Input.inputString!=null) {
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
                    case "Shovel":
                        if (SpeedX == 0 && SpeedY == 0)
                        {
                            interacmap();
                            GetComponentInChildren<Harvest>().CanHarvest();
                        }
                        break;
                    case "Khoai":

                        if (Gamemanager.instance.TimapsManager.cantrongkhoai(getpostile()) && Gamemanager.instance.TimapsManager.checkpos(Gamemanager.instance.TimapsManager.getpos(getpostile())))
                        {
                            Instantiate(khoai, Gamemanager.instance.TimapsManager.getpos(getpostile()), Quaternion.identity);
                            Gamemanager.instance.TimapsManager.addpos(Gamemanager.instance.TimapsManager.getpos(getpostile()));
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
        positionray = new Vector2(transform.position.x,transform.position.y+0.8f);
        Debug.DrawRay(positionray, transform.localScale.x > 0 ? Vector2.right * 2f : Vector2.left * 2f, Color.red);
        move();
        
    }

    void interacmap()
    {
            
            //Vector3Int tilePosition = interactabbleMap.WorldToCell(positon);
            if (Gamemanager.instance.TimapsManager.IsInteractable(getpostile()))
            {            
                Gamemanager.instance.TimapsManager.settileinterac(getpostile());
                Dig();
            }
        
    }
    Vector3Int getpostile()
    {
        Vector3Int positon = new Vector3Int((int)(transform.position.x-1f ), (int)(transform.position.y ), 0);
        return positon;
    }

    void move()
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
        
    }
    
    void Mining()
    {   
        MyAnimator.SetTrigger("mining");
        canmove = false;
        cooldowntimecandig = false;
        Invoke("timecandig", 0.6f);
        
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
        
         hit = Physics2D.Raycast(positionray, transform.localScale.x > 0 ? Vector2.right : Vector2.left , 2f , targetLayer); ;

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
