using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class TimapsManager : MonoBehaviour
{
    [SerializeField] Tilemap interactabbleMap;
    [SerializeField] Tile HiddeninteractabbleTile;
    [SerializeField] Tile TileInteract;
    public HashSet<Vector3> HS=new HashSet<Vector3>();
    void Start()
    {
        //foreach (var position in interactabbleMap.cellBounds.allPositionsWithin) {
        //    interactabbleMap.SetTile(position, HiddeninteractabbleTile); 
            
        //}
    }
    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile= interactabbleMap.GetTile(position);
        if (tile != null) { 
            if(tile.name == "interactable")
            {
                return true;
            }
            
        }
        return false;
    }
    public bool cantrongkhoai(Vector3Int position)
    {
        TileBase tile = interactabbleMap.GetTile(position);
        if (tile != null)
        {
            if (tile.name == "summer")
            {
                return true;
            }

        }
        return false;
    }
    public Vector3 getpos(Vector3Int posplayer)
    {     
        return interactabbleMap.GetCellCenterWorld(interactabbleMap.WorldToCell(posplayer));
    }
    public bool checkpos(Vector3 posplayer)
    {
        foreach (Vector3 pos in HS) {
            if(pos == posplayer)
            {
                return false;
            }
        }
        return true;
    }
    public void addpos(Vector3 posplayer)
    {
        HS.Add(posplayer);
    }
    public void movepos(Vector3 posplayer)
    {
        HS.Remove(posplayer);
    }
    public void settileinterac(Vector3Int position)
    {
        interactabbleMap.SetTile(position,TileInteract);
    }
    
}
