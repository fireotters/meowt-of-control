using System.Linq;
using UnityEngine;

public class WaterTower : Tower
{
    protected override void TrackAndShoot()
    {
        enemyToTarget = AcknowledgedEnemies.FirstOrDefault();
        
        base.TrackAndShoot();
    }
}
