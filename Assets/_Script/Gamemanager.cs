using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    public TimapsManager TimapsManager;
    private void Awake()
    {
        instance = this;
        TimapsManager= GetComponent<TimapsManager>();
    }
    void Start()
    {
        
    }

    
}
