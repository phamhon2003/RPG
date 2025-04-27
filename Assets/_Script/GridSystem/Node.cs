using UnityEngine;

public class Node 
{
    public Vector2Int gridPos;    
    public bool walkable;        
    public int gCost, hCost;      // g: từ start, h: tới đích
    public Node parent;           

    public int fCost => gCost + hCost;

    public Node(Vector2Int pos, bool walkable)
    {
        this.gridPos = pos;
        this.walkable = walkable;
    }
}
