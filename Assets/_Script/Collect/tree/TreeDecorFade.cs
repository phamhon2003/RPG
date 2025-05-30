using UnityEngine;

public class TreeDecorFade : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private float originalAlpha;
    public float fadedAlpha = 0.4f;

    void Start()
    {
        _spriteRenderer = GetComponentInParent<SpriteRenderer>();
        originalAlpha = _spriteRenderer.color.a;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color c = _spriteRenderer.color;
            c.a = fadedAlpha;
            _spriteRenderer.color = c;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color c = _spriteRenderer.color;
            c.a = originalAlpha;
            _spriteRenderer.color = c;
        }
    }
}
