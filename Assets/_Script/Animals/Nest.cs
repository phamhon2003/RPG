using UnityEngine;

public class Nest : MonoBehaviour
{
    public enum State { HaveEgge, NotEgge }
    public bool Haveegg =false , isPlayerInRange;
    public bool Ischicken=false;
    [SerializeField] Sprite SpriteEgge, SpriteNotEgge;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] public State currentState;
    [SerializeField] Item item;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        currentState=State.NotEgge;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isPlayerInRange && Haveegg)
        {
            collectEgge();
        }
    }
    public void GetEgge()
    {
        _spriteRenderer.sprite= SpriteEgge;
        Haveegg=true;
    }
    public void collectEgge()
    {
        _spriteRenderer.sprite = SpriteNotEgge;
        Haveegg = false;
        UIManager.Instance.TextCollectUI.SetActive(false);
        InventoryManager.instance.addItem(item);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if(Haveegg)
                UIManager.Instance.DirectionCollectUI(transform.position);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            UIManager.Instance.TextCollectUI.SetActive(false);
        }
    }
}
