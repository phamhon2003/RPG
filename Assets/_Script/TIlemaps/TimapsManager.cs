using UnityEngine;
using UnityEngine.Tilemaps;



public class TimapsManager : MonoBehaviour
{
    [SerializeField] Tilemap interactabbleMap;
    [SerializeField] Tile HiddeninteractabbleTile;
    [SerializeField] Tile TileInteract;
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
    public void settileinterac(Vector3Int position)
    {
        interactabbleMap.SetTile(position,TileInteract);
    }
    
}
