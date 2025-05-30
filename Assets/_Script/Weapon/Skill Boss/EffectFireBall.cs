using UnityEngine;

public class EffectFireBall : MonoBehaviour
{
    float Timer = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Timer >= 0)
        {
            Timer -= Time.deltaTime;
            return;
        }
        if (Timer < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
