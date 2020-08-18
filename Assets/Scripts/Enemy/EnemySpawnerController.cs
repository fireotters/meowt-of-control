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
            // Iterate spawn rate and GameManager's spawned enemy count
            nextSpawn = Time.time + spawnRate;

            // Choose one of four directions for the new enemy to come from
            int randSpawner = Random.Range(0, 4);
            _chosenSpawner = _enemySpawners[randSpawner];

            // Determine which enemy to spawn
            int randEnemy = Random.Range(0, 30);
            if (randEnemy == 0 && _gM.currentRound >= 4) // Spawn Big Chungus (1/30 chance)
            {
                if (bigChungusSpawned < bigChungusCap)
                {
                    _chosenSpawner.SpawnEnemy(GameAssets.i.enemyBigChungus);
                    _gM.enemyNumberSpawned++;
                }
            }
            else if (randEnemy < 5 && _gM.currentRound >= 2) // Spawn Sanic (4/30 chance)
            {
                _chosenSpawner.SpawnEnemy(GameAssets.i.enemySanic);
                _gM.enemyNumberSpawned++;
            }
            else if (randEnemy < 27) // Spawn Basic (22/30 chance)
            {
                _chosenSpawner.SpawnEnemy(GameAssets.i.enemyBasic);
                _gM.enemyNumberSpawned++;
            }

            // Nothing spawns at all (3/30 chance)
        }
    }

    public void SetBigChungusCap()
    {
        bigChungusSpawned = 0;
        bigChungusCap = _gM.currentRound / 2 - 1;
        //Debug.Log(bigChungusCap);
    }
}
