using UnityEngine;

public class ObjectsInPlay : MonoBehaviour
{
    private static ObjectsInPlay _i;

    public static ObjectsInPlay i {
        get {
            if (_i == null) _i = (Instantiate(Resources.Load("ObjectsInPlay")) as GameObject).GetComponent<ObjectsInPlay>();
            FindLinks();
            return _i;
        }
    }

    private static void FindLinks()
    {
        if (_i.timerUiParent == null) _i.timerUiParent = FindObjectOfType<Canvas>().transform.Find("TempUiObjects");

        if (_i.playField == null) _i.playField = GameObject.Find("Playfield").transform;
        if (_i.playFieldOther == null) _i.playFieldOther = _i.playField.Find("Other Objects").transform;

        if (_i.towersParent == null) _i.towersParent = _i.playField.Find("Towers");
        if (_i.placeableParent == null) _i.placeableParent = _i.towersParent.Find("Currently Placing This");
        if (_i.placementBlockersParent == null) _i.placementBlockersParent = _i.playField.Find("Other Objects").Find("TowerPlacementBarriers");

        if (_i.enemiesParent == null) _i.enemiesParent = _i.playField.Find("Enemies");
        if (_i.dropsParent == null) _i.dropsParent = _i.playField.Find("Drops");

        if (_i.projectilesParent == null) _i.projectilesParent = _i.playFieldOther.Find("Projectiles");
        if (_i.projectilesParentExtras == null) _i.projectilesParentExtras = _i.projectilesParent.Find("Projectiles Extras");
    }

    [Header("All variables set in code")]
    public Transform playField, playFieldOther;
    public Transform towersParent, placeableParent, placementBlockersParent;
    public Transform enemiesParent, dropsParent, projectilesParent, projectilesParentExtras;
    public Transform timerUiParent;

}
