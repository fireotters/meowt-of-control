using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField] private Enemy enemyBasic = default, enemySanic = default, enemyBigChungus = default;
    private float nextSpawn = 0.0f;
    private GameManager _gM;
    public float spawnRate;
    [SerializeField] private EnemySpawner[] _enemySpawners = default;
    private EnemySpawner _chosenSpawner;

    private void Start()
    {
        _gM = FindObjectOfType<GameManager>();
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
            if (randEnemy == 0 && _gM.currentRound > 2) // Spawn Big Chungus (1/30 chance)
            {
                _chosenSpawner.SpawnEnemy(enemyBigChungus);
            }
            else if (randEnemy < 5 && _gM.currentRound > 1) // Spawn Sanic (4/30 chance)
            {
                _chosenSpawner.SpawnEnemy(enemySanic);
            }
            else if (randEnemy < 25) // Spawn Basic (20/30 chance)
            {
                _chosenSpawner.SpawnEnemy(enemyBasic);
            }

            if (randEnemy < 25)
            {
                _gM.enemyNumberSpawned++;
            }
            // Nothing spawns at all (5/30 chance)
        }
    }
}
