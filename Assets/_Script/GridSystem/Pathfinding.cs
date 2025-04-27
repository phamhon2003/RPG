using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    public static List<Node> FindPath(Vector2 startWorld, Vector2 targetWorld)
    {
        var grid = GridSystem.Instance;

        Vector2Int start = grid.WorldToGrid(startWorld);
        Vector2Int target = grid.WorldToGrid(targetWorld);

        Node startNode = grid.GetNode(start);
        Node targetNode = grid.GetNode(target);

        if (startNode == null || targetNode == null || !targetNode.walkable)
            return null;

        List<Node> openSet = new List<Node> { startNode };
        HashSet<Node> closedSet = new HashSet<Node>();

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);
        startNode.parent = null;

        while (openSet.Count > 0)
        {
            Node current = openSet.OrderBy(n => n.fCost).ThenBy(n => n.hCost).First();

            if (current == targetNode)
                return RetracePath(startNode, targetNode);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbour in grid.GetNeighbours(current))
            {
                if (closedSet.Contains(neighbour)) continue;

                int tentativeG = current.gCost + GetDistance(current, neighbour);

                if (tentativeG < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = tentativeG;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = current;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return null; // Không tìm thấy đường
    }

    static int GetDistance(Node a, Node b)
    {
        // Khoảng cách Manhattan vì đi 4 hướng
        return Mathf.Abs(a.gridPos.x - b.gridPos.x) + Mathf.Abs(a.gridPos.y - b.gridPos.y);
    }

    static List<Node> RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }
}
