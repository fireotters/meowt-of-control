using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEditorInternal;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("GameManager")]
    public PlayerController player;
    public int currentCash = 10000, currentHealth = 100, currentRound = 0;
    public GameUi gameUi;
    private MusicManager musicManager;
    public GameObject musicManagerIfNotFoundInScene;

    [Header("Enemy Management")]
    public int enemyCount = 0;
    public int enemyMaxCount = 0;

    private void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
        if (!musicManager)
        {
            Instantiate(musicManagerIfNotFoundInScene);
            musicManager = FindObjectOfType<MusicManager>();
        }
        if (musicManager)
        {
            musicManager.ChangeMusicTrack(1);
        }

        sprRedArea = placementBlockersParent.Find("RedArea").GetComponent<SpriteRenderer>();
        sprGreenArea = placementBlockersParent.Find("ValidPlacements").Find("GreenArea").GetComponent<SpriteRenderer>();
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

}
