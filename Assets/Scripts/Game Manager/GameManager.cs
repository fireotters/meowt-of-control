using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEditorInternal;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public SoundManager soundManager;
    public MusicManager musicManager;
    public PlayerController player;
    public int currentCash = 50, currentHealth = 100, currentPlayerHealth = 3, currentRound = 0;
    private int maxHealth = 100, maxPlayerHealth = 3;
    public GameUi gameUi;

    [Header("Enemy Management")]
    public int enemyCount = 0;
    public int enemyMaxCount = 0;

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
            RoundEndCelebrate();
            Invoke(nameof(StartNextRound), 2f);
        }
    }

    private void RoundEndCelebrate()
    {
        debugLevelTransitionSound.Play();
    }

    private void StartNextRound()
    {
        currentRound += 1;
        currentCash += 50 * currentRound;
        enemyMaxCount = currentRound * 7;
        enemyCount = enemyMaxCount;
        gameUi.textRound.text = currentRound.ToString();
        print($"Enemy count: {enemyCount}");
        gameUi.UpdateRoundIndicator();
    }

    public AudioSource debugLevelTransitionSound;
    public void KillEnemy()
    {
        gameUi.UpdateRoundIndicator();
        if (enemyCount > 0)
        {
            enemyCount -= 1;
        }
        print($"Enemy count: {enemyCount}");
    }
    public void DamagePlayer()
    {
        if (currentPlayerHealth != 0)
        {
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
                if (currentHealth < maxHealth)
                {
                    gameUi.UpdateHealth(25);
                }
                else
                {
                    gameUi.UpdateCash(50);
                }

                if (currentHealth > 25)
                {
                    musicManager.ExitStressMode();
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
