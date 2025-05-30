using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
public class Savetile : MonoBehaviour
{
    [SerializeField] string filename;
    public Tilemap tilemap;
    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }
    public void SaveTilemap(string fileName)
    {
        var saveData = new TilemapSaveData();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int pos = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);
                TileBase tile = tilemap.GetTile(pos);
                if (tile != null)
                {
                    saveData.tiles.Add(new TileData
                    {
                        position = pos,
                        tileName = tile.name // Đặt đúng tên để load lại
                    });
                }
            }
        }
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName + ".json", json);
        Debug.Log("Saved to " + Application.persistentDataPath + "/" + fileName + ".json");
    }
    void OnDisable()
    {
        SaveTilemap(filename);
    }
    void OnApplicationQuit()
    {
        SaveTilemap(filename);
    }
}
