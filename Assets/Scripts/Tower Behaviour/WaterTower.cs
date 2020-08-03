using System.Linq;
using UnityEngine;

public class WaterTower : Tower
{
    protected override void TrackAndShoot()
    {
        var enemyToTarget = AcknowledgedEnemies.FirstOrDefault();

        if (enemyToTarget != null)
        {
            var lookDir = enemyToTarget.position - transform.position;
            var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            SetLookAnimation(angle);
            var rotationDir = new Vector3(0, 0, angle);

            BulletEmitter.rotation = Quaternion.Euler(rotationDir);
        }
        else
        {
            AcknowledgedEnemies.Remove(enemyToTarget);
        }

        base.TrackAndShoot();
    }

    protected override void Shoot()
    {
        Bullet = Instantiate(bulletPrefab, gunEnd.transform.position, gunEnd.rotation);
        
        base.Shoot();
    }
}
