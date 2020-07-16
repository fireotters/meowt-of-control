using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public Player player;
    public int currentYarn = 50, mainTowerHealth = 100, currentRound = 0;
    private const int maxMainTowerHealth = 100, hpMilkHeals = 25;
    public GameUi gameUi;

    [Header("Enemy Management")]
    public int enemyCount = 0;
    public int enemyMaxCount = 0;
    public int enemyNumberSpawned = 0;
    public GameObject dropMilk, dropYarn;

    private void Start()
    {
        sprTowerInvalidArea = placementBlockersParent.Find("RedArea").GetComponent<SpriteRenderer>();
        sprTowerRange = placementBlockersParent.Find("TowerRangeArea").GetComponent<SpriteRenderer>();
        ToggleTowerColourZones();
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
