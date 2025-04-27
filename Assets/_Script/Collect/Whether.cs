using UnityEngine;

public class Whether : MonoBehaviour
{
    [SerializeField] GameObject _camera;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position;    
    }
}
