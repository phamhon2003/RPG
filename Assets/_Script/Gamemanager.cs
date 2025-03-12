using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    public TimapsManager TimapsManager;
    public WeaponController weaponController;
    private void Awake()
    {
        instance = this;
        TimapsManager= GetComponent<TimapsManager>();
    }
    void Start()
    {

    }

    
}
