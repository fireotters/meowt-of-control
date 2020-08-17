using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FridgeTower : Tower
{
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
}
