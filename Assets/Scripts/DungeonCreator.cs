using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth;
    public int dungeonLength;
    public int roomWidthMin, roomLengthMin;
    public int maxIterations;
    public int corridorWidht;
    public Material material;
    [Range(0.0f, 0.3f)]
    public float RoomBottomCornerModifier;
    [Range(0.7f, 1f)]
    public float RoomTopCornerModifier;
    [Range(0, 2)]
    public int RoomOffset;

    public GameObject wallVertical, wallHorizontal;

    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;

    public GameObject PlayerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        CreateDungeon();
    }

    /// <summary>
    /// Erstellt Dungeon inklusive Boden, Waenden, (work in progress) Spieler, (work in progress)Gegnern
    /// </summary>
    public void CreateDungeon()
    {
        DestroyAllChildren();

        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateDungeonRooms(maxIterations, roomWidthMin, roomLengthMin,
            RoomBottomCornerModifier, RoomTopCornerModifier, RoomOffset);
        var listOfCorridors = generator.CalculateDungeonCorridors(corridorWidht);

        var dungeonList = listOfRooms.Concat(listOfCorridors).ToList();

        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;

        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();

        for (int i = 0; i < dungeonList.Count; i++)
        {
            CreateMesh(dungeonList[i].BottomLeftAreaCorner, dungeonList[i].TopRightAreaCorner);
        }
        
        CreateWalls(wallParent);

        int startRoomIndex = SpawnPlayer(listOfRooms);
    }

    private int SpawnPlayer(List<Node> listOfRooms)
    {
        //Zufälliger Startraum fuer Spieler
        int startRoomIndex = UnityEngine.Random.Range(0, listOfRooms.Count - 1);

        //Mittelpunkt als Spawnpoint setzen
        Node spawnRoom = listOfRooms[startRoomIndex];
        Debug.Log("Index Spawn Raum :" + startRoomIndex);
        Debug.Log("Spawn Room TopRightCorner:" + spawnRoom.TopRightAreaCorner);
        Debug.Log("Spawn Room BottomLeftCorner:" + spawnRoom.BottomLeftAreaCorner);
        
        Vector3Int middlePoint = new Vector3Int(
            (spawnRoom.TopRightAreaCorner.x + spawnRoom.BottomLeftAreaCorner.x) / 2,
            0,
            (spawnRoom.TopRightAreaCorner.y + spawnRoom.BottomLeftAreaCorner.y) / 2);

        Debug.Log("Berechneter Mittelpunkt: " + middlePoint);

        //Spielerprefab spawnen
        Instantiate(PlayerPrefab, middlePoint, Quaternion.identity, this.transform);

        //Startraum Index zurueckgeben
        return startRoomIndex;
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, new Vector3((float) (wallPosition.x + 0.5), (float)(wallPosition.y + 1), wallPosition.z), wallHorizontal);
        }
        foreach(var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, new Vector3(wallPosition.x, (float) (wallPosition.y + 1), (float) (wallPosition.z + 0.5)), wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3 wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.transform.parent = transform;

        for (int row =(int) bottomLeftV.x; row < (int) bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for(int row =(int) topLeftV.x; row < (int) topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col = (int) bottomLeftV.z; col < (int) topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }

    private void DestroyAllChildren()
    {
        while(transform.childCount != 0)
        {
            foreach(Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}
