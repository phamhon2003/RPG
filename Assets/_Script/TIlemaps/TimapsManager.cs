using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;



public class TimapsManager : MonoBehaviour
{
    public static TimapsManager instance;
    [SerializeField] PlayerController Player;
    public float _CooldownDig = 0f;
    public Tilemap interactabbleMap;
    [SerializeField] List<Tile> TileInteractable1;
    [SerializeField] Tile TileInteract, tileWatering;
    public HashSet<Vector3> HS = new HashSet<Vector3>();
    [SerializeField] private Transform highlightObject;
    Vector3 mouseWorldPos;
    public bool CanInteractableMap, HightlightTile, InteractFarm;
    Frence fence;
    public List<GameObject> Seeds;
    [SerializeField] GameObject Chest, Nest, Grass, Bonfire;
    [SerializeField] Item IChest, INest, IGrass, IBonfire;

    private void Awake()
    {
        instance = this;
        fence = GetComponent<Frence>();
    }
    private void Update()
    {
        if (_CooldownDig > 0)
        {
            _CooldownDig -= Time.deltaTime;
        }
        if (interactabbleMap == null) return;
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CanInteractableMap = IsInteractable(getpostile(mouseWorldPos));
        if (InventoryManager.instance.ItemSelection != null && !UIManager.Instance.IsOpenIventoryItem)
        {
            switch (InventoryManager.instance.ItemSelection.Name)
            {
                case "Shovel":
                    fence.enabled = false;
                    HightLight(CanInteractableMap);
                    InteractFarm = true;
                    break;
                case "Watering tools":
                    fence.enabled = false;
                    HightLight(Canwatering(getpostile(mouseWorldPos)));
                    InteractFarm = true;
                    break;
                case "Fence":
                    HightLight(CanInteractableMap);
                    fence.enabled = CanInteractableMap;
                    break;
                case "Seed Tomato" or "Seed Potato" or "Seed Wheat" or "Seed Cauliflower":
                    fence.enabled = false;
                    HightLight(CanPlantSeed(getpostile(mouseWorldPos)));
                    InteractFarm = true;
                    break;
                case "Chest":
                    fence.enabled = false;
                    HightLight(!CheckObstacel());
                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        PutItem(GetCenterTile(getpostile(mouseWorldPos)), Chest, IChest);
                    }
                    break;
                case "Nest":
                    fence.enabled = false;
                    HightLight(!CheckObstacel());
                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        PutItem(GetCenterTile(getpostile(mouseWorldPos)), Nest, INest);
                    }
                    break;
                case "Grass":
                    fence.enabled = false;
                    HightLight(!CheckObstacel());
                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        PutItem(GetCenterTile(getpostile(mouseWorldPos)), Grass, IGrass);
                    }
                    break;
                case "Bonfire":
                    fence.enabled = false;
                    HightLight(!CheckObstacel());
                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        PutItem(GetCenterTile(getpostile(mouseWorldPos)), Bonfire, IBonfire);
                    }
                    break;
                default:
                    InteractFarm = false;
                    highlightObject.gameObject.SetActive(false);
                    fence.enabled = false;
                    break;
            }
        }
        else
        {
            highlightObject.gameObject.SetActive(false);
            fence.enabled = false;
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
    public bool Canwatering (Vector3Int position)
    {   
        if(CheckObstacel()) return false;
        TileBase tile= interactabbleMap.GetTile(position);
        if (tile != null) {        
            if(tile == TileInteract)
                return true;     
        }
        return false;
    }
    public bool Iswatering(Vector3Int position)
    {
        if (CheckObstacel()) return false;
        TileBase tile = interactabbleMap.GetTile(position);
        if (tile != null)
        {
            if (tile == tileWatering)
                return true;
        }
        return false;
    }
    public bool IsInteractable (Vector3Int position)
    {
        if (CheckObstacel()) return false;
        TileBase tile = interactabbleMap.GetTile(position);
        if (tile != null)
        {
            foreach (Tile tileinteractable in TileInteractable1)
            {
                if (tile == tileinteractable)
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
    public bool CanPlantSeed(Vector3Int position)
    {
        TileBase tile = interactabbleMap.GetTile(position);
        if (tile != null)
        {
            if (tile==TileInteract|| tile == tileWatering)
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

    public void settileinterac(Vector3Int position,Tile tile)
    {
        interactabbleMap.SetTile(position, tile);
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
                    settileinterac(Pos, TileInteract);
                    Player.Dig();
                }
                break;
            case "Watering tools":
                if (Canwatering(Pos) && Gamemanager.instance._WateringTool.TotalWate>0)
                {
                    Gamemanager.instance._WateringTool.TotalWate -= 20;
                    _CooldownDig = 0.7f;
                    settileinterac(Pos, tileWatering);
                    Player.Watering();
                }
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
        if (!CanPlantSeed(getpostile(mouseWorldPos))) return;
        if (HS.Contains(PosSeed)) return;
        foreach (GameObject Seed in Seeds)
        {
            if (Seed.name == NameSeed)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance._PutSeed);
                Instantiate(Seed, PosSeed, Quaternion.identity).GetComponent<Seeds>().Nameprefab=NameSeed;           
                HS.Add(PosSeed);
                return;
            }
        }
    }
    public void PutItem(Vector3 PosSeed,GameObject Obj,Item item)
    {
        if (!CheckObstacel())
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance._PutSeed);
            Instantiate(Obj, PosSeed, Quaternion.identity);
            InventoryManager.instance.RemoveItem(item);
            PlacedObjectData data = new PlacedObjectData
            {
                prefabName = Obj.name,
                position = PosSeed,
                uniqueID = System.Guid.NewGuid().ToString()
            };

            SaveManager.Instance.placedObjects.Add(data);
            SaveManager.Instance.SaveDataInstatiate();
        }     
    }
    public void ResetTileWasWatering()
    {
        BoundsInt bounds = interactabbleMap.cellBounds;
        TileBase[] tiles = interactabbleMap.GetTilesBlock(bounds);

        int i = 0;
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                if (tiles[i++] == tileWatering)
                {
                    settileinterac(new Vector3Int(x, y, 0), TileInteract);
                }
            }
        }
    }
}
