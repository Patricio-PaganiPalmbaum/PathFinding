using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridExplorer : MonoBehaviour
{
    private enum EMovementType { Straight, Dianogal }

    public NodeManager nodeManager;
    public GridPainter gridPainter;

    private Node startNode;
    private Node goalNode;

    private Action<Node> SetNode;

    private EMovementType currentMovementType;
    private int currentPathFindingAlgorithm;

    private List<Vector2Int> Directions => (currentMovementType == EMovementType.Straight) ? Constants.straightDirections : Constants.diagonalDirections;
    private List<Node> processedNodes;
    private IEnumerable<Node> path;

    void Start()
    {
        nodeManager.SubscribeToOnClickNode(OnSelectNode);
        SetNode = SetStartNode;

        processedNodes = new List<Node>();
    }

    public void SetCurrentPathFindingAlgorithm(int pathFindingAlgorithm)
    {
        currentPathFindingAlgorithm = pathFindingAlgorithm;
    }

    public void SetCurrentMovementType(int movementType)
    {
        currentMovementType = (EMovementType)movementType;
    }

    private void ExecutePathFinding()
    {
        SetNode = SetStartNode;

        gridPainter.ResetGrid(processedNodes);
        processedNodes.Clear();

        gridPainter.SetGoalNodeColor(goalNode);

        switch (currentPathFindingAlgorithm)
        {
            case 0:
                ExecuteBFS();
                break;
            case 1:
                ExecuteDFS();
                break;
            case 2:
                ExecuteDijkstra();
                break;
            case 3:
                ExecuteAStar();
                break;
        }

        gridPainter.StopDisplayProccesedNodes();
        gridPainter.DisplayProccesedNodes(processedNodes, new List<Node>() { startNode }.Concat(path));
    }

    private void ExecuteBFS()
    {
        path = BreadthFirstSearch.Run(startNode,
                                      Satisfies,
                                      (n) => nodeManager.GetNeighboards(n, Directions));
    }

    private void ExecuteDFS()
    {
        path = DepthFirstSearch.Run(startNode,
                                    Satisfies,
                                    (n) => nodeManager.GetNeighboards(n, Directions));
    }

    private void ExecuteDijkstra()
    {
        path = Dijkstra.Run(startNode,
                            Satisfies,
                            (n) => nodeManager.GetNeighboardsWithCost(n, Directions));
    }

    private void ExecuteAStar()
    {
        path = AStar.Run(startNode,
                         Satisfies,
                         (n) => nodeManager.GetNeighboardsWithCost(n, Directions),
                         (n) => Vector2Int.Distance(goalNode.gridPosition, n.gridPosition));
    }

    private bool Satisfies(Node node)
    {
        processedNodes.Add(node);
        return node == goalNode;
    }

    private void OnSelectNode(Node selectedNode)
    {
        if (selectedNode.IsLocked)
        {
            return;
        }

        SetNode.Invoke(selectedNode);
    }

    private void SetStartNode(Node node)
    {
        gridPainter.ResetGrid(processedNodes);

        startNode?.ResetColor();
        startNode = node;

        gridPainter.StopDisplayProccesedNodes();
        gridPainter.SetStartNodeColor(startNode);

        SetNode = SetEndNode;
    }

    private void SetEndNode(Node node)
    {
        goalNode?.ResetColor();
        goalNode = node;
        gridPainter.SetGoalNodeColor(goalNode);

        SetNode = SetStartNode;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            startNode = goalNode = null;
        }

        if (Input.GetKeyDown(KeyCode.Space) && startNode && goalNode)
        {
            ExecutePathFinding();
        }
    }
}