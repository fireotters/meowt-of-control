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
    public float spawnRate;

    public GameObject enemy_target;
    private GameManager gM;

    private float nextSpawn = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        gM = FindObjectOfType<GameManager>();
        //Adding mobs
        mobs.Add(basicEnemy);
        mobs.Add(bigChungus);
        mobs.Add(sanic);
        foreach (var mob in mobs)
        {

            if (mob != null)
            {
                mob.GetComponent<AIDestinationSetter>().target = enemy_target.transform;
            }
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
        if (Time.time > nextSpawn && gM.enemyNumberSpawned < gM.enemyMaxCount)
        {
            nextSpawn = Time.time + spawnRate;
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

            // Half the time, nothing will spawn.
            int randCheck = Random.Range(0, 20);
            if (randCheck == 0) // Spawn big chungus
            {
                Instantiate(mobs[1], spawnPlace, Quaternion.identity);
                gM.enemyNumberSpawned++;
            }
            else if (randCheck < 3) // Spawn Sanic
            {
                Instantiate(mobs[2], spawnPlace, Quaternion.identity);
                gM.enemyNumberSpawned++;
            }
            else if (randCheck < 10) // Spawn Common
            {
                Instantiate(mobs[0], spawnPlace, Quaternion.identity);
                gM.enemyNumberSpawned++;
            }
        }
    }
}