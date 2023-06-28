using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    DungeonCreator dungeonCreator;

    public int RoundsTillBoss = 4;

    private int round = 0;
    // Start is called before the first frame update
    void Start()
    {
        dungeonCreator = GetComponent<DungeonCreator>();
        dungeonCreator.CreateDungeon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
