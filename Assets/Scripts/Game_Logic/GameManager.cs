using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    DungeonCreator dungeonCreator;

    public GameObject EnemyParent;
    public int RoundsTillBoss = 4;

    private int round = 0;
    private bool dungeonIsInitialized = false;
    // Start is called before the first frame update
    void Start()
    {
        dungeonCreator = GetComponent<DungeonCreator>();
        StartNewRound();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyParent.transform.childCount == 0 && dungeonIsInitialized)
        {
            StartNewRound(); 
        }
    }

    void StartNewRound()
    {
        if (round > RoundsTillBoss)
        {
            //Max Rundenzahl erreicht also Bosskampf starten
            StartBossFight();
        }
        else
        {
            //Es muessen noch Runden gespielt werden
            round++;
            dungeonCreator.CreateDungeon(round);
            dungeonIsInitialized = true;
        }
    }

    void StartBossFight()
    {
        SceneManager.LoadScene("BossFightScene");
    }
}
