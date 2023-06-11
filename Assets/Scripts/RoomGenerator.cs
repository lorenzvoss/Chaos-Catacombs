using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    private int maxIterations;
    private int roomLengthMin;
    private int roomWidthMin;

    public RoomGenerator(int maxIterations, int roomLengthMin, int roomWidthMin)
    {
        this.maxIterations = maxIterations;
        this.roomLengthMin = roomLengthMin;
        this.roomWidthMin = roomWidthMin;
    }

    public List<RoomNode> GenerateRoomsInGivenSpace(List<Node> roomSpaces, float roomBottomCornerModifier,
        float roomTopCornerModifier, int roomOffset)
    {
        List<RoomNode> result = new List<RoomNode>();
        foreach(var space in roomSpaces)
        {
            Vector2Int newBottomLeftPoint = space.BottomLeftAreaCorner + new Vector2Int(2, 2);

            Vector2Int newTopRightPoint = space.TopRightAreaCorner - new Vector2Int(2, 2);

            space.BottomLeftAreaCorner = newBottomLeftPoint;
            space.TopRightAreaCorner = newTopRightPoint;
            space.BottomRightAreaCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
            space.TopLeftAreaCorner = new Vector2Int(newBottomLeftPoint.x, newTopRightPoint.y);
            result.Add((RoomNode) space);
        }
        return result;
    }
}