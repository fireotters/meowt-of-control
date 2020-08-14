using Pathfinding;
using System;
using System.Net.Sockets;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool breaksThruObstacles = false;
    public float enemyMaxHealth;
    private float enemyHealthRemaining;

    private AIPath _aiPath;
    private float storedMaxSpeed;

    private GameManager _gM;

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
            float chipDamage = Time.deltaTime * waterDamageScale;
            DealDamage(chipDamage);
        }
    }

    private void ResetWaterStun()
    {
        currentlyStunnedByWater = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Being hit by projectile reduces HP. If it's water, the enemy is briefly stunned.
        if (col.gameObject.CompareTag("PlayerBullet"))
        {
            DealDamage(col.gameObject.GetComponent<Bullet>().damageToEnemy);
            if (col.gameObject.name.StartsWith("WaterProj"))
            {
                justHitByWater = true;
            }
        }
        // Big chungus destroys any scrap or towers he touches.
        else if (breaksThruObstacles)
        {
            if (col.gameObject.CompareTag("Scrap"))
            {
                Destroy(col.gameObject);
            }
            else if (col.gameObject.CompareTag("Tower"))
            {
                col.transform.GetComponent<Tower>().BigEnemyDestroysTower();
            }
        }
    }
    
    public void DealDamage(float damage)
    {
        enemyHealthRemaining -= damage;
        ChangeHealthBar();
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
            DropScrapAndItems();
            Destroy(gameObject);
        }
    }

    private void DropScrapAndItems()
    {
        Instantiate(GameAssets.i.pfScrap, transform.position, Quaternion.identity, ObjectsInPlay.i.dropsParent);

        int randCheck = UnityEngine.Random.Range(0, 30);
        // 2/30 of the time, yarn will drop
        if (randCheck < 2)
        {
            DroppedItem.Create(transform.position, DroppedItem.PickupType.Yarn);
        }
        // 2/30 of the time, milk will drop
        else if (randCheck < 4)
        {
            DroppedItem.Create(transform.position, DroppedItem.PickupType.Milk);
        }
        // 2/30 of the time, tape will drop
        else if (randCheck < 6)
        {
            DroppedItem.Create(transform.position, DroppedItem.PickupType.Tape);
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