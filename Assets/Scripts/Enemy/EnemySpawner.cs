using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private GameObject basicEnemy, sanic, bigChungus;
    public List<GameObject> mobs;
    private float randX, randY;
    
    private Vector2 spawnPlace;

    //Spawn rate of every mob
    public float basicSpawnRate = 1f;
    public float bigChungusSpawnRate = 5f;
    public float sanicSpawnRate = 10f;

    public GameObject enemy_target;

    private float nextSpawn = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Adding mobs
        mobs.Add(basicEnemy);
        mobs.Add(bigChungus);
        mobs.Add(sanic);
        foreach (var mob in mobs)
        {
            mob.GetComponent<AIDestinationSetter>().target = enemy_target.transform;
        }

        /*
         basicEnemy.GetComponent<AIDestinationSetter>().target = enemy_target.transform;
         bigChungus.GetComponent<AIDestinationSetter>().target = enemy_target.transform;
         sanic.GetComponent<AIDestinationSetter>().target = enemy_target.transform;
         */
    }

    // Update is called once per frame
    void Update()
    {
        //Basic mob spawn
        spawnEnemy();
    }

    private void spawnEnemy()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + basicSpawnRate;
            randX = Random.Range(-8.4f, 8.4f);
            randY = Random.Range(-3f, 3f);
            if (gameObject.tag.Equals("SideTower"))
            {
                spawnPlace = new Vector2(transform.position.x, randY);
            }
            else
            {
                spawnPlace = new Vector2(randX, transform.position.y);
            }


            Instantiate(mobs[0], spawnPlace, Quaternion.identity);

            /*Instantiate(bigChungus, spawnPlace, Quaternion.identity);
            Instantiate(sanic, spawnPlace, Quaternion.identity);
            */
        }

        //New Big chungus every 5 sec (Pending to discuss)
        if (Time.time > bigChungusSpawnRate)
        {
            bigChungusSpawnRate = Time.time + 5.0f;
            Instantiate(mobs[1], spawnPlace, Quaternion.identity);
        }

        // New Sanic every 10 sec (Pending to discuss)
        if (Time.time > sanicSpawnRate)
        {
            sanicSpawnRate = Time.time + 10.0f;
            Instantiate(mobs[2], spawnPlace, Quaternion.identity);
        }
    }
}