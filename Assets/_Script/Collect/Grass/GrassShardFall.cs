using UnityEngine;

public class GrassChip : MonoBehaviour
{
    [Header("Physics Settings")]
    public float minHorizontalForce = 1f;  // Lực ngang tối thiểu
    public float maxHorizontalForce = 3f;  // Lực ngang tối đa
    public float minVerticalForce = 2f;    // Lực đẩy lên tối thiểu
    public float maxVerticalForce = 5f;    // Lực đẩy lên tối đa
    public float lifetime = 1f;            // Thời gian tồn tại
    public float fadeDuration = 0.5f;      // Thời gian fade-out

    [Header("Bounce Settings")]
    public int maxBounces = 2;
    public float groundOffset = 0.5f;       // Giả định mặt đất cách initialY - 2f
    public float bounceDamping = 0.6f;    // Mỗi lần nảy yếu dần

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private float _currentLifetime;

    private float _initialY;
    private int _bounceCount = 0;
    private bool _startFading = false;
    private bool _hasBouncedThisFall = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentLifetime = lifetime;
        _initialY = transform.position.y;
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
        if (!_startFading)
        {
            float groundY = _initialY - groundOffset;

            if (_bounceCount < maxBounces && !_hasBouncedThisFall)
            {
                if (transform.position.y <= groundY && _rb.linearVelocity.y <= 0f)
                {
                    float bounceForce = Mathf.Lerp(maxVerticalForce, minVerticalForce, (float)_bounceCount / maxBounces) * bounceDamping;
                    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, bounceForce);
                    _bounceCount++;
                    _hasBouncedThisFall = true;
                }
            }

            // Reset flag khi đang bay lên
            if (_rb.linearVelocity.y > 0f)
            {
                _hasBouncedThisFall = false;
            }

            if (_bounceCount >= maxBounces)
            {
                _startFading = true;
                _currentLifetime = fadeDuration;
            }
        }
        else
        {
            _currentLifetime -= Time.deltaTime;
            float alpha = _currentLifetime / fadeDuration;
            _spriteRenderer.color = new Color(1, 1, 1, alpha);

            if (_currentLifetime <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
