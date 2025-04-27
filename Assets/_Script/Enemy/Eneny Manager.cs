using System.Collections.Generic;
using UnityEngine;

public class EnenyManager : MonoBehaviour
{
    public static EnenyManager Instance { get; private set; }

    public List<Enemy> activeEnemies = new List<Enemy>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
}
