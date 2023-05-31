using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorNode : Node
{
    private Node struc1;
    private Node struc2;
    private int corridorWidth;
    private int modifierDistanceFromWall = 1;

    public CorridorNode(Node node1, Node node2, int corridorWidth) : base(null)
    {
        this.struc1 = node1;
        this.struc2 = node2;
        this.corridorWidth = corridorWidth;
        GeneratCorridor();
    }

    private void GeneratCorridor()
    {
        var relativePosStructure2 = CheckPosStruc2AgainstStruc1();

        switch (relativePosStructure2)
        {
            case RelativePosition.Up:
                ProcessRoomInRelationUpOrDown(this.struc1, this.struc2);
                break;
            case RelativePosition.Down:
                ProcessRoomInRelationUpOrDown(this.struc2, this.struc1);
                break;
            case RelativePosition.Right:
                ProcessRoomInRelationRightOrLeft(this.struc1, this.struc2);
                break;
            case RelativePosition.Left:
                ProcessRoomInRelationRightOrLeft(this.struc2, this.struc1);
                break;
            default:
                break;
        }
    }

    private void ProcessRoomInRelationUpOrDown(Node struc1, Node struc2)
    {
        Node bottomStructure = null;
        List<Node> structureBottomChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(struc1);
        
        Node topStructure = null;
        List<Node> strucutreAboveChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(struc2);

        var sortedBottomStructure = structureBottomChildren.OrderByDescending(
            child => child.TopRightAreaCorner.y).ToList();

        if(sortedBottomStructure.Count == 1)
        {
            bottomStructure = structureBottomChildren[0];
        }else
        {
            int maxY = sortedBottomStructure[0].TopLeftAreaCorner.y;
            sortedBottomStructure = sortedBottomStructure.Where(
                child => Mathf.Abs(maxY- child.TopLeftAreaCorner.y) < 10).ToList();

            int index = UnityEngine.Random.Range(0, sortedBottomStructure.Count);
            bottomStructure = sortedBottomStructure[index];
        }

        var possibleNeighboursInTopStructure = strucutreAboveChildren.Where(
            child => GetValiXForNeighbourUpDown(
                bottomStructure.TopLeftAreaCorner,
                bottomStructure.TopRightAreaCorner,
                child.BottomLeftAreaCorner,
                child.BottomRightAreaCorner)
                != -1
            ).OrderBy(child  => child.BottomRightAreaCorner.y).ToList();
        if(possibleNeighboursInTopStructure.Count == 0)
        {
            topStructure = struc2;
        }
        else
        {
            topStructure = possibleNeighboursInTopStructure[0];
        }

        int x = GetValiXForNeighbourUpDown(bottomStructure.TopLeftAreaCorner,
                bottomStructure.TopRightAreaCorner,
                topStructure.BottomLeftAreaCorner,
                topStructure.BottomRightAreaCorner);
        while(x == -1 && sortedBottomStructure.Count > 1){
            sortedBottomStructure = sortedBottomStructure.Where(
                child => child.TopLeftAreaCorner.x != topStructure.TopLeftAreaCorner.x).ToList();
            bottomStructure = sortedBottomStructure[0];

            x = GetValiXForNeighbourUpDown(bottomStructure.TopLeftAreaCorner,
                bottomStructure.TopRightAreaCorner,
                topStructure.BottomLeftAreaCorner,
                topStructure.BottomRightAreaCorner);
        }

        BottomLeftAreaCorner = new Vector2Int(x, bottomStructure.TopLeftAreaCorner.y);
        TopRightAreaCorner = new Vector2Int(x + this.corridorWidth, topStructure.BottomLeftAreaCorner.y);
    }

    private int GetValiXForNeighbourUpDown(Vector2Int bottomNodeLeft, Vector2Int bottomNodeRight, 
        Vector2Int topNodeLeft, Vector2Int topNodeRight)
    {
        if(topNodeLeft.x < bottomNodeLeft.x && bottomNodeRight.x < topNodeRight.x)
        {
            return StructureHelper.CalculateMiddlePoint(
                bottomNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                bottomNodeRight - new Vector2Int(modifierDistanceFromWall + modifierDistanceFromWall, 0)
                ).x;
        }
        if(topNodeLeft.x >= bottomNodeLeft.x && bottomNodeRight.x >= topNodeRight.x)
        {
            return StructureHelper.CalculateMiddlePoint(
                topNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                topNodeRight - new Vector2Int(this.corridorWidth + modifierDistanceFromWall, 0)
                ).x;
        }
        if (bottomNodeLeft.x >= topNodeLeft.x && bottomNodeLeft.x <= topNodeRight.x)
        {
            return StructureHelper.CalculateMiddlePoint(
                bottomNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                topNodeRight - new Vector2Int(this.corridorWidth + modifierDistanceFromWall,0)
                ).x;
        }
        if(bottomNodeRight.x <= topNodeRight.x && bottomNodeRight.x >= topNodeLeft.x)
        {
            return StructureHelper.CalculateMiddlePoint(
                topNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                bottomNodeRight - new Vector2Int(this.corridorWidth + modifierDistanceFromWall,0)
                ).x;
        }
        return -1;
    }

    private void ProcessRoomInRelationRightOrLeft(Node struc1, Node struc2)
    {
        Node leftStructure = null;
        List<Node> leftStructureChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(struc1);

        Node rightStructure = null;
        List<Node> rightStructureChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(struc2);

        //Raum von linker Seite finden, welcher am weitesten rechts ist
        var sortedLeftStructures = leftStructureChildren.OrderByDescending(child => child.TopRightAreaCorner.x).ToList();
        if (sortedLeftStructures.Count == 1)
        {
            leftStructure = sortedLeftStructures[0];
        }
        else
        {
            int maxX = sortedLeftStructures[0].TopRightAreaCorner.x;
            sortedLeftStructures = sortedLeftStructures.Where(
                children => Math.Abs(maxX - children.TopRightAreaCorner.x) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortedLeftStructures.Count);
            leftStructure = sortedLeftStructures[index];
        }

        //Raum von rechter Seite finden, welcher am weitesten links ist
        var possibleNeighboursInRightStructureList = rightStructureChildren.Where(
            child => GetValidYForNeighbourLeftRight(
                leftStructure.TopRightAreaCorner,
                leftStructure.BottomRightAreaCorner,
                child.TopLeftAreaCorner,
                child.BottomLeftAreaCorner
                ) != -1
            ).ToList();

        if (possibleNeighboursInRightStructureList.Count <= 0)
        {
            rightStructure = struc2;
        }
        else
        {
            rightStructure = possibleNeighboursInRightStructureList[0];
        }

        int y = GetValidYForNeighbourLeftRight(leftStructure.TopRightAreaCorner, leftStructure.BottomRightAreaCorner,
            rightStructure.TopLeftAreaCorner, rightStructure.BottomLeftAreaCorner);
        while(y == -1 && sortedLeftStructures.Count > 1)
        {
            sortedLeftStructures = sortedLeftStructures.Where(
                child => child.TopLeftAreaCorner.y != leftStructure.TopLeftAreaCorner.y).ToList();
            leftStructure = sortedLeftStructures[0];
            y = GetValidYForNeighbourLeftRight(leftStructure.TopRightAreaCorner, leftStructure.BottomRightAreaCorner,
                rightStructure.TopLeftAreaCorner, rightStructure.BottomLeftAreaCorner);
        }

        BottomLeftAreaCorner = new Vector2Int(leftStructure.BottomRightAreaCorner.x, y);
        TopRightAreaCorner = new Vector2Int(rightStructure.TopLeftAreaCorner.x, y + this.corridorWidth);
    }

    private int GetValidYForNeighbourLeftRight(Vector2Int leftNodeUp, Vector2Int leftNodeDown,
        Vector2Int rightNodeUp, Vector2Int rightNodeDown)
    {
        if (rightNodeUp.y >= leftNodeUp.y && leftNodeDown.y >= rightNodeDown.y)
        {
            return StructureHelper.CalculateMiddlePoint(
                leftNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                leftNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.corridorWidth)).y;
        }
        if (rightNodeUp.y <= leftNodeUp.y && leftNodeDown.y <= rightNodeDown.y)
        {
            return StructureHelper.CalculateMiddlePoint(
                rightNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                rightNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.corridorWidth)).y;
        }
        if (leftNodeUp.y >= rightNodeDown.y && leftNodeUp.y <= rightNodeUp.y)
        {
            return StructureHelper.CalculateMiddlePoint(
                rightNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                leftNodeUp - new Vector2Int(0, modifierDistanceFromWall)).y;
        }
        if (leftNodeDown.y >= rightNodeDown.y && leftNodeDown.y <= rightNodeUp.y)
        {
            return StructureHelper.CalculateMiddlePoint(
                leftNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                rightNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.corridorWidth)).y;
        }
        return -1;
    }

    private RelativePosition CheckPosStruc2AgainstStruc1()
    {
        Vector2 middlePointStructure1Temp =
            ((Vector2)struc1.TopRightAreaCorner + struc1.BottomLeftAreaCorner) / 2;
        Vector2 middlePointStrcuture2Temp =
            ((Vector2)struc2.TopRightAreaCorner + struc2.BottomLeftAreaCorner) / 2;

        float angle = CalculateAngle(middlePointStructure1Temp, middlePointStrcuture2Temp);

        if ((angle < 45 && angle >= 0) || (angle > -45 && angle < 0))
        {
            return RelativePosition.Right;
        }
        else if (angle > 45 && angle < 135)
        {
            return RelativePosition.Up;
        }
        else if (angle > -135 && angle < -45)
        {
            return RelativePosition.Down;
        }
        else
        {
            return RelativePosition.Left;
        }
    }

    private float CalculateAngle(Vector2 middlePointStructure1Temp, Vector2 middlePointStrcuture2Temp)
    {
        return Mathf.Atan2(middlePointStrcuture2Temp.y - middlePointStructure1Temp.y,
            middlePointStrcuture2Temp.x - middlePointStructure1Temp.x) * Mathf.Rad2Deg;
    }
}