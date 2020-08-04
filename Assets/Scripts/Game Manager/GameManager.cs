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
    public int enemyMaxCount = 0, enemyNumberSpawned = 0, enemyTotalKilledEver = 0;

    [Header("Other Variables")]
    public GameUi gameUi;
    [HideInInspector] public MainTower _mainTower;
    [HideInInspector] public int pricePillow = 10, priceWater = 30, priceFridge = 50, priceMissile = 20;
    public bool gameIsOver = false;
    public GameObject scrap;
    public TowerManager towerManager;

    private void Start()
    {
        _mainTower = FindObjectOfType<MainTower>();
        gameUi.ToggleTowerColourZones();
        StartNextRound();
    }
    private void Update()
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
        // Every 60 frames, tell Astar to scan for new navigation data
        if (Time.frameCount % 60 == 0)
        {
            AstarPath.active.Scan();
        }
    }

    private void StartNextRound()
    {
        // Iterate current round and grant yarn
        currentRound += 1;
        gameUi.UpdateYarn(20 * currentRound);

        // Manage enemy counts. Spawnrate increases each round. RoundNo*7 enemies per round.
        IncreaseEnemySpawnRate();
        enemyMaxCount = currentRound * 7;
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
            gameUi.UpdateMainTowerHealth(hpToHeal);
            _mainTower.ChangeHealthBar();
        }

        if (mainTowerHealth > 25)
        {
            gameUi.musicManager.ExitStressMode();
        }
    }

    public void GameIsOverPlayEndScene()
    {
        gameIsOver = true;
        gameUi.musicManager.ChangeToGameOverMusic();
        gameUi.Invoke(nameof(gameUi.GameIsOverShowUi), 3f);
    }
}
