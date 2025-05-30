using System.Net.Sockets;
using UnityEngine;

public class HandBoss : Bullet
{
    public float lifetime = 5f;

    //public float straightTime = 0.5f; // Bay thẳng ban đầu
    public float rotateSpeed = 150f; 
    public float moveSpeed = 3f;

    private Transform target;
    private bool hasLockedDirection = false;


    protected override void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        Destroy(gameObject, lifetime);      
    }
    public void init(Transform target)
    {
        this.target = target;
    }
    void FixedUpdate()
    {
        lifetime += Time.fixedDeltaTime;
        Chase();
    }
    private void Chase()
    {
        if (target == null)
        {
            _rb.linearVelocity = transform.up * _speed;
            return;
        }

        if (!hasLockedDirection)
        {
            Vector2 toTarget = (Vector2)target.position - _rb.position;
            if (toTarget.magnitude < 2f)
            {
                hasLockedDirection = true;
            }
            else
            {
                Vector2 directionToTarget = toTarget.normalized;
                float angleToTarget = Vector2.SignedAngle(-transform.right, directionToTarget);
                float rotateStep = rotateSpeed * Time.fixedDeltaTime;
                float clampedAngle = Mathf.Clamp(angleToTarget, -rotateStep, rotateStep);
                transform.Rotate(0, 0, clampedAngle);
            }
        }
        _rb.linearVelocity = -transform.right * _speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // TODO: Gây damage
            Debug.Log("Tên lửa trúng player!");
            Destroy(gameObject);
        }
        else if (!collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
