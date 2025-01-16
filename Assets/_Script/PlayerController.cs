using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour,action
{
    [SerializeField] Tilemap interactabbleMap;
    [SerializeField] float MoveSpeed;
    float SpeedX,SpeedY;
    private Vector2 Movement;
    Rigidbody2D rb;
    SpriteRenderer MySpriteRenderer;
    Animator MyAnimator;
    bool facingRight=true;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        MyAnimator = GetComponent<Animator>();
       
    }
    public void attack()
    {

    }
    private void Update()
    {

        interacmap();
    }
    private void FixedUpdate()
    {
       
        move();
        
    }
    
    void interacmap()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
 
            Vector3Int positon = new Vector3Int((int)(transform.position.x-0.7),(int)(transform.position.y+2.4f),0);
            Vector3Int tilePosition = interactabbleMap.WorldToCell(positon);
            Debug.Log(positon);
            Debug.Log(Gamemanager.instance.TimapsManager.IsInteractable(positon));
            if (Gamemanager.instance.TimapsManager.IsInteractable(positon)) {
                Debug.Log("can interac");
                Gamemanager.instance.TimapsManager.settileinterac(positon);
                
            }
        }
    }

    // Update is called once per frame

    void move()
    {
        SpeedX = Input.GetAxisRaw("Horizontal")*MoveSpeed;
        SpeedY = Input.GetAxisRaw("Vertical")*MoveSpeed;
        MyAnimator.SetFloat("SpeedX",SpeedX);
        MyAnimator.SetFloat("SpeedY", SpeedY);
        Movement = new Vector2(SpeedX,SpeedY);
        rb.linearVelocity = Movement;   
        if(SpeedX > 0 && !facingRight)
        {
            flip();
        }
        if(SpeedX < 0 && facingRight)
        {
            flip();
        }
    }
    void flip()
    {
        Vector3 currentScale = transform.localScale;    
        currentScale.x *= -1;
        transform.localScale = currentScale;
        facingRight =!facingRight;  
    }
}
