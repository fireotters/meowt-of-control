﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("EnemySpawn Variables")]
    [SerializeField] private EnemySpawnerController _spawnController = default;


    /// <summary>
    /// Enemy spawn rate reduced by 10% of previous rate each round. When it reaches one attempt every 2 seconds, lock the spawn rate.
    /// </summary>
    private void IncreaseEnemySpawnRate()
    {
        if (_spawnController.spawnRate > 2)
        {
            _spawnController.spawnRate *= 0.9f;
        }
        if (_spawnController.spawnRate < 2)
        {
            _spawnController.spawnRate = 2;
        }
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
