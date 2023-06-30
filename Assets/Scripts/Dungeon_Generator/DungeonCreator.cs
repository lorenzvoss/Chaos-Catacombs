using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DungeonCreator: MonoBehaviour
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

    public List<GameObject> roomPlanes;

    public GameObject wallVertical, wallHorizontal;
    public GameObject EnemyParent;

    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;

    public GameObject PlayerPrefab;
    public GameObject Enemy1Prefab;
    public GameObject Enemy2Prefab;
    public GameObject Enemy3Prefab;

    private GameObject player;

    /// <summary>
    /// Erstellt Dungeon inklusive Boden, Waenden, Spieler, (work in progress)Gegnern
    /// </summary>
    public void CreateDungeon(int round)
    {
        DestroyAllChildren();
        roomPlanes.Clear();

        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateDungeonRooms(maxIterations, roomWidthMin, roomLengthMin,
            RoomBottomCornerModifier, RoomTopCornerModifier, RoomOffset);
        var listOfCorridors = generator.CalculateDungeonCorridors(corridorWidht);

        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;

        GameObject corridorParent = new GameObject("DungeonCorridors");
        GameObject roomsParent = new GameObject("RoomParent");
        roomsParent.transform.parent = transform;

        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();

        //GameObjects fuer Raeume erstellen
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner, roomsParent);
        }
        //GameObjects fuer Gaenge erstellen
        for (int i = 0; i < listOfCorridors.Count; i++)
        {
            CreateMesh(listOfCorridors[i].BottomLeftAreaCorner, listOfCorridors[i].TopRightAreaCorner, corridorParent);
        }


        CreateWalls(wallParent);

        NavMeshSurface nav = GetComponent<NavMeshSurface>();
        nav.BuildNavMesh();

        int startRoomIndex = SpawnPlayer(listOfRooms);
        //Gegner hier spawnen in allen Räumen außer startRoomIndex

        listOfRooms.RemoveAt(startRoomIndex); //Spieler raum entfernen
        roomPlanes.RemoveAt(startRoomIndex);
        SpawnAllEnemies(listOfRooms, round);
    }

    private int SpawnPlayer(List<Node> listOfRooms)
    {
        //Zuf�lliger Startraum fuer Spieler
        int startRoomIndex = UnityEngine.Random.Range(0, listOfRooms.Count);

        Vector3 middlePoint = computeMiddlePoint(listOfRooms[startRoomIndex]);

        //Spielerprefab spawnen
        player = Instantiate(PlayerPrefab, middlePoint, Quaternion.identity, this.transform);
        player.AddComponent<PlayerHealth>();
        
        //Startraum Index zurueckgeben
        return startRoomIndex;
    }

    private Vector3 computeMiddlePoint(Node room)
    {
        Vector3Int middlePoint = new Vector3Int(
            (room.TopRightAreaCorner.x + room.BottomLeftAreaCorner.x) / 2,
            0,
            (room.TopRightAreaCorner.y + room.BottomLeftAreaCorner.y) / 2);

        Debug.Log("Berechneter Mittelpunkt: " + middlePoint);
        return middlePoint;
    }

    private void SpawnAllEnemies(List<Node> enemyRooms, int round)
    {
        for(int i = 0; i < enemyRooms.Count; i++)
        {
            int enemyType = UnityEngine.Random.Range(1, 4);
            switch(enemyType)
            {
                case 1:
                    SpawnEnemies(enemyRooms[i], Enemy1Prefab, 3 + round, i);
                    break;
                case 2:
                    SpawnEnemies(enemyRooms[i], Enemy2Prefab, 2 + round, i);
                    break;
                case 3:
                    SpawnEnemies(enemyRooms[i], Enemy3Prefab, 2 + round, i);
                    break;
            }
        }
    }

    private void SpawnEnemies(Node room, GameObject enemyPrefab, int count, int indexRoom)
    {
        Vector3 spawnPoint = computeMiddlePoint(room);
        for(int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity, this.transform);
            enemy.GetComponent<BehaviorExecutor>().SetBehaviorParam("wanderArea", roomPlanes[indexRoom]);
            enemy.GetComponent<BehaviorExecutor>().SetBehaviorParam("target", player);
            enemy.transform.parent = EnemyParent.transform;
        }
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, new Vector3((float)(wallPosition.x + 0.5), (float)(wallPosition.y + 1), wallPosition.z), wallHorizontal);
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, new Vector3(wallPosition.x, (float)(wallPosition.y + 1), (float)(wallPosition.z + 0.5)), wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3 wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner, GameObject parentObject)
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

        Vector3 center = Vector3.zero;
        foreach(Vector3 vertice in vertices){
            center += vertice;
        }
        center /= vertices.Length;
        GameObject centerOfFloor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        centerOfFloor.transform.position = center;
        centerOfFloor.transform.parent = dungeonFloor.transform;
        centerOfFloor.AddComponent<BoxCollider>();
        Bounds bounds = mesh.bounds;
        centerOfFloor.transform.localScale = bounds.size / 10;


        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.GetComponent<MeshRenderer>().enabled = false;
        dungeonFloor.transform.parent = parentObject.transform;
        dungeonFloor.AddComponent<BoxCollider>();

        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        
        if(parentObject.name == "RoomParent")
        {
            roomPlanes.Add(centerOfFloor);
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
        DestroyImmediate(GameObject.Find("DungeonCorridors"));
        DestroyImmediate(GameObject.Find("P_LPSP_UI_Canvas(Clone)"));

        while (transform.childCount != 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}
