﻿using System.Collections.Generic;
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
    internal BaseBullet bullet;
    [SerializeField] internal Transform gunEnd = default;
    [SerializeField] internal BaseBullet bulletPrefab = default;
    internal Transform enemyToTarget;
    private CircleCollider2D rangeCollider;
    internal float rangeOfTower;

    [Header("Timer Functionality")]
    [SerializeField] private float maxLifespan = default;
    private float timeLeft;
    private bool towerGone = false;

    [Header("Timer UI")]
    public GameObject timerUiPrefab;
    private Image currentTimerUi;
    private Transform timerUiParent;
    private Vector3 timerUiOffset = new Vector2(0f, -0.5f);

    [Header("Animation, etc")]
    [SerializeField] internal Animator _towerAnimator = default;
    private static readonly int Direction = Animator.StringToHash("Direction");
    [SerializeField] private AudioClip audTowerDestroy = default;
    internal GameManager _gM;

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
        currentTimerUi = Instantiate(timerUiPrefab, timerUiParent).GetComponent<Image>();
        currentTimerUi.transform.position = transform.position + timerUiOffset;
        timeLeft = maxLifespan;
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
    }
    
    private void FixedUpdate()
    {
        if (AcknowledgedEnemies.Count > 0)
        {
            TrackAndShoot();
        }
        else
        {
            if (timeLeft < maxLifespan)
            {
                timeLeft += Time.deltaTime / 4f;
            }
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

        // While able to shoot, subtract from tower timer
        timeLeft -= Time.deltaTime;

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

    private void UpdateTimerUi()
    {
        currentTimerUi.fillAmount = timeLeft / maxLifespan;
        if (timeLeft <= 0f && !towerGone)
        {
            towerGone = true;
            DisableTurretPlayDestroySound();
        }
    }

    private void DisableTurretPlayDestroySound()
    {
        AudioSource audioSrc = GetComponent<AudioSource>();
        audioSrc.clip = audTowerDestroy;
        audioSrc.Play();

        // Set tower to not shoot, pretend to be gone
        _canShootAgain = 3f;
        transform.Find("tower").gameObject.SetActive(false);
        transform.Find("base").gameObject.SetActive(false);

        // Drop a differently coloured piece of scrap. TODO replace with recoloured sprite
        GameObject towerScrapDrop = Instantiate(_gM.scrap, _gM.gameUi.dropsInPlayParent);
        towerScrapDrop.transform.position = transform.position;
        towerScrapDrop.GetComponent<SpriteRenderer>().color = new Color(0.21f, 0.65f, 0.97f);

        Invoke(nameof(DestroyTurret), 1f);
    }

    private void DestroyTurret()
    {
        Destroy(gameObject);
    }
    
}
