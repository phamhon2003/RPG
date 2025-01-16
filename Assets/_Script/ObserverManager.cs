using System.Collections.Generic;
using UnityEngine;

public class ObserverManager : MonoBehaviour
{
    public static ObserverManager Instance;
    private List<action> Action = new List<action>();
    private void Awake()
    {
        Instance = this;
    }
    public void addObserver(action Act)
    {
        Action.Add(Act);
    }
    public void MoveObserver(action Act)
    {
        Action.Remove(Act);
    }
    public void attack()
    {
        foreach (action Act in Action) {
            Act.attack();             
        }
    }
}
