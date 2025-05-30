using UnityEngine;

public class Laser : MonoBehaviour
{
 
    float Timer = 1.5f;
    
    void Start()
    {
    }
    private void OnEnable()
    {
        Timer = 1.5f;
    }
    public void rotate(Transform target)
    {
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    // Update is called once per frame
    void Update()
    {
        if (Timer >= 0)
            Timer -= Time.deltaTime;
        if (Timer < 0)
            this.gameObject.SetActive(false);
    }
}
