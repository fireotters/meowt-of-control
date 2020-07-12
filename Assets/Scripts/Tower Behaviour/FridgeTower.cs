using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FridgeTower : Tower
{
    // TODO Actually implement big chungus enemies

    protected override void TrackAndShoot()
    {
        var firstBigEnemy = ObtainFirstBigChungusEnemy(AcknowledgedEnemies);
        var lookDir = firstBigEnemy.position - transform.position;
        var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        SetLookAnimation(angle);
        var rotationDir = new Vector3(0, 0, angle);
        BulletEmitter.rotation = Quaternion.Euler(rotationDir);

        base.TrackAndShoot();
    }

    private Transform ObtainFirstBigChungusEnemy(IEnumerable<Transform> transforms)
    {
        foreach (var enemyTransform in transforms)
        {
            if (enemyTransform.CompareTag("BigChungusEnemy"))
            {
                return enemyTransform;
            }
        }

        return transforms.First();
    }
    
    protected override void Shoot()
    {
        Bullet = Instantiate(bulletPrefab, gunEnd.transform.position, gunEnd.rotation);
        
        base.Shoot();
    }
}
