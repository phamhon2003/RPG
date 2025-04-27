using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;



public class TimapsManager : MonoBehaviour
{
    public static TimapsManager instance;
    [SerializeField] PlayerController Player;
    public float _CooldownDig = 0f;
    public Tilemap interactabbleMap;
    [SerializeField] List<Tile> TileInteractable1;
    [SerializeField] Tile TileInteract;
    public HashSet<Vector3> HS=new HashSet<Vector3>();
    [SerializeField] private Transform highlightObject;
    Vector3 mouseWorldPos ;
    public bool CanInteractableMap,HightlightTile,InteractFarm;
    Frence fence;
    public List<GameObject> Seeds;
    private void Awake()
    {
        instance = this;
        fence = GetComponent<Frence>();
    }
    private void Update()
    {
        if (_CooldownDig > 0)
        {
            _CooldownDig-=Time.deltaTime;
        }
        if (interactabbleMap == null) return;
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CanInteractableMap = IsInteractable(getpostile(mouseWorldPos));
        if (InventoryManager.instance != null && InventoryManager.instance.ItemSelection != null)
        {
            switch (InventoryManager.instance.ItemSelection.Name)
            {
                case "Shovel":
                    HightLight(CanInteractableMap);
                    InteractFarm = true;
                    break;
                case "Fence":
                    HightLight(CanInteractableMap);
                    fence.enabled = CanInteractableMap;
                    break;
                case "Seed Tomato" or "Seed Potato" or "Seed Wheat" or "Seed Cauliflower":
                    HightLight(cantrongkhoai(getpostile(mouseWorldPos)));
                    InteractFarm = true;
                    break;
                default:
                    InteractFarm = false;                  
                    highlightObject.gameObject.SetActive(false);
                    fence.enabled = false;
                    break;
            }
        }
    }
    void HightLight(bool CanHightLight)
    {
        if (CanHightLight)
        {
            highlightObject.gameObject.SetActive(true);
            highlightObject.position = GetCenterTile(getpostile(mouseWorldPos));
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
        if(CheckObstacel()) return false;
        TileBase tile= interactabbleMap.GetTile(position);
        if (tile != null) { 
            foreach(Tile tileinteractable in TileInteractable1)
            {
                if(tile == tileinteractable)
                    return true;
            }
        }
        return false;
    }
    private bool CheckObstacel()
    {
        Vector3 worldPosition = GetCenterTile(getpostile(mouseWorldPos));
        int obstacleLayer = LayerMask.GetMask("Obstacle");  
        Collider2D hit = Physics2D.OverlapCircle(worldPosition, 0.4f, obstacleLayer);
        return hit != null;
    }
    public bool cantrongkhoai(Vector3Int position)
    {
        TileBase tile = interactabbleMap.GetTile(position);
        if (tile != null)
        {
            if (tile==TileInteract)
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

    public void settileinterac(Vector3Int position)
    {
        interactabbleMap.SetTile(position,TileInteract);
    }
    public Vector3Int getpostile(Vector3 Pos)
    {
        return interactabbleMap.WorldToCell(Pos);
    }
    public Vector3 GetCenterTile(Vector3Int position)
    { 
        return interactabbleMap.GetCellCenterWorld(position);
    }
    public void HandleInteraction(Vector3Int Pos,Vector3 PosSeed)
    {
        switch (InventoryManager.instance.ItemSelection.Name)
        {
            case "Shovel":
                if (IsInteractable(Pos))
                {                   
                    _CooldownDig = 0.7f; 
                    settileinterac(Pos);
                    Player.Dig();
                }
                //GetComponentInChildren<Harvest>().CanHarvest();
                break;
            case "Seed Potato":
                PlantSeeds("Seed Potato", PosSeed);
                break;
            case "Seed Wheat":
                PlantSeeds("Seed Wheat", PosSeed);
                break;
            case "Seed Cauliflower":
                PlantSeeds("Seed Cauliflower", PosSeed);
                break;
            case "Seed Tomato":
                PlantSeeds("Seed Tomato", PosSeed);
                break;
            default:
                break;
        }
    }
    public void PlantSeeds(string NameSeed,Vector3 PosSeed)
    {
        if (HS.Contains(PosSeed)) return;
        foreach (GameObject Seed in Seeds)
        {
            if (Seed.name == NameSeed)
            {
                Instantiate(Seed, PosSeed, Quaternion.identity);
                HS.Add(PosSeed);
                return;
            }
        }
    }  
}
