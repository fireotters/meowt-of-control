using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] internal float shootCadence = 0.5f;
    private float _canShootAgain;
    private bool _canShoot;

    internal Transform BulletEmitter;
    internal List<Transform> AcknowledgedEnemies;
    internal BaseBullet Bullet;
    [SerializeField] internal Transform gunEnd;
    [SerializeField] internal BaseBullet bulletPrefab;
    private Animator _towerAnimator;
    private static readonly int Direction = Animator.StringToHash("Direction");

    private void Start()
    {
        _canShootAgain = shootCadence;
        AcknowledgedEnemies = new List<Transform>();
        BulletEmitter = transform.GetChild(0);
        _towerAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        //if (!other.CompareTag("Enemy") && !other.CompareTag("BigChungusEnemy")) return;
        Debug.Log("enemy entered!");
        AcknowledgedEnemies.Add(other.transform);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        //if (!other.CompareTag("Enemy") && !other.CompareTag("BigChungusEnemy")) return;
        Debug.Log("exited!");
        AcknowledgedEnemies.RemoveAt(0);
    }
    
    private void Update()
    {
        _canShootAgain -= Time.deltaTime;

        if (_canShootAgain <= 0)
        {
            _canShoot = true;
        }
    }
    
    private void FixedUpdate()
    {
        if (AcknowledgedEnemies.Count > 0)
        {
            TrackAndShoot();
        }
    }

    protected virtual void TrackAndShoot()
    {
        if (_canShoot)
        {
            Shoot();
        }
    }

    protected virtual void Shoot()
    {
        _towerAnimator.SetBool("Shoot", true);
        Bullet.Pew();
        _towerAnimator.SetBool("Shoot", false);
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
    
}
