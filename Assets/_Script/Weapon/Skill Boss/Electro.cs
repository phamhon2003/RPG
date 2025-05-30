using UnityEngine;

public class Electro : MonoBehaviour
{
    private Transform target;
    float Timer = 2f;
    public void init(Transform target)
    {
        this.target = target;
        transform.position = target.position;
    }

    void Update()
    {
        if (Timer >= 0) 
            Timer -= Time.deltaTime;
        if (Timer < 0)
            this.gameObject.SetActive(false);
    }
}
