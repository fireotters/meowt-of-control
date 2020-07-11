using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] internal float shootCadence = 0.5f;
    internal float CanShootAgain;
    internal bool CanShoot;

    internal Transform BulletEmitter;
    internal List<Transform> AcknowledgedEnemies;
    [SerializeField] internal Transform gunEnd;
    [SerializeField] internal BaseBullet bulletPrefab;

    private void Start()
    {
        CanShootAgain = shootCadence;
        AcknowledgedEnemies = new List<Transform>();
        BulletEmitter = transform.GetChild(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("entered!");
        AcknowledgedEnemies.Add(other.transform);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("exited!");
        AcknowledgedEnemies.RemoveAt(0);
    }
    
    private void Update()
    {
        CanShootAgain -= Time.deltaTime;

        if (CanShootAgain <= 0)
        {
            CanShoot = true;
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
        if (CanShoot)
        {
            Shoot();
        }
    }
    protected abstract void Shoot();
    
}
