using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("EnemySpawn Variables")]
    [SerializeField] private EnemySpawnerController _spawnController = default;


    /// <summary>
    /// Enemy spawn rate reduced by 0.5sec each round. When it reaches one attempt every 2sec, lock the spawn rate.
    /// </summary>
    private void IncreaseEnemySpawnRate()
    {
        if (_spawnController.spawnRate > 2)
        {
            _spawnController.spawnRate -= 0.5f;
        }
        if (_spawnController.spawnRate < 2)
        {
            _spawnController.spawnRate = 2;
        }
        _spawnController.PerWaveChanges();
    }

    public void IncrementEnemyKillCount()
    {
        if (enemyCount > 0)
        {
            enemyCount--;
            enemyTotalKilledEver++;
        }
        gameUi.UpdateRoundIndicator();
    }
}
