using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPainter : MonoBehaviour
{
    public Color startNodeColor;
    public Color goalNodeColor;
    public Color startNodeCheck;
    public Color goalNodeCheck;
    public Color pathColor;

    public LineRenderer pathRenderer;

    public float waitBetweenPaintedNodes;

    private WaitForSeconds waitToPaint;

    private void Start()
    {
        waitToPaint = new WaitForSeconds(waitBetweenPaintedNodes);
    }

    public void SetStartNodeColor(Node startNode)
    {
        startNode.SetColor(startNodeColor);
    }

    public void SetGoalNodeColor(Node goalNode)
    {
        goalNode.SetColor(goalNodeColor);
    }

    public void DisplayProccesedNodes(List<Node> processedNodes, IEnumerable<Node> path)
    {
        StartCoroutine(PaintNodes(processedNodes, path));
    }

    public void StopDisplayProccesedNodes()
    {
        StopAllCoroutines();
    }

    public void ResetGrid(List<Node> proccesedNodes)
    {
        pathRenderer.positionCount = 0;

        foreach (Node node in proccesedNodes)
        {
            node.ResetColor();
        }
    }

    private IEnumerator PaintNodes(List<Node> processedNodes, IEnumerable<Node> path)
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < processedNodes.Count; i++)
        {
            processedNodes[i].SetColor(Color.Lerp(startNodeCheck, goalNodeCheck, i / (float)(processedNodes.Count - 1)));
            yield return waitToPaint;
        }

        foreach (Node node in path)
        {
            node.SetColor(pathColor);
            positions.Add(new Vector3(node.gridPosition.x, node.gridPosition.y, -0.1f));
            yield return waitToPaint;
        }

        pathRenderer.positionCount = positions.Count;
        pathRenderer.SetPositions(positions.ToArray());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            pathRenderer.positionCount = 0;
        }
    }
}
