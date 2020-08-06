using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private float randX, randY;
    public bool isHorizontalSpawner;

    private Vector2 spawnPlace;
    [SerializeField] private EnemySpawnerController _spawnController = default;

    public void PrepToSpawnEnemy(Enemy chosenEnemy)
    {
        ChooseSpawnLocation();
        SpawnEnemy(chosenEnemy);
    }

    private void ChooseSpawnLocation()
    {
        if (isHorizontalSpawner)
        {
            randX = Random.Range(-8.4f, 8.4f);
            spawnPlace = new Vector2(randX, transform.position.y);
        }
        else
        {
            randY = Random.Range(-3f, 3f);
            spawnPlace = new Vector2(transform.position.x, randY);
        }
    }

    private void SpawnEnemy(Enemy chosenEnemy)
    {
        Instantiate(chosenEnemy, spawnPlace, Quaternion.identity, _spawnController.enemiesInPlayParent);
    }

}