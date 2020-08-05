using Pathfinding;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool breaksThruScrap = false;
    public float enemyMaxHealth;
    internal float enemyHealthRemaining;

    private GameManager gM;
    private Vector2 droppedItemOffset = new Vector2(0f, -0.5f);

    // Health bar
    private float healthBarFullSize;
    private Transform healthBar;

    // Status Effects
    private bool standingOnIce = false, standingOnWater = false;

    private void Awake()
    {
        AstarPath.active.logPathResults = PathLog.None;    // Disable seeker logs to clean up log output.
    }

    private void Start()
    {
        gM = FindObjectOfType<GameManager>();
        enemyHealthRemaining = enemyMaxHealth;
        healthBar = transform.Find("HealthBar");
        healthBarFullSize = healthBar.localScale.x;
        GetComponent<AIDestinationSetter>().target = gM.mainTower.transform;
    }

    private void Update()
    {
        CheckStatusEffects();
    }

    private void CheckStatusEffects()
    {
        if (standingOnIce)
        {

        }
        if (standingOnWater)
        {

        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Being hit by projectile reduces HP by one
        if (col.gameObject.CompareTag("PlayerBullet"))
        {
            enemyHealthRemaining--;
            ChangeHealthBar();

            if (enemyHealthRemaining == 0)
            {
                gM.IncrementEnemyKillCount();
                Vector2 enemyLastPos = new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.y);
                DropScrapAndItems(enemyLastPos);
                Destroy(gameObject);
            }
        }
        // Big chungus destroys any scrap he touches.
        else if (breaksThruScrap && col.gameObject.CompareTag("Scrap"))
        {
            Destroy(col.gameObject);
        }
    }

    private void ChangeHealthBar()
    {
        Vector3 scaleChange = healthBar.localScale;
        float percentOfLifeLeft = enemyHealthRemaining / enemyMaxHealth;
        scaleChange.x = percentOfLifeLeft * healthBarFullSize;
        healthBar.localScale = scaleChange;
    }

    private void DropScrapAndItems(Vector2 enemyLastPos)
    {
        GameObject scrapDrop = Instantiate(gM.scrapEnemy, gM.gameUi.dropsInPlayParent);
        scrapDrop.transform.position = enemyLastPos;

        int randCheck = UnityEngine.Random.Range(0, 30);
        // 1/30 of the time, yarn will drop
        if (randCheck == 0)
        {
            GameObject yarnDrop = Instantiate(gM.dropYarn, gM.gameUi.dropsInPlayParent);
            yarnDrop.transform.position = enemyLastPos + droppedItemOffset;
        }
        // 3/30 of the time, milk will drop
        else if (randCheck < 3)
        {
            GameObject milkDrop = Instantiate(gM.dropMilk, gM.gameUi.dropsInPlayParent);
            milkDrop.transform.position = enemyLastPos + droppedItemOffset;
        }
    }

    public void SetFreezeStatus(bool iceStatus)
    {
        standingOnIce = iceStatus;
    }

    public void SetWaterStatus(bool waterStatus)
    {
        standingOnWater = waterStatus;
    }
}