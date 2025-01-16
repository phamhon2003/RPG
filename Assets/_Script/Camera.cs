using UnityEngine;

public class Camera : MonoBehaviour
{   
    public GameObject player;
    
    void Update()
    {   
       
        transform.position=new Vector3(player.transform.position.x, player.transform.position.y+3f,transform.position.z);
    }
}
