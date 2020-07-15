using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public SoundManager soundManager;
    public PlayerController player;
    public int currentCash = 50, currentHealth = 100, currentPlayerHealth = 3, currentRound = 0;
    private const int maxHealth = 100, maxPlayerHealth = 3, hpMilkHeals = 25;
    public GameUi gameUi;
    private float nextPlayerDmg = 0.0f;

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
        currentCash += 50 * currentRound;
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

    public void DamagePlayer()
    {
        if (currentPlayerHealth != 0 && Time.time > nextPlayerDmg)
        {
            nextPlayerDmg = Time.time + 3f;
            soundManager.SoundPlayerHit();
            currentPlayerHealth--;
            gameUi.UpdatePlayerHealth();
        }
    }

    public void PickupItem(string type)
    {
        if (type == "milk")
        {
            if (currentPlayerHealth < maxPlayerHealth)
            {
                currentPlayerHealth = maxPlayerHealth;
                gameUi.UpdatePlayerHealth();
            }
            else if (currentPlayerHealth == maxPlayerHealth)
            {
                if (currentHealth <= maxHealth - hpMilkHeals)
                {
                    gameUi.UpdateHealth(hpMilkHeals);
                }
                else
                {
                    gameUi.UpdateCash(50);
                }

                if (currentHealth > 25)
                {
                    gameUi.musicManager.ExitStressMode();
                }
            }
        }
        else if (type == "yarn")
        {
            gameUi.UpdateCash(50);
        }
        else
        {
            Debug.LogError("GameManger.PickupItem: Invalid pickup type");
        }
    }

}
