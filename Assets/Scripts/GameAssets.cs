using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i {
        get {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    [Header("Drop-Related Prefabs")]
    public GameObject pfScrap;
    public GameObject pfDropMilk, pfDropYarn;

    [Header("Tower-Related Prefabs")]
    public MissileReticle pfPlaceableMissile;
    public GameObject pfCircleBarrier, pfTowerOverheatTimer;
    public Tower pfTowerPillow, pfTowerWater, pfTowerFridge;
    public PlaceableTower pfPlaceablePillow, pfPlaceableWater, pfPlaceableFridge;

    [Header("Sounds")]
    public AudioClip audTowerDestroy;
}
