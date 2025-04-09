using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    public WeaponController weaponController;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    
}
