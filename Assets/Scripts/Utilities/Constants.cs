using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static readonly List<Vector2Int> straightDirections = new List<Vector2Int>()
    {
        Vector2Int.right,
        Vector2Int.left,
        Vector2Int.down,
        Vector2Int.up
    };

    public static readonly List<Vector2Int> diagonalDirections = new List<Vector2Int>()
    {
        Vector2Int.right,
        Vector2Int.down + Vector2Int.right,
        Vector2Int.left,
        Vector2Int.down + Vector2Int.left,
        Vector2Int.down,
        Vector2Int.up,
        Vector2Int.up + Vector2Int.right,
        Vector2Int.up + Vector2Int.left
    };
}
