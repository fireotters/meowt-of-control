using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField] private Enemy enemyBasic = default, enemySanic = default, enemyBigChungus = default;
    private float nextSpawn = 0.0f;
    private GameManager _gM;
    public float spawnRate;
    public Transform enemiesInPlayParent;
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
            _gM.enemyNumberSpawned++;

            // Choose one of four directions for the new enemy to come from
            int randSpawner = Random.Range(0, 4);
            _chosenSpawner = _enemySpawners[randSpawner];

            // Determine which enemy to spawn
            int randEnemy = Random.Range(0, 30);
            if (randEnemy == 0) // Spawn Big Chungus (1/30 chance)
            {
                _chosenSpawner.PrepToSpawnEnemy(enemyBigChungus);
            }
            else if (randEnemy < 5) // Spawn Sanic (4/30 chance)
            {
                _chosenSpawner.PrepToSpawnEnemy(enemySanic);
            }
            else if (randEnemy < 25) // Spawn Basic (20/30 chance)
            {
                _chosenSpawner.PrepToSpawnEnemy(enemyBasic);
            }
            // Nothing spawns at all (5/30 chance)
        }
    }
}
