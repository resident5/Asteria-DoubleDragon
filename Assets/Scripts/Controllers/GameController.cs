using System;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public AmbushTracker ambushTracker;

    public int comboCounter = 0;
    public float comboDelay = 1.2f;
    public float startTime;

    public enum GameState
    {
        AMBUSHED,
        NEUTRAL,
    }

    public GameState gameState;

    public float spawnOffset = 2f;
    public GameObject enemyPrefab;
    public List<GameObject> enemyList;
    public int enemyCounter = 0;

    public int enemyWave;
    public bool isAmbushed = false;

    public float spawnDelay = 0.2f;
    public int enemyLimit = 3;


    public int numberOfKilledEnemies = 0;
    public int maxEnemyCounter = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this.gameObject);
        }

        ambushTracker = GetComponent<AmbushTracker>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.NEUTRAL;
        enemyList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (comboCounter > 0)
        {
            if (Time.time - startTime >= comboDelay)
            {
                comboCounter = 0;
            }
        }

        if( numberOfKilledEnemies < maxEnemyCounter && gameState == GameState.AMBUSHED)
        {
            if (enemyCounter < enemyLimit)
            {
                SpawnEnemyNearPlayer();
            }
        }
    }

    public void ComboCount()
    {
        comboCounter += 1;
        startTime = Time.time;
    }

    public void SpawnEnemyNearPlayer()
    {
        var playerPosition = CharacterMovement.Instance.transform.position;
        int sideRandomizer = Random.Range(0, 2);

        var randomPointNextToPlayer = new Vector2(playerPosition.x + (sideRandomizer == 0 ? spawnOffset : -spawnOffset),
            playerPosition.y);

        GameObject spawnedEnemy = Instantiate(enemyPrefab, randomPointNextToPlayer, quaternion.identity);
        // enemyWave++;
        enemyCounter++;
    }

    public void BeginAmbush()
    {
        gameState = GameState.AMBUSHED;
        numberOfKilledEnemies = 0;
        enemyCounter = 0;
    }

    public void EnemyDeathCounterIncrease()
    {
        if (gameState == GameState.AMBUSHED)
        {
            numberOfKilledEnemies++;
            Debug.Log("Increase by 1: " + numberOfKilledEnemies + " " + gameObject.name);
        }

        if (numberOfKilledEnemies >= maxEnemyCounter)
        {
            gameState = GameState.NEUTRAL;
            ambushTracker.EndEnemyAmbush();
        }
    }
}