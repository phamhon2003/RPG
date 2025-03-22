using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    Enemy enemy;
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    
    void Update()
    {
        
    }
}
