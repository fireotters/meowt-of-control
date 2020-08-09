using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tower : MonoBehaviour
{
    [Header("Shooting & Targeting")]
    [SerializeField] internal float shootCadence = 0.5f;
    private float _canShootAgain;
    private bool _canShoot; 
    internal Transform BulletEmitter;
    internal List<Transform> AcknowledgedEnemies;
    internal Bullet bullet;
    [SerializeField] internal Transform gunEnd = default;
    [SerializeField] internal Bullet bulletPrefab = default;
    internal Transform enemyToTarget;
    private CircleCollider2D rangeCollider;
    public float rangeOfTower;

    private enum RecoverSpeed { Slow, Mid, Fast }
    [Header("Overheat (Max is 10, scale penalties accordingly)")]
    [SerializeField] private float penaltyPerShot = default;
    [SerializeField] private RecoverSpeed overheatRecoverSpeed = default;
    private readonly float maxOverheat = 10;
    private float timeOverheated = 0f, overheatRecoveryRate;
    private bool towerGone = false;

    [Header("Timer UI")]
    public GameObject timerUiPrefab;
    private Image currentTimerUiFill;
    private Transform currentTimerUi, timerUiParent;
    private Vector3 timerUiOffset = new Vector2(0f, -0.5f);

    [Header("Animation, etc")]
    [SerializeField] internal Animator _towerAnimator = default;
    private static readonly int Direction = Animator.StringToHash("Direction");
    [SerializeField] private AudioClip audTowerDestroy = default;
    internal GameManager _gM;


    [SerializeField] private GameObject _attachedScrap = default;
    [HideInInspector] public GameObject attachedPlacementBlocker;

    protected virtual void Awake()
    {
        _gM = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        _canShootAgain = shootCadence;
        AcknowledgedEnemies = new List<Transform>();
        BulletEmitter = transform.GetChild(0);

        rangeCollider = GetComponent<CircleCollider2D>();
        rangeCollider.radius *= rangeOfTower;

        timerUiParent = FindObjectOfType<Canvas>().transform.Find("TowerUiTimerParent");
        currentTimerUi = Instantiate(timerUiPrefab, timerUiParent).transform;
        currentTimerUi.transform.position = transform.position + timerUiOffset;
        currentTimerUiFill = currentTimerUi.Find("Fill").GetComponent<Image>();

        // Determine multiplier for recovery speed
        switch (overheatRecoverSpeed)
        {
            case RecoverSpeed.Slow:
                overheatRecoveryRate = 0.5f;
                break;
            case RecoverSpeed.Mid:
                overheatRecoveryRate = 2f;
                break;
            case RecoverSpeed.Fast:
                overheatRecoveryRate = 4f;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Dismiss collider if it isn't an enemy
        if (!other.CompareTag("Enemy") && !other.CompareTag("LargeEnemy")) return;

        AcknowledgedEnemies.Add(other.transform);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        // Dismiss collider if it isn't an enemy
        if (!other.CompareTag("Enemy") && !other.CompareTag("LargeEnemy")) return;

        AcknowledgedEnemies.Remove(other.transform);
    }
    
    private void Update()
    {
        _canShootAgain -= Time.deltaTime;

        if (_canShootAgain <= 0)
        {
            _canShoot = true;
        }

        UpdateTimerUi();

        if (timeOverheated > 0)
        {
            // If no enemies, recover at full speed. If shooting, recover at quarter speed.
            timeOverheated -= Time.deltaTime * overheatRecoveryRate / (enemyToTarget == null ? 1 : 4);
        }
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

    protected virtual void TrackAndShoot()
    {
        // Tracking logic
        if (enemyToTarget != null)
        {
            var lookDir = enemyToTarget.position - transform.position;
            var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            SetLookAnimation(angle);
            var rotationDir = new Vector3(0, 0, angle);

            BulletEmitter.rotation = Quaternion.Euler(rotationDir);
        }
        else
        {
            AcknowledgedEnemies.Remove(enemyToTarget);
        }

        if (_canShoot)
        {
            _towerAnimator.SetTrigger("Shoot");
            Shoot();
        }
    }

    protected virtual void Shoot()
    {
        bullet.Pew();
        _canShoot = false;
        _canShootAgain = shootCadence;
        StartCoroutine(ShotIncreasesOverheat());
    }

    protected void SetLookAnimation(float angle)
    {
        if (Inbetween(angle, -45, 45))
        {
            _towerAnimator.SetInteger(Direction, 3);
        }
        else if (Inbetween(angle, 45, 145))
        {
            _towerAnimator.SetInteger(Direction, 2);
        }
        else if (Inbetween(angle, 145, 180) || Inbetween(angle, -180, -145))
        {
            _towerAnimator.SetInteger(Direction, 1);
        }
        else if (Inbetween(angle, -145, -45))
        {
            _towerAnimator.SetInteger(Direction, 0);
        }
    }

    private static bool Inbetween(float target, float val1, float val2)
    {
        return target > val1 && target < val2;
    }

    private IEnumerator ShotIncreasesOverheat()
    {
        for (int i = 0; i < 3; i++)
        {
            timeOverheated += penaltyPerShot / 3;
            yield return new WaitForSeconds(shootCadence / 10);
        }
    }

    private void UpdateTimerUi()
    {
        if (currentTimerUiFill != null)
        {
            currentTimerUiFill.fillAmount = timeOverheated / maxOverheat;
        }
        if (timeOverheated >= maxOverheat && !towerGone)
        {
            towerGone = true;
            DisableTurretPlayDestroySound();
        }
    }

    private void DisableTurretPlayDestroySound()
    {
        // Play destruction sound
        AudioSource audioSrc = GetComponent<AudioSource>();
        audioSrc.clip = audTowerDestroy;
        audioSrc.Play();

        // Set tower to not shoot, pretend to be gone
        _canShootAgain = 3f;
        transform.Find("tower").gameObject.SetActive(false);
        transform.Find("base").gameObject.SetActive(false);
        Destroy(currentTimerUi.gameObject);
        Destroy(attachedPlacementBlocker);

        // Drop a differently coloured piece of scrap
        GameObject towerScrapDrop = Instantiate(_attachedScrap, _gM.gameUi.dropsInPlayParent);
        towerScrapDrop.transform.position = transform.position;

        Invoke(nameof(DestroyTurret), 1f);
    }

    private void DestroyTurret()
    {
        Destroy(gameObject);
    }

    public void BigEnemyDestroysTower()
    {
        timeOverheated = maxOverheat;
    }
    
}
