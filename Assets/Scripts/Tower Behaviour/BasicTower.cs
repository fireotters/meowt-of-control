using UnityEngine;

public class BasicTower : Tower
{
    protected override void TrackAndShoot()
    {
        var lookDir = AcknowledgedEnemies[0].position - transform.position;
        var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        var rotationDir = new Vector3(0, 0, angle);
        
        BulletEmitter.rotation = Quaternion.Euler(rotationDir);

        base.TrackAndShoot();
    }

    protected override void Shoot()
    {
        Bullet = Instantiate(bulletPrefab, gunEnd.transform.position, gunEnd.rotation);
        
        base.Shoot();
    }
}
