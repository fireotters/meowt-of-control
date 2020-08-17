using System;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("Stat Variables")]
    public Player player;
    public int currentYarn = 0, mainTowerHealth = 100, currentRound = 0;
    private const int maxMainTowerHealth = 100, hpTapeHeals = 25;

    [Header("Enemy Variables")]
    public int enemyCount = 0;
    public int enemyCountMultipler = 5, enemyMaxCount = 0, enemyNumberSpawned = 0, enemyTotalKilledEver = 0;

    [Header("Other Variables")]
    public GameUi gameUi;
    [HideInInspector] public MainTower mainTower;
    [HideInInspector] public int pricePillow = 10, priceWater = 30, priceFridge = 50, priceMissile = 20;
    public bool gameIsOver = false;
    private int yarnMultiplier = 1;

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
                Invoke(nameof(CelebrateEndOfRound), 1f);
            }
        }

    }

    private void RefreshAstar()
    {
        AstarPath.active.Scan();
    }

    private void CelebrateEndOfRound()
    {
        ResetPurchaseState();
        int totalTimeForTransition = 6;
        gameUi.RoundComplete(totalTimeForTransition);
        Invoke(nameof(StartNextRound), totalTimeForTransition);
    }
    
    private void StartNextRound()
    {
        // Reset player and game objects, unblock Build Panel
        player.ResetPosition();
        gameUi.UnblockBuildPanel();
        DestroyExistingObjects();

        // Update round number
        currentRound += 1;

        // Every third round, multiply the yarn given per round, until a x4 multiplier is reached.
        if (yarnMultiplier < 4 && currentRound % 3 == 0)
        {
            yarnMultiplier += 1;
        }
        gameUi.UpdateYarn(50 * yarnMultiplier);

        // Every second round, drop a Tape item
        if (currentRound % 2 == 0)
        {
            mainTower.DropItem(DroppedItem.PickupType.Tape);
        }

        // Manage enemy counts. Spawnrate increases each round.
        IncreaseEnemySpawnRate();
        enemyMaxCount = currentRound * enemyCountMultipler;
        enemyCount = enemyMaxCount;
        enemyNumberSpawned = 0;

        gameUi.textRound.text = currentRound.ToString();
        gameUi.UpdateRoundIndicator();
    }

    public void HandleTapePickup()
    {
        // If main tower can be healed, then heal Box for 25% or less, depending on how much there is to go til 100%
        if (mainTowerHealth < maxMainTowerHealth)
        {
            int hpToHeal = maxMainTowerHealth - mainTowerHealth;
            if (hpToHeal > hpTapeHeals)
            {
                hpToHeal = hpTapeHeals;
            }
            gameUi.UpdateBoxCatHealth(hpToHeal);
        }

        if (mainTowerHealth > 25)
        {
            gameUi.musicManager.ExitStressMode();
        }
    }

    public void GameIsOverPlayEndScene()
    {
        gameIsOver = true;
        ResetPurchaseState();
        gameUi.musicManager.ChangeToGameOverMusic();
        gameUi.Invoke(nameof(gameUi.GameIsOverShowUi), 3f);
    }

    private void DestroyExistingObjects()
    {
        Tower[] towersToDestroy = ObjectsInPlay.i.towersParent.GetComponentsInChildren<Tower>();
        foreach (Tower tower in towersToDestroy)
        {
            tower.EndOfRoundDestroyTurret();
        }
        foreach (Transform drop in ObjectsInPlay.i.dropsParent)
        {
            Destroy(drop.gameObject);
        }
        foreach (Transform aoe in ObjectsInPlay.i.projectilesParentExtras)
        {
            Destroy(aoe.gameObject);
        }
    }
}
