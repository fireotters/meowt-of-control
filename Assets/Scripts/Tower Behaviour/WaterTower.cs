using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;

public class WaterTower : Tower
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void TrackAndShoot()
    {
        enemyToTarget = AcknowledgedEnemies.FirstOrDefault();

        base.TrackAndShoot();
    }

    protected override void Shoot()
    {
        bullet = Instantiate(bulletPrefab, gunEnd.transform.position, gunEnd.rotation, _gM.projectilesParent);
        
        base.Shoot();
    }
}
