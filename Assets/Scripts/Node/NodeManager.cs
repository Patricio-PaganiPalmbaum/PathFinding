using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public Node nodePrefab;
    public int gridWidth;
    public int gridHeight;

    private Node[,] nodeGrid;

    private void Awake()
    {
        nodeGrid = new Node[gridWidth, gridHeight];
        CreateNodeGrid();
    }

    private void CreateNodeGrid()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                nodeGrid[i, j] = Instantiate(nodePrefab, new Vector3(i, j, 0f), Quaternion.identity, transform);
                nodeGrid[i, j].gridPosition = new Vector2Int(i, j);
            }
        }
    }

    public void SubscribeToOnClickNode(Action<Node> callback)
    {
        foreach (Node node in nodeGrid)
        {
            node.OnSelectCell += callback;
        }
    }

    public IEnumerable<Node> GetNeighboards(Node node, List<Vector2Int> directions)
    {
        return directions.Select(d => d + node.gridPosition)
                         .Where(gp => gp.x >= 0 && gp.x < nodeGrid.GetLength(0) && gp.y >= 0 && gp.y < nodeGrid.GetLength(1))
                         .Select(gp => nodeGrid[gp.x, gp.y])
                         .Where(n => !n.IsLocked);
    }

    public IEnumerable<Tuple<Node, float>> GetNeighboardsWithCost(Node node, List<Vector2Int> directions)
    {
        return GetNeighboards(node, directions).Select(n => Tuple.Create(n, n.Cost));
    }
}