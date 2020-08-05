using Pathfinding;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool breaksThruScrap = false;
    public float enemyMaxHealth;
    public float enemyHealthRemaining;

    private AIPath _aiPath;
    private float storedMaxSpeed;

    private GameManager _gM;
    private Vector2 droppedItemOffset = new Vector2(0f, -0.5f);

    // Health bar
    private float healthBarFullSize;
    private Transform healthBar;

    // Status Effects
    public float waterDamageScale;
    private bool standingOnIce = false, standingOnWater = false, justHitByWater = false, currentlyStunnedByWater = false;

    private void Awake()
    {
        AstarPath.active.logPathResults = PathLog.None;    // Disable seeker logs to clean up log output.
    }

    private void Start()
    {
        _aiPath = GetComponent<AIPath>();
        storedMaxSpeed = _aiPath.maxSpeed;

        _gM = FindObjectOfType<GameManager>();
        enemyHealthRemaining = enemyMaxHealth;
        healthBar = transform.Find("HealthBar");
        healthBarFullSize = healthBar.localScale.x;
        GetComponent<AIDestinationSetter>().target = _gM.mainTower.transform;
    }

    private void Update()
    {
        CheckStatusEffects();
    }


    /// <summary>
    /// Status effects are Water and Ice.<br/>
    /// Water: When first hit by a balloon, stun for a short time. And for every frame standing in it, deduct some health.<br/>
    /// Ice: While standing in it, slow max movement to 20%.
    /// </summary>
    private void CheckStatusEffects()
    {
        if (!currentlyStunnedByWater)
        {
            // Ice speed reduction if not stunned by water
            _aiPath.maxSpeed = standingOnIce ? storedMaxSpeed * 0.2f : storedMaxSpeed;

            if (justHitByWater)
            {
                justHitByWater = false;
                currentlyStunnedByWater = true;
                _aiPath.maxSpeed = 0f;
                Invoke(nameof(ResetWaterStun), 0.5f);
            }
        }
        if (standingOnWater)
        {
            enemyHealthRemaining -= Time.deltaTime * waterDamageScale;
            ChangeHealthBar();
        }
    }

    private void ResetWaterStun()
    {
        currentlyStunnedByWater = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Being hit by projectile reduces HP by one
        if (col.gameObject.CompareTag("PlayerBullet"))
        {
            enemyHealthRemaining--;
            ChangeHealthBar();
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

        if (enemyHealthRemaining <= 0)
        {
            _gM.IncrementEnemyKillCount();
            DropScrapAndItems(transform.position);
            Destroy(gameObject);
        }
    }

    private void DropScrapAndItems(Vector2 enemyLastPos)
    {
        GameObject scrapDrop = Instantiate(_gM.scrapEnemy, _gM.gameUi.dropsInPlayParent);
        scrapDrop.transform.position = enemyLastPos;

        int randCheck = UnityEngine.Random.Range(0, 30);
        // 1/30 of the time, yarn will drop
        if (randCheck == 0)
        {
            GameObject yarnDrop = Instantiate(_gM.dropYarn, _gM.gameUi.dropsInPlayParent);
            yarnDrop.transform.position = enemyLastPos + droppedItemOffset;
        }
        // 3/30 of the time, milk will drop
        else if (randCheck < 3)
        {
            GameObject milkDrop = Instantiate(_gM.dropMilk, _gM.gameUi.dropsInPlayParent);
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
        if (standingOnWater)
        {
            justHitByWater = true;
        }
    }
}