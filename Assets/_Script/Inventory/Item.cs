using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Scriptable object/Item")]
public class Item : ScriptableObject 
{
    public int count=0;
    public string Name;
    public TileBase Tile;
    public Sprite image;
    public Itemtype type;
    public Actiontype actiontype;
    public Vector2Int range= new Vector2Int(5,4);
    public bool stackable=true;
}
public enum Itemtype
{   
    itemFarming,
    BuildingBLock,
    Tool
}
public enum Actiontype
{
    Dig,
    Mine
}