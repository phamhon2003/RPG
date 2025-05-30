    using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float _speed, _dmg, _lifetime;
    [SerializeField] protected Rigidbody2D _rb;
    protected Vector2 _movement=Vector2.zero;
    protected virtual void Start()
    {
        if (_rb == null)
        {
            _rb=GetComponent<Rigidbody2D>();
        }
    }
    public void Init(float speed, float dmg, float lifetime, Vector2 movement)
    {
        this._speed = speed;
        this._dmg = dmg;
        this._lifetime = lifetime;
        this._movement = movement;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        _rb.linearVelocity=_movement*_speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<action>().takedamage(_dmg);
            gameObject.SetActive(false);
        }
    }
}
