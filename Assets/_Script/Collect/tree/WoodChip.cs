using UnityEngine;

public class WoodChip : MonoBehaviour
{
    [Header("Physics Settings")]
    public float minHorizontalForce = 1f;  // Lực ngang tối thiểu
    public float maxHorizontalForce = 3f;  // Lực ngang tối đa
    public float minVerticalForce = 2f;    // Lực đẩy lên tối thiểu
    public float maxVerticalForce = 5f;    // Lực đẩy lên tối đa
    public float lifetime = 1f;            // Thời gian tồn tại
    public float fadeDuration = 0.5f;      // Thời gian fade-out

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private float _currentLifetime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentLifetime = lifetime;

        // Tự động áp dụng lực ngẫu nhiên khi spawn
        ApplyRandomForce();
    }

    private void ApplyRandomForce()
    {
        Vector2 force = new Vector2(
            Random.Range(-1f, 1f) * Random.Range(minHorizontalForce, maxHorizontalForce),
            Random.Range(minVerticalForce, maxVerticalForce)
        );
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Update()
    {
        _currentLifetime -= Time.deltaTime;

        // Fade-out
        if (_currentLifetime <= fadeDuration)
        {
            float alpha = _currentLifetime / fadeDuration;
            _spriteRenderer.color = new Color(1, 1, 1, alpha);
        }

        // Tự hủy khi hết thời gian
        if (_currentLifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
