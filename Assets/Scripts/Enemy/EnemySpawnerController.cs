using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    private float nextSpawn = 0.0f;
    private GameManager _gM;
    public float spawnRate;
    [SerializeField] private EnemySpawner[] _enemySpawners = default;
    private EnemySpawner _chosenSpawner;
    private int bigChungusSpawned = 0, bigChungusCap;
    private int basicChance, sanicChance, bigChungusChance;

    private void Start()
    {
        _gM = ObjectsInPlay.i.gameManager;
    }

    private void Update()
    {
        AttemptSpawn();
    }

    private void AttemptSpawn()
    {
        if (Time.time > nextSpawn && _gM.enemyNumberSpawned < _gM.enemyMaxCount)
        {
            // Iterate spawn rate
            nextSpawn = Time.time + spawnRate;

            // Basic Enemy by default, and choose where to spawn it
            int randEnemy = Random.Range(1, 101);
            Enemy _chosenEnemy = GameAssets.i.enemyBasic;
            int randSpawner = Random.Range(0, 4);

            // Choose Big Chungus instead
            if (randEnemy <= bigChungusChance)
            {
                if (bigChungusSpawned < bigChungusCap)
                {
                    bigChungusSpawned++;
                    nextSpawn += 8f; // Big Chungus spawn delays further spawns
                    _chosenEnemy = GameAssets.i.enemyBigChungus;
                    randSpawner = Random.Range(3, 4); // Ensure Big Chungus only spawns on left/right side (to give players time to finish him)
                }
            }
            // Choose Sanic instead
            else if (randEnemy <= sanicChance + bigChungusChance)
            {
                _chosenEnemy = GameAssets.i.enemySanic;
            }

            // Spawn the enemy
            _chosenSpawner = _enemySpawners[randSpawner];
            _chosenSpawner.SpawnEnemy(_chosenEnemy);
            _gM.enemyNumberSpawned++;
        }
    }

    public void PerWaveChanges()
    {
        bigChungusSpawned = 0;
        bigChungusCap = _gM.currentRound / 2 - 1;


        // Big Chungus Chance (Day 1-3: 0% |    | Day 4+: 5%)
        bigChungusChance = _gM.currentRound >= 4 ? 5 : 0;

        /* Sanic Chance (Day 1: 0% |    | Day 2-9: 10% + 3% * DayNum (Day 2-3 adding Big Chungus' 5%) |    | Day 10+: 40%) */
        if (_gM.currentRound >= 2)
        {
            sanicChance = 10 + _gM.currentRound * 3;
            sanicChance += 5 - bigChungusChance;
            if (sanicChance > 40) sanicChance = 40;
        }
        else { sanicChance = 0; }

        /* Basic Chance (Day 1: 100% |    | Day 2-9: Remainder of other two. 79% to 58% |    | Day 10+: 55%) */
        basicChance = 100 - bigChungusChance - sanicChance;

        string bigChungusString = $"Big Chungus: {bigChungusChance}%";
        string sanicString = $"Sanic: {sanicChance}%";
        string basicString = $"Basic: {basicChance}%";
        Debug.Log($"Spawn chances this wave: {bigChungusString} ||| {sanicString} ||| {basicString}");
    }
}
