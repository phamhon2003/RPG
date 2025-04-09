using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class TimapsManager : MonoBehaviour
{
    public static TimapsManager instance;
    [SerializeField] Tilemap interactabbleMap;
    [SerializeField] Tile HiddeninteractabbleTile;
    [SerializeField] Tile TileInteract;
    public HashSet<Vector3> HS=new HashSet<Vector3>();
    [SerializeField] private Transform highlightObject;
    Vector3 mouseWorldPos ;
    public bool CanInteractableMap,HightlightTile;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CanInteractableMap = IsInteractable(getpostile(mouseWorldPos));
        //highlightObject.gameObject.SetActive(true);///
        //highlightObject.position = GetCenterTile();///
        if (InventoryManager.instance != null && InventoryManager.instance.ItemSelection != null)
        {
            switch (InventoryManager.instance.ItemSelection.Name)
            {
                case "Shovel":
                    HightLight(CanInteractableMap);
                    break;
                case "Khoai":
                    HightLight(cantrongkhoai(getpostile(mouseWorldPos)));
                    break;
                default:
                    highlightObject.gameObject.SetActive(false);
                    break;
            }
        }
    }
    void HightLight(bool CanHightLight)
    {
        if (CanHightLight)
        {
            highlightObject.gameObject.SetActive(true);
            highlightObject.position = GetCenterTile();
            HightlightTile= true;
        }
        else
        {
            highlightObject.gameObject.SetActive(false);
            HightlightTile = false;
        }
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
    public Vector3Int getpostile(Vector3 Pos)
    {
        return interactabbleMap.WorldToCell(Pos);
    }
    public Vector3 GetCenterTile()
    { 
        mouseWorldPos.z = 0;
        Vector3Int tilePosition= getpostile(mouseWorldPos);
        return interactabbleMap.GetCellCenterWorld(tilePosition);
    } 
}
