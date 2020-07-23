using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTower : MonoBehaviour
{
    private GameManager _gameManager;
    private Animator _mainTowerAnimator;
    private AudioSource _audioSource;
    public AudioClip catCannon1, catCannon2;
    float _curTime = 0;
    private const float nextDamage = 1f;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _mainTowerAnimator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (_curTime <= 0)
        {
            if (col.gameObject.CompareTag("LargeEnemy"))
            {
                _gameManager.gameUi.UpdateMainTowerHealth(-10);
            }
            else if(col.gameObject.CompareTag("Enemy"))
            {
                _gameManager.gameUi.UpdateMainTowerHealth(-5);
            }

            _curTime = nextDamage;
        }
       
        else
        {
            _curTime -= Time.deltaTime;
        }

        if (_gameManager.mainTowerHealth <= 0)
        {
            _gameManager.player.PlayerIsDead();
        }

        if (_gameManager.mainTowerHealth <= 25 && _gameManager.mainTowerHealth != 0)
        {
            _gameManager.gameUi.musicManager.ChangeStressMode();
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
        _gameManager.gameUi.UpdateYarn(-_gameManager.priceMissile);
        _mainTowerAnimator.SetBool("isShooting", true);
        Invoke(nameof(SoundOfShooting), 0.5f);
        Invoke(nameof(CancelShooting), 2f);
    }

    private void SoundOfShooting()
    {
        _audioSource.clip = catCannon2;
        _audioSource.Play();
    }
}
