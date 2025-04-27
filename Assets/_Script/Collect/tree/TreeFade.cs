using UnityEngine;

public class TreeFade : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer, _spriteRendererinChild;
    private float originalAlpha;
    public float fadedAlpha = 0.4f;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        originalAlpha = _spriteRenderer.color.a;
        _spriteRendererinChild = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color c = _spriteRenderer.color;
            c.a = fadedAlpha;
            _spriteRenderer.color = c;
            _spriteRendererinChild.color = c;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color c = _spriteRenderer.color;
            c.a = originalAlpha;
            _spriteRenderer.color = c;
            _spriteRendererinChild.color = c;
        }
    }
}
