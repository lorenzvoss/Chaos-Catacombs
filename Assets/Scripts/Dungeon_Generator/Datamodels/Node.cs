using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private Node Parent;
    private List<Node> childrenNodes;
    
    public List<Node> ChildrenNodes { get => childrenNodes; }

    public bool Visited { get; set; }
    public Vector2Int BottomLeftAreaCorner { get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopLeftAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }

    public int TreeLayerIndex { get; set; }

    public Node(Node parent)
    {
        childrenNodes = new List<Node>();
        Parent = parent;
        if(Parent != null)
        {
            Parent.AddChild(this);
        }
    }

    public void AddChild(Node node)
    {
        childrenNodes.Add(node);
    }

    public void RemoveChild(Node node)
    {
        childrenNodes.Remove(node);
    }
}