using UnityEngine;

public class FireBall : Bullet
{
    [SerializeField] GameObject FireballEffect;
    protected override void Start()
    {
        base.Start();
        
    }
    void Update()
    {
        if (_lifetime >= 0)
        {
            _lifetime -= Time.deltaTime;
            return;
        }
        if (_lifetime < 0)
        {
            Instantiate(FireballEffect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
