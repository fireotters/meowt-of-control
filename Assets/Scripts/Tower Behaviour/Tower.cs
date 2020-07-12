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
    [SerializeField] private SoundManager soundManager;

    private void Start()
    {
        _canShootAgain = shootCadence;
        AcknowledgedEnemies = new List<Transform>();
        BulletEmitter = transform.GetChild(0);
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
        Bullet.Pew();
        _canShoot = false;
        _canShootAgain = shootCadence;
    }
    
    //When destroyed, call soundManager SoundDestroyTurret();
    
}
