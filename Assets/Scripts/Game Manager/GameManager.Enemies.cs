using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("EnemySpawn Variables")]
    public EnemySpawner[] spawners;


    /// <summary>
    /// Enemy spawn rate reduced by 10% of previous rate each round. When it reaches one attempt every 2 seconds, lock the spawn rate.
    /// </summary>
    private void IncreaseEnemySpawnRate()
    {
        foreach (EnemySpawner spawner in spawners)
        {
            if (spawner.spawnRate > 2)
            {
                spawner.spawnRate *= 0.9f;
            }
            if (spawner.spawnRate < 2)
            {
                spawner.spawnRate = 2;
            }
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
