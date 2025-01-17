﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StructureHelper
{
    public static List<Node> TraverseGraphToExtractLowestLeafes(Node parentNode)
    {
        Queue<Node> nodesToCheck = new Queue<Node>();
        List<Node> result = new List<Node>();
        if (parentNode.ChildrenNodes.Count == 0)
        {
            return new List<Node>() { parentNode };
        }
        foreach (var child in parentNode.ChildrenNodes)
        {
            nodesToCheck.Enqueue(child);
        }
        while (nodesToCheck.Count > 0)
        {
            var currentNode = nodesToCheck.Dequeue();
            if (currentNode.ChildrenNodes.Count == 0)
            {
                result.Add(currentNode);
            }
            else
            {
                foreach (var child in currentNode.ChildrenNodes)
                {
                    nodesToCheck.Enqueue(child);
                }
            }
        }
        return result;
    }

    public static Vector2Int GenerateBottomLeftCornerBetween(
        Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;

        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;
        return new Vector2Int(
            Random.Range(minX, (int)(minX + (maxX - minX) * pointModifier)),
            Random.Range(minY, (int)(minY + (maxY - minY) * pointModifier))
            );
    }

    public static Vector2Int GenerateTopRightCornerBetween(
        Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;

        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;
        return new Vector2Int(
            Random.Range((int)(minX + (maxX - minX) * pointModifier), maxX),
            Random.Range((int)(minY + (maxY - minY) * pointModifier), maxY)
            );
    }

    public static Vector2Int CalculateMiddlePoint(Vector2Int v1, Vector2Int v2)
    {
        Vector2 sum = v1 + v2;
        Vector2 tmpVector = sum / 2;
        return new Vector2Int((int)tmpVector.x, (int)tmpVector.y);
    }
}

public enum RelativePosition
{
    Up,
    Down,
    Left,
    Right,

}
