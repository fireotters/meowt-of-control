using Pathfinding;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private enum EnemyType { Basic, Sanic, BigChungus }
    [SerializeField] EnemyType typeOfEnemy = default;
    public float waterStunTime = 0.5f;
    public float enemyMaxHealth;
    private float enemyHealthRemaining;

    private AIPath _aiPath;
    private float storedMaxSpeed;

    private GameManager _gM;
    private SpriteRenderer _sprRenderer;
    private CircleCollider2D _collider;
    [SerializeField] AudioSource audMetal = default, audRobotics = default;

    // Health bar
    private float healthBarFullSize;
    private Transform healthBar;

    // Status Effects
    public float waterDamageScale, iceSlowdownScale; // Water: higher = more damage taken, Ice: higher = less affected by ice
    private bool standingOnIce = false, standingOnWater = false, justHitByWater = false, currentlyStunnedByWater = false;

    private void Awake()
    {
        AstarPath.active.logPathResults = PathLog.None;    // Disable seeker logs to clean up log output.
    }

    private void Start()
    {
        _aiPath = GetComponent<AIPath>();
        storedMaxSpeed = _aiPath.maxSpeed;

        _gM = ObjectsInPlay.i.gameManager;
        enemyHealthRemaining = enemyMaxHealth;
        healthBar = transform.Find("HealthBar");
        healthBarFullSize = healthBar.localScale.x;
        GetComponent<AIDestinationSetter>().target = _gM.mainTower.transform;
        _collider = GetComponent<CircleCollider2D>();
        _sprRenderer = GetComponent<SpriteRenderer>();
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
            _aiPath.maxSpeed = standingOnIce ? storedMaxSpeed * iceSlowdownScale : storedMaxSpeed;

            if (justHitByWater)
            {
                justHitByWater = false;
                currentlyStunnedByWater = true;
                _aiPath.maxSpeed = 0f;
                Invoke(nameof(ResetWaterStun), waterStunTime);
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
        // Being hit by projectile reduces HP.
        // If it's water, the enemy is briefly stunned.
        if (col.gameObject.CompareTag("PlayerBullet"))
        {
            DealDamage(col.gameObject.GetComponent<Bullet>().damageToEnemy);
            if (col.gameObject.name.StartsWith("WaterProj"))
            {
                justHitByWater = true;
            }
        }

        // Big Chungus destroys any scrap or towers he touches.
        if (typeOfEnemy == EnemyType.BigChungus)
        {
            if (col.gameObject.CompareTag("Scrap")) {
                Destroy(col.gameObject);
            }
            else if (col.gameObject.CompareTag("Tower")) {
                col.transform.GetComponent<Tower>().BigEnemyDestroysTower();
            }
        }
        // Basic enemies destroys scrap after touching it for a while (the scrap itself tracks this). Cannot destroy active towers.
        // Sanic walks through scrap. Does not destroy it.
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

        if (enemyHealthRemaining <= 0 && _collider.enabled)
        {
            _gM.IncrementEnemyKillCount();
            DropScrapAndItems();
            PretendEnemyIsGone();
        }
    }

    private void DropScrapAndItems()
    {
        // Drop scrap & roll for item drops
        GameObject droppedScrap = Instantiate(GameAssets.i.pfScrap, transform.position, Quaternion.identity, ObjectsInPlay.i.dropsParent);
        int randCheck = UnityEngine.Random.Range(0, 30);

        // Big Chungus drops larger scrap pieces, and guarantees an item drop
        if (typeOfEnemy == EnemyType.BigChungus) {
            droppedScrap.GetComponent<EnemyScrap>().ScrapIsBigChungus();
            randCheck = UnityEngine.Random.Range(0, 4);
        }

        // Yarn drop. Normal: 2/30, Chungus: 2/4
        if (randCheck < 2)
        {
            DroppedItem.Create(transform.position, DroppedItem.PickupType.Yarn);
        }
        // Milk drop. Normal: 1/30, Chungus: 1/4
        else if (randCheck < 3)
        {
            DroppedItem.Create(transform.position, DroppedItem.PickupType.Milk);
        }
        // Tape drop. Normal: 1/30, Chungus: 1/4
        else if (randCheck < 4)
        {
            DroppedItem.Create(transform.position, DroppedItem.PickupType.Tape);
        }
    }

    public bool GetFreezeStatus()
    {
       return standingOnIce;
    }
    public void SetFreezeStatus(bool iceStatus)
    {
        standingOnIce = iceStatus;
    }

    public bool GetWaterStatus()
    {
        return standingOnWater;
    }
    public void SetWaterStatus(bool waterStatus)
    {
        standingOnWater = waterStatus;
    }

    private void PretendEnemyIsGone()
    {
        _collider.enabled = false;
        _sprRenderer.enabled = false;
        healthBar.gameObject.SetActive(false);

        audMetal.Play();
        audRobotics.Play();

        Invoke(nameof(ActuallyDestroy), 1f);
    }

    private void ActuallyDestroy()
    {
        Destroy(gameObject);
    }
}