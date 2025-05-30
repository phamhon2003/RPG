
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour, action
{
    [Header("PlayerStats")]
    public float _HP = 100f, _Food = 100f;
    [SerializeField] float MoveSpeed,Damage;
    
    
    Rigidbody2D rb;
    public Animator MyAnimator;
    private Vector2 Movement;
    float SpeedX, SpeedY,_CooldownTimeAction=0 ;
    bool facingRight = true;
    [SerializeField] bool iswalks;
    bool canmove = true,cooldowntimecandig=true;
    private float stepDistance = 0.4f; 
    private float lastStepTime = 0f;

    [Header("RayCast")]
    public bool stonedetection, treedetection, _Canfishing; 
    public bool _Isfishing, _Isreeling;
    RaycastHit2D hit;
    public LayerMask targetLayer; 
    public GameObject _Harvest;

    public WeaponController weaponController;
    private Coroutine moveCoroutine;


    void Start()
    {
        if(LoadSceneStatic.PosPlayer != Vector3.zero) transform.position = LoadSceneStatic.PosPlayer;
        rb = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
        
    }
    private void Update()
    {
        HandleHunger();
        if (_CooldownTimeAction > 0) _CooldownTimeAction -= Time.deltaTime;
        if (_CooldownTimeAction <= 0 && !_Isfishing)
        {
            cooldowntimecandig=true ;
            canmove = true;
        }
            if (UIManager.Instance.IsOpenIventoryItem) return;
        if (Input.inputString!=null && InventoryManager.instance.ItemSelection!=null) {
            bool inumber = int.TryParse(Input.inputString, out int number);

            if (inumber && InventoryManager.instance.ItemSelection.Name == "Bow")
            {
                weaponController.gameObject.SetActive(true);
            }
            if (inumber && InventoryManager.instance.ItemSelection.Name != "Bow")
            {
                weaponController.gameObject.SetActive(false);
            }
        }
        ShootRay();
        if(TimapsManager.instance!=null) InteractFarm();
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
                        break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && !_Isfishing )
        {
            if (InventoryManager.instance.ItemSelection != null)
            {
                switch (InventoryManager.instance.ItemSelection.Name)
                {
                    case "fishing rod":
                        fishing();
                        break;                
                    default:
                        break;
                }
            }
                
        }
        if (Input.GetMouseButtonDown(1) && !_Isfishing)
        {
            if (InventoryManager.instance.ItemSelection != null)
            {
                switch (InventoryManager.instance.ItemSelection.Name)
                {
                    case "Cooked fish":
                        _Food += 20f;
                        _Food = Mathf.Min(_HP, 100f);
                        break;
                    case "Cooked beef":
                        _Food += 50f;
                        _Food = Mathf.Min(_HP, 100f);
                        break;
                    case "Cooked chicken":
                        _Food += 50f;
                        _Food = Mathf.Min(_HP, 100f);
                        break;
                    case "Bread":
                        _Food += 50f;
                        _Food = Mathf.Min(_HP, 100f);
                        break;
                    default:
                        break;
                }
            }

        }
    }
    private void FixedUpdate()
    {  
        if (UIManager.Instance.IsOpenIventoryItem) return;
        Debug.DrawRay(transform.position, transform.localScale.x > 0 ? Vector2.right * 2f : Vector2.left * 2f, Color.red);
        Move();
        
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
            if (SpeedX > 0 && !facingRight) flip();
            if (SpeedX < 0 && facingRight)  flip();        
        }
        if (SpeedX == 0 && SpeedY == 0)    iswalks = false;                  
        else if (SpeedX !=0 || SpeedY !=0) iswalks = true;               
        if (iswalks && Time.time - lastStepTime > stepDistance)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance._FoodStep);
            lastStepTime = Time.time;
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
    {   if (_Harvest != null && _Harvest.activeSelf) return; 
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() ) {
            if (TimapsManager.instance.interactabbleMap == null) return;
            Vector3Int Pos = TimapsManager.instance.getpostile(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector3 PosSeed = TimapsManager.instance.GetCenterTile(Pos);
            if (InventoryManager.instance.ItemSelection != null && TimapsManager.instance.HightlightTile && TimapsManager.instance.InteractFarm)
            {
                if (InventoryManager.instance.ItemSelection.Name == "Shovel" && TimapsManager.instance._CooldownDig > 0) return; 
                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine); 
                }               
                moveCoroutine = StartCoroutine(MoveToClickPosition(PosSeed, () =>
                {
                    TimapsManager.instance.HandleInteraction(Pos, PosSeed);
                }));
            }
        }
    }
    
    public IEnumerator MoveToClickPosition(Vector2 targetPosition, System.Action onComplete)
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
    public void Dig()
    {
        MyAnimator.SetTrigger("Dig");
        canmove = false;
        _CooldownTimeAction = 0.7f;
        AudioManager.Instance.PlaySFX(AudioManager.Instance._Dig);
    }
    public void Watering()
    {
        MyAnimator.SetTrigger("Watering");
        canmove = false;
        _CooldownTimeAction = 0.7f;
        //AudioManager.Instance.PlaySFX(AudioManager.Instance._Dig);
    }
    void Axe()
    {
        MyAnimator.SetTrigger("Axe");       
        canmove = false;
        cooldowntimecandig = false;
        _CooldownTimeAction = 0.7f;
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
        stonedetection = false;
        treedetection = false;
        _Canfishing = false;

        if (hit.collider != null)
        {
            string tag = hit.collider.tag;

            if (tag == "Stone")
            {
                stonedetection = true;
            }
            else if (tag == "Tree")
            {
                treedetection = true;
            }
            else if (tag == "Fishing")
            {
                _Canfishing = true;
            }
        }

    }
    public void Attack()
    {
        MyAnimator.SetTrigger("attack");
    }
    public void takedamage(float Damage)
    {
        _HP -= Damage;
    }
    void fishing()
    {
        if (_Canfishing)
        {
            canmove = false;
            _Isfishing = true;
            StartCoroutine(Casting());
            StartCoroutine(FishBiteRoutine());
        }
    }
    IEnumerator Casting()
    {   
        MyAnimator.SetTrigger("Casting");
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySFX(AudioManager.Instance._Splash);
    }
    IEnumerator FishBiteRoutine()
    {
        float waitTime = Random.Range(10, 20);
        yield return new WaitForSeconds(waitTime);
        _Isreeling = true;
        MyAnimator.SetBool("Isreeling", _Isreeling);
        AudioManager.Instance.PlayLoopingSFX(AudioManager.Instance._PullFish);
        UIManager.Instance.StartFishing();
        
    }
    void HandleHunger()
    {
        if (_Food > 0)
        {
            _Food -= 0.2f * Time.deltaTime;
            _Food = Mathf.Max(_Food, 0f); 
        }

        if (_Food <= 0 && _HP > 0)
        {
            _HP -= 0.5f * Time.deltaTime;
            _HP = Mathf.Max(_HP, 0f);
        }
        if (_Food > 90f && _HP < 100f)
        {
            _HP += 0.2f * Time.deltaTime;
            _HP = Mathf.Min(_HP, 100f); 
        }
    }
}
