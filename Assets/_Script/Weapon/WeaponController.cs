using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Bullet _bullet;   
    [SerializeField] float _bulletspeed, _bulletdamege, _lifetime;
    public Transform player;
    public float orbitRadius = 1f;
    void Start()
    {


    }
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;  
        Vector2 direction = (mouseWorldPos - player.position).normalized;
        transform.position = (Vector2)player.position + direction * orbitRadius;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);      
    }

    public void Fire()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Bullet b = Instantiate<Bullet>(_bullet, this.transform.position, rotation);
        b.Init(_bulletspeed, _bulletdamege, _lifetime, direction);
    }   
}