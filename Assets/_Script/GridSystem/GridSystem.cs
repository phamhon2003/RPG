using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    public static GridSystem Instance;

    public int width, height;
    public float cellSize = 1f;
    Vector3 startPosition=new Vector3(-38,-14,0);
    private Node[,] grid;

    void Awake()
    {
        Instance = this;
        CreateGrid();
        Debug.Log(GridToWorld(new Vector2Int(69,69)));
        Debug.Log(GridToWorld(new Vector2Int(0, 0)));
    }

    void CreateGrid()
    {
        grid = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = GridToWorld(new Vector2Int(x, y));

                // Kiểm tra nếu có collider chặn đường tại vị trí này bằng OverlapBox
                // Tạo một hình hộp với kích thước bằng với size của ô grid
                Collider2D hit = Physics2D.OverlapBox(worldPos, new Vector2(1f, 1f), 0f); // 0f là góc quay, có thể thay đổi nếu cần

                // Nếu có chướng ngại vật (hit != null), ô này không thể đi được
                bool walkable = hit == null; // Nếu không có va chạm, ô này có thể đi

                grid[x, y] = new Node(new Vector2Int(x, y), true);
            }
        }
    }

    public Node GetNode(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
            return grid[pos.x, pos.y];
        return null;
    }

    public Vector2Int WorldToGrid(Vector2 worldPos)
    {
        Vector2 offset = worldPos - (Vector2)startPosition;
        int x = Mathf.FloorToInt(offset.x / cellSize);
        int y = Mathf.FloorToInt(offset.y / cellSize);

        return new Vector2Int(x, y);
    }

    public Vector2 GridToWorld(Vector2Int gridPos)
    {
        Vector2 worldPos = (Vector2)startPosition + new Vector2(gridPos.x * cellSize, gridPos.y * cellSize);
        return worldPos;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        Vector2Int[] dirs = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        foreach (var dir in dirs)
        {
            Node n = GetNode(node.gridPos + dir);
            if (n != null && n.walkable)
                neighbours.Add(n);
        }

        return neighbours;
    }
}
