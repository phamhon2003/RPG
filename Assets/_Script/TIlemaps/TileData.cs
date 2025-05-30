using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

[System.Serializable]
public class TileData
{
    public Vector3Int position;
    public string tileName; // tên tile để load lại
}
[System.Serializable]
public class TilemapSaveData
{
    public List<TileData> tiles = new List<TileData>();
}