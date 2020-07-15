using Pathfinding;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool ignoreScrapColliders = false;
    public GameObject scrap;
    public int enemyMaxHits;
    internal int enemyHitsRemaining;

    private GameManager gM;

    // Health bar
    private float healthBarFullSize;
    private Transform healthBar;

    private void Awake()
    {
        AstarPath.active.logPathResults = PathLog.None;    // Disable seeker logs to clean up log output.
    }

    void Start()
    {
        healthBar = transform.Find("HealthBar");
        gM = FindObjectOfType<GameManager>();
        enemyHitsRemaining = enemyMaxHits;
        healthBarFullSize = healthBar.localScale.x;
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        // Being hit by projectile reduces HP by one
        if (col.gameObject.CompareTag("PlayerBullet"))
        {
            enemyHitsRemaining--;
            ChangeHealthBar();

            if (enemyHitsRemaining == 0)
            {
                gM.IncrementEnemyKillCount();
                Vector2 enemyLastPos = new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.y);
                Destroy(gameObject);
                scrap.transform.position = enemyLastPos;
                Instantiate(scrap, gM.gameUi.dropsInPlayParent);

                int randCheck = Random.Range(0, 10);
                // 3/10 of the time, one of two items will drop
                if (randCheck == 0) // Drop 50 yarn
                {
                    Instantiate(gM.dropYarn, gM.gameUi.dropsInPlayParent);
                    gM.enemyNumberSpawned++;
                }
                else if (randCheck < 3)
                {
                    Instantiate(gM.dropMilk, gM.gameUi.dropsInPlayParent);
                    gM.enemyNumberSpawned++;
                }
                // No item spawns 7/10 of the time
            }
        }
        // Hitting player damages them
        else if (col.gameObject.CompareTag("Player"))
        {
            gM.DamagePlayer();
        }
        // Big chungus ignores the collision with scraps
        else if (ignoreScrapColliders && col.gameObject.CompareTag("Scrap"))
        {
            Physics2D.IgnoreCollision(col.gameObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
            Debug.Log("BigChungus ignored scrap collision");
        }
    }

    private void ChangeHealthBar()
    {
        Vector3 scaleChange = healthBar.localScale;
        float percentOfLifeLeft = (float)enemyHitsRemaining / (float)enemyMaxHits;
        scaleChange.x = percentOfLifeLeft * healthBarFullSize;
        healthBar.localScale = scaleChange;
    }
}