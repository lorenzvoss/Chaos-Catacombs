using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using UnityEngine;

public class DungeonGenerator
{
    List<RoomNode> allSpaceNodes = new List<RoomNode>();
    private int DungeonWidht;
    private int DungeonLength;

    public DungeonGenerator(int widht, int length)
    {
        DungeonWidht = widht;
        DungeonLength = length;
    }

    public List<Node> CalculateDungeon(int maxIterations, int roomWidthMin, int roomLengthMin, 
        float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidth)
    {
        BinarySpacePartitioner partitioner = new BinarySpacePartitioner(DungeonWidht, DungeonLength);
        allSpaceNodes = partitioner.PrepareNodesCollection(maxIterations, roomWidthMin, roomLengthMin);
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeafes(partitioner.RootNode);

        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomLengthMin, roomWidthMin);
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpace(roomSpaces, roomBottomCornerModifier,
            roomTopCornerModifier, roomOffset);

        CorridorsGenerator corridorGenerator = new CorridorsGenerator();
        var cooridorList = corridorGenerator.CreateCorridors(allSpaceNodes, corridorWidth);
        
        return new List<Node>(roomList).Concat(cooridorList).ToList();
    }
}