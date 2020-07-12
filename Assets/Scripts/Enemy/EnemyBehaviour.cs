using Pathfinding;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject corpse;
    public int bigChungusLife;
    public int basicLife;
    public int sanicLife;

    private Vector2 enemyLastPos;

    private void Awake()
    {
        AstarPath.active.logPathResults = PathLog.None;    // Disable seeker logs to clean up log output.
    }

    void Start()
    {
        bigChungusLife = 5;
        basicLife = 2;
        sanicLife = 1;
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        // Very redundant but actually works so idc I wanna sleep

        if (col.gameObject.tag == "Projectile" && gameObject.tag == "Enemy")
        {
            basicLife--;
        }

        if (col.gameObject.tag == "Projectile" && gameObject.tag == "BigChungusEnemy")
        {
            bigChungusLife--;
        }

        if (col.gameObject.tag == "Projectile" && gameObject.tag == "Sanic")
        {
            sanicLife--;
        }

        if (col.gameObject.tag == "Projectile" && basicLife == 0)
        {
            Vector2 enemyLastPos =
                new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.y);
            Destroy(gameObject);
            corpse.transform.position = enemyLastPos;
            Instantiate(corpse);
        }

        if (col.gameObject.tag == "Projectile" && bigChungusLife == 0)
        {
            Vector2 enemyLastPos =
                new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.y);
            Destroy(gameObject);
            corpse.transform.position = enemyLastPos;
            Instantiate(corpse);
        }

        if (col.gameObject.tag == "Projectile" && sanicLife == 0)
        {
            Vector2 enemyLastPos =
                new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.y);
            Destroy(gameObject);
            corpse.transform.position = enemyLastPos;
            Instantiate(corpse);
        }
    }
}