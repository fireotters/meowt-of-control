using System;
using UnityEngine;

public class MainTower : MonoBehaviour
{
    private GameManager _gM;
    private Animator _mainTowerAnimator;
    private AudioSource _audioSource;
    public AudioClip catCannon1, catCannon2;
    float _curTime = 0;
    private const float nextDamage = 0.3f;
    [HideInInspector] public const int stressModeThreshold = 35;
    [SerializeField] private SpriteRenderer _normalSprite = default, _deathSprite = default;

    private Transform confettiLaunchPoint, itemDropPoint;
    [SerializeField] private GameObject confetti = default;
    private Vector2 cursorPos;

    private void Start()
    {
        _gM = ObjectsInPlay.i.gameManager;
        _mainTowerAnimator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();

        confettiLaunchPoint = transform.Find("ConfettiLaunchPoint");
        itemDropPoint = transform.Find("ItemDropPoint");
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (_curTime <= 0)
        {
            if (col.gameObject.CompareTag("LargeEnemy"))
            {
                _gM.gameUi.UpdateBoxCatHealth(-5);
            }
            else if(col.gameObject.CompareTag("Enemy"))
            {
                _gM.gameUi.UpdateBoxCatHealth(-1);
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
        GameObject confettiCopy = Instantiate(confetti, ObjectsInPlay.i.projectilesParent);
        confettiCopy.transform.position = confettiLaunchPoint.position;
        confettiCopy.GetComponent<Confetti>().landingCoords = cursorPos;

        _audioSource.clip = catCannon2;
        _audioSource.Play();
    }

    private void TowerIsDead()
    {
        _mainTowerAnimator.enabled = false;
        _normalSprite.enabled = false;
        _deathSprite.enabled = true;
        _gM.player.PlayerIsDead();
    }

    public void DropItem(DroppedItem.PickupType pickupType)
    {
        DroppedItem.Create(itemDropPoint.position, pickupType);
    }
}
