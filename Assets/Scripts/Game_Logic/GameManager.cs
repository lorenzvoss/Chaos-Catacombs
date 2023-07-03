using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    DungeonCreator dungeonCreator;

    public GameObject EnemyParent;
    public int RoundsTillBoss = 4;

    private int round = 0;
    private int enemiesMaxCount;
    private bool dungeonIsInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        dungeonCreator = GetComponent<DungeonCreator>();
        StartNewRound();
        enemiesMaxCount = EnemyParent.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyParent.transform.childCount == 0 && dungeonIsInitialized)
        {
            StartNewRound();
        }
        else
        {
            var canvas = GameObject.Find("EnemiesCount");
            TextMeshProUGUI roundText = canvas.GetComponent<TextMeshProUGUI>();
            roundText.text = $"{EnemyParent.transform.childCount}/{enemiesMaxCount}";
        }
    }

    void StartNewRound()
    {
        if (round >= RoundsTillBoss)
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

            //Anzahl Gegner am anfang der Runde aktualisieren
            enemiesMaxCount = EnemyParent.transform.childCount;
            
            //Set Round Count
            var canvas = GameObject.Find("RoundCount");
            TextMeshProUGUI roundText = canvas.GetComponent<TextMeshProUGUI>();
            roundText.text = $"{round}/{RoundsTillBoss}";
        }
    }

    void StartBossFight()
    {
        SceneManager.LoadScene("BossFightScene");
    }
}
