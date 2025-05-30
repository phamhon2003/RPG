using UnityEngine;
using UnityEngine.Tilemaps;

public class Frence : MonoBehaviour
{
    public Tilemap fenceTilemap;
    public TileBase fenceHorizontal;  // Tile ngang
    public TileBase fenceVertical;    // Tile dọc (chung cho mọi hướng)
    public TileBase fenceTopLeft, fenceTopRight, fenceBottomLeft, fenceBottomRight;
    public TileBase fenceSingle;
    private void Start()
    {
        
    }

    void Update()
    {
        BuildFence();
    }
    public void BuildFence()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = fenceTilemap.WorldToCell(mousePos);

            if (fenceTilemap.GetTile(cellPos) != null) return;

            AudioManager.Instance.PlaySFX(AudioManager.Instance._Put);
            bool left = fenceTilemap.GetTile(cellPos + Vector3Int.left) != null;
            bool right = fenceTilemap.GetTile(cellPos + Vector3Int.right) != null;
            bool top = fenceTilemap.GetTile(cellPos + Vector3Int.up) != null;
            bool bottom = fenceTilemap.GetTile(cellPos + Vector3Int.down) != null;

            bool bottomLeft = fenceTilemap.GetTile(cellPos + Vector3Int.down + Vector3Int.left) != null;
            bool bottomRight = fenceTilemap.GetTile(cellPos + Vector3Int.down + Vector3Int.right) != null;
            bool topLeft = fenceTilemap.GetTile(cellPos + Vector3Int.up + Vector3Int.left) != null;
            bool topRight = fenceTilemap.GetTile(cellPos + Vector3Int.up + Vector3Int.right) != null;
            if (!top && !left && !bottom && !right)
            {
                fenceTilemap.SetTile(cellPos, fenceSingle);
                return;
            }
            if (right)
            {
                if (top)
                {
                    fenceTilemap.SetTile(cellPos, fenceBottomLeft);
                    fenceTilemap.SetTile(cellPos + Vector3Int.up, fenceVertical);
                    fenceTilemap.SetTile(cellPos + Vector3Int.right, fenceHorizontal);
                    return;
                }
                if (bottom)
                {
                    fenceTilemap.SetTile(cellPos, fenceTopLeft);
                    fenceTilemap.SetTile(cellPos + Vector3Int.down, fenceVertical);
                    fenceTilemap.SetTile(cellPos + Vector3Int.right, fenceHorizontal);
                    return;
                }
                if (topRight)
                {
                    fenceTilemap.SetTile(cellPos, fenceHorizontal);
                    fenceTilemap.SetTile(cellPos + Vector3Int.right, fenceBottomRight);
                    return;
                }
                if (bottomRight)
                {
                    fenceTilemap.SetTile(cellPos, fenceHorizontal);
                    fenceTilemap.SetTile(cellPos + Vector3Int.right, fenceTopRight);
                    return;
                }
                if (left)
                {
                    if (topLeft)
                    {
                        fenceTilemap.SetTile(cellPos, fenceHorizontal);
                        fenceTilemap.SetTile(cellPos + Vector3Int.left, fenceBottomLeft);
                        if(fenceTilemap.GetTile(cellPos + Vector3Int.right) == fenceSingle)
                            fenceTilemap.SetTile(cellPos + Vector3Int.right, fenceHorizontal);
                        return;
                    }
                    if (bottomLeft)
                    {
                        fenceTilemap.SetTile(cellPos, fenceHorizontal);
                        fenceTilemap.SetTile(cellPos + Vector3Int.left, fenceTopLeft);
                        if (fenceTilemap.GetTile(cellPos + Vector3Int.right) == fenceSingle)
                            fenceTilemap.SetTile(cellPos + Vector3Int.right, fenceHorizontal);
                        return;
                    }
                    fenceTilemap.SetTile(cellPos, fenceHorizontal);
                    fenceTilemap.SetTile(cellPos + Vector3Int.right, fenceHorizontal);
                    fenceTilemap.SetTile(cellPos + Vector3Int.left, fenceHorizontal);
                    return;
                }
                
                fenceTilemap.SetTile(cellPos, fenceHorizontal);
                fenceTilemap.SetTile(cellPos + Vector3Int.right, fenceHorizontal);
                return;
            }
            if (left)
            {
                if (top)
                {
                    fenceTilemap.SetTile(cellPos, fenceBottomRight);
                    fenceTilemap.SetTile(cellPos + Vector3Int.up, fenceVertical);
                    fenceTilemap.SetTile(cellPos + Vector3Int.left, fenceHorizontal);
                    return;
                }
                if (bottom)
                {
                    fenceTilemap.SetTile(cellPos, fenceTopRight);
                    fenceTilemap.SetTile(cellPos + Vector3Int.down, fenceVertical);
                    fenceTilemap.SetTile(cellPos + Vector3Int.left, fenceHorizontal);
                    return;
                }
                if (topLeft)
                {
                    fenceTilemap.SetTile(cellPos, fenceHorizontal);
                    fenceTilemap.SetTile(cellPos + Vector3Int.left, fenceBottomLeft);
                    return;
                }
                if (bottomLeft)
                {
                    fenceTilemap.SetTile(cellPos, fenceHorizontal);
                    fenceTilemap.SetTile(cellPos + Vector3Int.left, fenceTopLeft);
                    return;
                }
                fenceTilemap.SetTile(cellPos, fenceHorizontal);
                fenceTilemap.SetTile(cellPos + Vector3Int.left, fenceHorizontal);
                return;
            }
            if (top)
            {
                if (topRight)
                {
                    fenceTilemap.SetTile(cellPos, fenceVertical);
                    fenceTilemap.SetTile(cellPos + Vector3Int.up, fenceTopLeft);
                    return;
                }
                if (topLeft)
                {
                    fenceTilemap.SetTile(cellPos, fenceVertical);
                    fenceTilemap.SetTile(cellPos + Vector3Int.up, fenceTopRight);
                    return;
                }
                fenceTilemap.SetTile(cellPos, fenceVertical);
                return;
            }
            if (bottom)
            {
                if (bottomRight)
                {
                    fenceTilemap.SetTile(cellPos, fenceVertical);
                    fenceTilemap.SetTile(cellPos + Vector3Int.down, fenceBottomLeft);
                    return;
                }
                if (bottomLeft)
                {
                    fenceTilemap.SetTile(cellPos, fenceVertical);
                    fenceTilemap.SetTile(cellPos + Vector3Int.down, fenceBottomRight);
                    return;
                }
                fenceTilemap.SetTile(cellPos, fenceVertical);
                return;
            }
        }
    }
}


