using UnityEngine;

public class Line
{
    Orientation orientation;
    Vector2Int coordinates;

    public Line(Orientation orientation, Vector2Int coordinates)
    {
        this.orientation = orientation;
        this.coordinates = coordinates;
    }

    public Orientation Orientation { get => orientation; set => Orientation = value; }
    public Vector2Int Coordinates { get => coordinates; set => Coordinates = value; }
}

public enum Orientation
{
    Horizontal = 0,
    Vertical = 1,
}