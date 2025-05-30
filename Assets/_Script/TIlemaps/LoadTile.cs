using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;

public class LoadTile : MonoBehaviour
{
    [SerializeField] string Filename;
    public Tilemap tilemap;
    public List<TileBase> tileReferences; // gán trong inspector
    private void Start()
    {
        tilemap =  GetComponent<Tilemap>();
        //DeleteSaveFile(Filename);
        LoadTilemap(Filename);
       
    }
    
    public void LoadTilemap(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        TilemapSaveData saveData = JsonUtility.FromJson<TilemapSaveData>(json);

        tilemap.ClearAllTiles();

        foreach (TileData data in saveData.tiles)
        {
            TileBase tile = tileReferences.Find(t => t.name == data.tileName);
            if (tile != null)
                tilemap.SetTile(data.position, tile);
        }
        Debug.Log("Loaded tilemap from " + path);
    }
  
}
