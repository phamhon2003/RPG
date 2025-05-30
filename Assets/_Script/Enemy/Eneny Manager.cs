using System.Collections.Generic;
using UnityEngine;

public class EnenyManager : MonoBehaviour
{
    public static EnenyManager Instance { get; private set; }

    public List<Transform> activeEnemies = new List<Transform>();

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
