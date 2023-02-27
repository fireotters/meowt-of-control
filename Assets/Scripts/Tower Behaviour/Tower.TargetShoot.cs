using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Tower : MonoBehaviour
{
    /// <summary>
    /// Rotate toward the targetted enemy if there is one. If tower can shoot, then shoot.
    /// </summary>
    protected virtual void TrackAndShoot()
    {
        if (enemyToTarget != null)
        {
            var lookDir = enemyToTarget.position - transform.position;
            var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            SetLookAnimation(angle);
            var rotationDir = new Vector3(0, 0, angle);

            bulletEmitter.rotation = Quaternion.Euler(rotationDir);

            if (_canShoot)
            {
                _towerAnimator.SetTrigger("Shoot");
                Shoot();
            }
        }
    }

    protected virtual void Shoot()
    {
        Bullet.Create(gunEnd.position, gunEnd.rotation, attachedBulletType);
        _canShoot = false;
        _canShootAgain = shootCadence;
        StartCoroutine(ShotIncreasesOverheat());
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
