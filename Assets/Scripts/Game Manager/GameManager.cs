using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("Stat Variables")]
    public Player player;
    public int currentYarn = 50, mainTowerHealth = 100, currentRound = 0;
    private const int maxMainTowerHealth = 100, hpMilkHeals = 25;
    public GameObject dropMilk, dropYarn;

    [Header("Enemy Variables")]
    public int enemyCount = 0;
    public int enemyMaxCount = 0, enemyNumberSpawned = 0;

    [Header("Other Variables")]
    public GameUi gameUi;
    private MainTower _mainTower;
    [HideInInspector] public int pricePillow = 10, priceWater = 30, priceFridge = 50, priceMissile = 20;

    private void Start()
    {
        _mainTower = FindObjectOfType<MainTower>();
        gameUi.ToggleTowerColourZones();
        StartNextRound();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPlacingTower)
        {
            AttemptTowerPlacement();
        }
        if (enemyCount == 0) // TODO When enemy dies, tell gamemanager to remove one from enemyCount
        {
            enemyCount = -1;
            Invoke(nameof(StartNextRound), 2f);
        }
    }

    private void StartNextRound()
    {
        currentRound += 1;
        currentYarn += 50 * currentRound;
        enemyMaxCount = currentRound * 7;
        enemyCount = enemyMaxCount;
        enemyNumberSpawned = 0;
        gameUi.textRound.text = currentRound.ToString();
        print($"Enemy count: {enemyCount}");
        gameUi.UpdateRoundIndicator();
    }

    public void IncrementEnemyKillCount()
    {
        if (enemyCount > 0)
        {
            enemyCount -= 1;
        }
        gameUi.UpdateRoundIndicator();
    }

    public void HandleMilkPickup()
    {
        if (mainTowerHealth <= maxMainTowerHealth - hpMilkHeals)
        {
            gameUi.UpdateMainTowerHealth(hpMilkHeals);
        }
        else
        {
            gameUi.UpdateYarn(50);
        }

        if (mainTowerHealth > 25)
        {
            gameUi.musicManager.ExitStressMode();
        }
    }

}
