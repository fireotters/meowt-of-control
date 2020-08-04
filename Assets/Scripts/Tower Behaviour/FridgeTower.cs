using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FridgeTower : Tower
{
    protected override void Awake()
    {
        base.Awake();
        rangeOfTower = _gM.towerManager.rangeOfFridge;
    }

    protected override void TrackAndShoot()
    {
        enemyToTarget = TargetBigEnemyFirstThenOthers();

        base.TrackAndShoot();
    }

    private Transform TargetBigEnemyFirstThenOthers()
    {
        foreach (var enemyTransform in AcknowledgedEnemies)
        {
            if (enemyTransform == null)
            {
                break;
            }
            if (enemyTransform.CompareTag("LargeEnemy"))
            {
                return enemyTransform;
            }
        }
        return AcknowledgedEnemies.FirstOrDefault();
    }
    
    protected override void Shoot()
    {
        bullet = Instantiate(bulletPrefab, gunEnd.transform.position, gunEnd.rotation, _gM.projectilesInPlayParent);
        
        base.Shoot();
    }
}
