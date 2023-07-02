using System.Linq;
using UnityEngine;

public class WaterTower : Tower
{
    protected override void TrackAndShoot()
    {
        enemyToTarget = TrackingPriority();
        
        base.TrackAndShoot();
    }

    // Prioritise enemies which haven't been doused yet
    private Transform TrackingPriority()
    {
        foreach (var enemyTransform in AcknowledgedEnemies)
        {
            if (enemyTransform == null)
            {
                // Ignore any unexpected dead enemies
                break;
            }
            if (enemyTransform.GetWaterStatus() == false)
            {
                // Shoot an undoused enemy
                return enemyTransform.transform;
            }
        }
        // Else, shoot the enemy who entered range first.
        return AcknowledgedEnemies.FirstOrDefault().transform;
    }
}
