using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTower : MonoBehaviour
{
    private GameManager _gM;
    private Animator _mainTowerAnimator;
    private AudioSource _audioSource;
    public AudioClip catCannon1, catCannon2;
    float _curTime = 0;
    private const float nextDamage = 1f;

    // Health bar
    private float healthBarFullSize;
    private Transform healthBar;

    private Transform confettiLaunchPoint;
    [SerializeField] private GameObject confetti = default;
    private Vector3 cursorPos;
    private void Start()
    {
        _gM = FindObjectOfType<GameManager>();
        _mainTowerAnimator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();

        healthBar = transform.Find("HealthBar");
        healthBarFullSize = healthBar.localScale.x;
        confettiLaunchPoint = transform.Find("ConfettiLaunchPoint");
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (_curTime <= 0)
        {
            if (col.gameObject.CompareTag("LargeEnemy"))
            {
                _gM.gameUi.UpdateMainTowerHealth(-10);
                ChangeHealthBar();
            }
            else if(col.gameObject.CompareTag("Enemy"))
            {
                _gM.gameUi.UpdateMainTowerHealth(-5);
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
            _gM.player.PlayerIsDead();
        }

        if (_gM.mainTowerHealth <= 25 && _gM.mainTowerHealth != 0)
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

    public void AnimateShooting()
    {
        _mainTowerAnimator.SetBool("isShooting", true);

        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        Vector3 scaleChange = healthBar.localScale;
        float percentOfLifeLeft = (float)_gM.mainTowerHealth / (float)100;
        scaleChange.x = percentOfLifeLeft * healthBarFullSize;
        healthBar.localScale = scaleChange;
    }
}
