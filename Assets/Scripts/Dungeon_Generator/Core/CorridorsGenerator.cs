using System;
using System.Collections.Generic;
using System.Linq;

public class CorridorsGenerator
{
    public List<Node> CreateCorridors(List<RoomNode> allSpaceNodes, int corridorWidth)
    {
        List<Node> result = new List<Node>();
        Queue<RoomNode> structuresToCheck = new Queue<RoomNode>(
            allSpaceNodes.OrderByDescending(node => node.TreeLayerIndex).ToList());

        while( structuresToCheck.Count > 0 )
        {
            var node = structuresToCheck.Dequeue();
            if(node.ChildrenNodes.Count == 0 )
            {
                continue;
            }
            else
            {
                CorridorNode corridor = new CorridorNode(node.ChildrenNodes[0], node.ChildrenNodes[1], corridorWidth);
                result.Add(corridor);
            }
        }

        return result;
    }
}