﻿using Pathfinding;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool ignoreScrapColliders = false, breaksThruScrap = false;
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
                DropScrapAndItems(enemyLastPos);
                Destroy(gameObject);
            }
        }
        // Big chungus destroys any scrap he touches
        else if (ignoreScrapColliders && col.gameObject.CompareTag("Scrap"))
        {
            Destroy(col.gameObject);
        }
    }

    private void ChangeHealthBar()
    {
        Vector3 scaleChange = healthBar.localScale;
        float percentOfLifeLeft = (float)enemyHitsRemaining / (float)enemyMaxHits;
        scaleChange.x = percentOfLifeLeft * healthBarFullSize;
        healthBar.localScale = scaleChange;
    }

    private void DropScrapAndItems(Vector2 enemyLastPos)
    {
        GameObject scrapDrop = Instantiate(scrap, gM.gameUi.dropsInPlayParent);
        scrapDrop.transform.position = enemyLastPos;

        int randCheck = Random.Range(0, 30);
        // 1/30 of the time, yarn will drop
        if (randCheck == 0)
        {
            GameObject yarnDrop = Instantiate(gM.dropYarn, gM.gameUi.dropsInPlayParent);
            yarnDrop.transform.position = enemyLastPos;
        }
        // 3/30 of the time, milk will drop
        else if (randCheck < 3)
        {
            GameObject milkDrop = Instantiate(gM.dropMilk, gM.gameUi.dropsInPlayParent);
            milkDrop.transform.position = enemyLastPos;
        }
    }
}