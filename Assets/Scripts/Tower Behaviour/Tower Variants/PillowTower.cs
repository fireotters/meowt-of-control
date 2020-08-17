using System.Linq;
using UnityEngine;

public class PillowTower : Tower
{

    protected override void TrackAndShoot()
    {
        enemyToTarget = AcknowledgedEnemies.FirstOrDefault();

        base.TrackAndShoot();
    }
}
