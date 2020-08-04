using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;

public class WaterTower : Tower
{
    protected override void Awake()
    {
        base.Awake();
        rangeOfTower = _gM.towerManager.rangeOfWater;
    }

    protected override void TrackAndShoot()
    {
        enemyToTarget = AcknowledgedEnemies.FirstOrDefault();

        base.TrackAndShoot();
    }

    protected override void Shoot()
    {
        Bullet = Instantiate(bulletPrefab, gunEnd.transform.position, gunEnd.rotation);
        
        base.Shoot();
    }
}
