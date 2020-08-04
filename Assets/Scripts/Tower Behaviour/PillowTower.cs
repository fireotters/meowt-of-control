using System.Linq;
using UnityEngine;

public class PillowTower : Tower
{
    protected override void Awake()
    {
        base.Awake();
        rangeOfTower = _gM.towerManager.rangeOfPillow;
    }

    protected override void TrackAndShoot()
    {
        enemyToTarget = AcknowledgedEnemies.FirstOrDefault();

        base.TrackAndShoot();
    }

    protected override void Shoot()
    {
        bullet = Instantiate(bulletPrefab, gunEnd.transform.position, gunEnd.rotation, _gM.projectilesInPlayParent);
        
        base.Shoot();
    }
}
