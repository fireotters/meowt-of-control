using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FridgeTower : Tower
{
    protected override void TrackAndShoot()
    {
        enemyToTarget = TrackingPriority();

        base.TrackAndShoot();
    }

    // Prioritise enemies which haven't been frozen yet
    private Transform TrackingPriority()
    {
        foreach (var enemyTransform in AcknowledgedEnemies)
        {
            if (enemyTransform == null)
            {
                // Ignore any unexpected dead enemies
                break;
            }
            if (enemyTransform.GetFreezeStatus() == false)
            {
                // Shoot an unfrozen enemy
                return enemyTransform.transform;
            }
        }
        // Else, do not shoot. Fridge Tower ignores any non-frozen enemies.
        return null;
    }
}
