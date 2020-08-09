using System;
using UnityEngine;

public class MainTower : MonoBehaviour
{
    private GameManager _gM;
    private Animator _mainTowerAnimator;
    private AudioSource _audioSource;
    public AudioClip catCannon1, catCannon2;
    float _curTime = 0;
    private const float nextDamage = 1f;
    private const int stressModeThreshold = 35;

    // Health bar
    private float _healthBarFullSize;
    private Transform _healthBar;
    [SerializeField] private SpriteRenderer _normalSprite = default, _deathSprite = default;

    private Transform confettiLaunchPoint;
    [SerializeField] private GameObject confetti = default;
    private Vector2 cursorPos;

    private void Start()
    {
        _gM = FindObjectOfType<GameManager>();
        _mainTowerAnimator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _healthBar = transform.Find("HealthBar");
        _healthBarFullSize = _healthBar.localScale.x;

        confettiLaunchPoint = transform.Find("ConfettiLaunchPoint");
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (_curTime <= 0)
        {
            if (col.gameObject.CompareTag("LargeEnemy"))
            {
                _gM.gameUi.UpdateBoxCatHealth(-10);
                ChangeHealthBar();
            }
            else if(col.gameObject.CompareTag("Enemy"))
            {
                _gM.gameUi.UpdateBoxCatHealth(-5);
                ChangeHealthBar();
            }

            _curTime = nextDamage;
        }
       
        else
        {
            _curTime -= Time.deltaTime;
        }

        if (_gM.mainTowerHealth <= 0 && !_gM.gameIsOver)
        {
            TowerIsDead();
        }

        if (_gM.mainTowerHealth <= stressModeThreshold && _gM.mainTowerHealth != 0)
        {
            _gM.gameUi.musicManager.ChangeStressMode();
        }
    }

    public void PrepToShoot()
    {
        _mainTowerAnimator.SetBool("begunAiming", true);
        _audioSource.clip = catCannon1;
        _audioSource.Play();
    }

    public void CancelShooting()
    {
        _mainTowerAnimator.SetBool("begunAiming", false);
        _mainTowerAnimator.SetBool("isShooting", false);
    }

    public void AnimateShooting(PlaceableTower missileReticle)
    {
        _mainTowerAnimator.SetBool("isShooting", true);


        cursorPos = missileReticle.transform.position;
        Invoke(nameof(ShootConfetti), 0.5f);
        Invoke(nameof(CancelShooting), 2f);
    }

    private void ShootConfetti()
    {
        GameObject confettiCopy = Instantiate(confetti, _gM.projectilesInPlayParent);
        confettiCopy.transform.position = confettiLaunchPoint.position;
        confettiCopy.GetComponent<Confetti>().landingCoords = cursorPos;

        _audioSource.clip = catCannon2;
        _audioSource.Play();
    }

    public void ChangeHealthBar()
    {
        Vector3 scaleChange = _healthBar.localScale;
        float percentOfLifeLeft = (float)_gM.mainTowerHealth / (float)100;
        scaleChange.x = percentOfLifeLeft * _healthBarFullSize;
        _healthBar.localScale = scaleChange;
    }

    private void TowerIsDead()
    {
        _mainTowerAnimator.enabled = false;
        _normalSprite.enabled = false;
        _deathSprite.enabled = true;
        _gM.player.PlayerIsDead();
    }
}
