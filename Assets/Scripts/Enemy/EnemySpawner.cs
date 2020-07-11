using Pathfinding;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;

    private float randX;

    private float randY;

    private Vector2 spawnPlace;

    public float spawnRate = 1f;

    public GameObject enemy_target;

    private float nextSpawn = 0.0f;
    // Start is called before the first frame update
    void Start()
    {

        enemy.GetComponent<AIDestinationSetter>().target = enemy_target.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn)
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
           
            Instantiate(enemy, spawnPlace, Quaternion.identity);
        }
    }
}
