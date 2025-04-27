using UnityEngine;

public class Nest : MonoBehaviour
{
    public enum State { HaveEgge, NotEgge }
    [SerializeField] Sprite SpriteEgge, SpriteNotEgge;
    private SpriteRenderer spriteRenderer;
    [SerializeField] public State currentState;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState=State.NotEgge;
    }

    public void GetEgge()
    {
        spriteRenderer.sprite= SpriteEgge;
    }
}
