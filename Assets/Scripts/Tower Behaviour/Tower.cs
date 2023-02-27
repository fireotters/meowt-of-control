using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class Tower : MonoBehaviour
{
    [Header("Animation, etc")]
    internal Animator _towerAnimator = default;
    private static readonly int Direction = Animator.StringToHash("Direction");
    internal GameManager _gM;

    [Header("Attached Objects")]
    [SerializeField] internal Bullet attachedBulletType = default;
    [SerializeField] private GameObject _attachedScrap = default;
    [HideInInspector] public GameObject attachedPlacementBlocker;

    [Header("Overheat (Max is 10, scale penalties accordingly)")]
    [SerializeField] private float penaltyPerShot = default;
    [SerializeField] private RecoverSpeed overheatRecoverSpeedName = default;
    private readonly float maxOverheat = 10;
    private float currentOverheat = 0f, overheatRecoverRate;
    private bool towerGone = false;
    private enum RecoverSpeed { Slow, Mid, Fast }
    private readonly float[] recoverSpeedValues = { .5f, 1.5f, 3f };

    [Header("Timer UI")]
    [SerializeField] private TowerTimerUi attachedTimerUi;

    [Header("Shooting")]
    [SerializeField] internal float shootCadence = 0.5f;
    private float _canShootAgain;
    private bool _canShoot;
    internal Transform bulletEmitter, gunEnd;

    [Header("Targetting")]
    public float rangeOfTower;
    private CircleCollider2D rangeCollider;
    internal Transform enemyToTarget;
    internal List<Enemy> AcknowledgedEnemies = new List<Enemy>();

    [HideInInspector] public static Vector3 spritePivotOffset = new Vector3(0, 0.5f, 0);
    public static Tower Create(Vector3 position, Tower typeToSpawn)
    {
        Transform towerTransform = Instantiate(typeToSpawn, position, Quaternion.identity, ObjectsInPlay.i.towersParent).transform;
        towerTransform.position += spritePivotOffset;

        Tower tower = towerTransform.GetComponent<Tower>();

        // Attach a placement barrier and timer UI, so when the tower is destroyed, so are they.
        GameObject newBarrier = Instantiate(GameAssets.i.pfCircleBarrier, towerTransform.position, Quaternion.identity, ObjectsInPlay.i.placementBlockersParent);
        tower.attachedPlacementBlocker = newBarrier;
        tower.attachedTimerUi = TowerTimerUi.Create(towerTransform.position);
        return tower;
    }

    private void Start()
    {
        FindComponents();
        _canShootAgain = shootCadence;
        rangeCollider.radius *= rangeOfTower;

        // Determine multiplier for recovery speed
        overheatRecoverRate = recoverSpeedValues[(int)overheatRecoverSpeedName];
    }

    /// If an enemy walks into range, add it to the list of acknowledged enemies.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy") && !other.CompareTag("LargeEnemy")) return;

        AcknowledgedEnemies.Add(other.GetComponent<Enemy>());
    }

    /// If an enemy walks out of range, remove it from the list of acknowledged enemies.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy") && !other.CompareTag("LargeEnemy")) return;

        AcknowledgedEnemies.Remove(other.GetComponent<Enemy>());
    }

    /// <summary>
    /// Reduce the shooting cooldown and overheat time. Update the overheat timer.
    /// </summary>
    private void Update()
    {
        _canShootAgain -= Time.deltaTime;
        if (_canShootAgain <= 0) _canShoot = true;

        if (!towerGone) UpdateTimerUi();

        // If no enemies, recover at full speed. If shooting, recover at lower speed.
        if (currentOverheat > 0) currentOverheat -= Time.deltaTime * overheatRecoverRate / (enemyToTarget == null ? 1 : 4);
    }
    
    /// <summary>
    /// When enemy is in range, track and shoot it. When no enemy, reduce overheat.
    /// </summary>
    private void FixedUpdate()
    {
        if (AcknowledgedEnemies.Count > 0)
        {
            TrackAndShoot();
        }
    }

    private void FindComponents()
    {
        _gM = ObjectsInPlay.i.gameManager;
        bulletEmitter = transform.GetChild(0);
        gunEnd = bulletEmitter.GetChild(0);
        _towerAnimator = transform.Find("tower").GetComponent<Animator>();
        rangeCollider = GetComponent<CircleCollider2D>();
    }
}
