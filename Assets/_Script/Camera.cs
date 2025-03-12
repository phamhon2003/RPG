using UnityEngine;

public class camera : MonoBehaviour
{   
    public GameObject player;
    
    void Update()
    {        
        transform.position=new Vector3(player.transform.position.x, player.transform.position.y,transform.position.z);
    }
}
