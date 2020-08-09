using System;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("Stat Variables")]
    public Player player;
    public int currentYarn = 0, mainTowerHealth = 100, currentRound = 0;
    private const int maxMainTowerHealth = 100, hpMilkHeals = 25;
    public GameObject dropMilk, dropYarn;

    [Header("Enemy Variables")]
    public int enemyCount = 0;
    public int enemyCountMultipler = 5, enemyMaxCount = 0, enemyNumberSpawned = 0, enemyTotalKilledEver = 0;

    [Header("Other Variables")]
    public GameUi gameUi;
    [HideInInspector] public MainTower mainTower;
    [HideInInspector] public int pricePillow = 10, priceWater = 30, priceFridge = 50, priceMissile = 20;
    public bool gameIsOver = false;
    public GameObject scrapEnemy;
    public Transform projectilesInPlayParent;
    private int yarnMultiplier = 0;

    private void Start()
    {
        mainTower = FindObjectOfType<MainTower>();
        gameUi.ToggleTowerColourZones();
        StartNextRound();

        // Every 2 seconds, tell Astar to scan for new navigation data
        InvokeRepeating(nameof(RefreshAstar), 0f, 2f);
    }
    private void Update()
    {
        // If game is not over, perform per-frame key checks and StartNextRound check
        if (!gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isAlreadyPlacingObject)
            {
                AttemptTowerPlacement();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift))
            {
                gameUi.ToggleDemolishText();
            }
            if (enemyCount == 0)
            {
                enemyCount = -1;
                Invoke(nameof(StartNextRound), 2f);
            }
        }

    }

    private void RefreshAstar()
    {
        AstarPath.active.Scan();
    }

    private void StartNextRound()
    {
        // Iterate current round and grant yarn
        currentRound += 1;
        if (yarnMultiplier < 10)
        {
            yarnMultiplier += 1;
        }
        gameUi.UpdateYarn(20 * yarnMultiplier);

        // Manage enemy counts. Spawnrate increases each round.
        IncreaseEnemySpawnRate();
        enemyMaxCount = currentRound * enemyCountMultipler;
        enemyCount = enemyMaxCount;
        enemyNumberSpawned = 0;
        print($"Enemy count: {enemyCount}");

        // Update round indicator number & percentage marker
        gameUi.textRound.text = currentRound.ToString();
        gameUi.UpdateRoundIndicator();
    }

    public void HandleMilkPickup()
    {
        if (mainTowerHealth < maxMainTowerHealth)
        {
            int hpToHeal = maxMainTowerHealth - mainTowerHealth;
            if (hpToHeal > hpMilkHeals)
            {
                hpToHeal = hpMilkHeals;
            }
            gameUi.UpdateBoxCatHealth(hpToHeal);
            mainTower.ChangeHealthBar();
        }

        if (mainTowerHealth > 25)
        {
            gameUi.musicManager.ExitStressMode();
        }
    }

    public void GameIsOverPlayEndScene()
    {
        gameIsOver = true;
        GameOverResetPurchaseState();
        gameUi.musicManager.ChangeToGameOverMusic();
        gameUi.Invoke(nameof(gameUi.GameIsOverShowUi), 3f);
    }
}
