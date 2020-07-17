using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("Placeable Towers Logic")]
    [HideInInspector] public bool isPlacingTower = false, isPlacingMissile = false;
    public Transform placeableParent, towersInPlayParent;
    public PlaceableTower placeablePillow, placeableWater, placeableFridge, placeableMissile;

    private bool isCancellingTower = false;
    private PlaceableTower currentPlacingTower;
    private int currentPlacingTowerNum = -1, newPlacingTowerNum = -1;
    public Transform placementBlockersParent;
    public Vector3 spritePivotOffset = new Vector3(0, 0.5f, 0);
    
    /// <summary>
    /// When player selects a tower to place, spawn a placeable version of the prefab on top of them.
    /// - If they're swapping from another tower, remove the old and spawn a new placeable tower.
    /// </summary>
    /// <param name="whichTower">Tower type to place</param>
    public void SpawnPlaceableTower(int whichTower)
    {
        newPlacingTowerNum = whichTower;

        if (isPlacingTower)
        {
            DestroyOldTowerSpawnNewOne();
        }
        if (!isCancellingTower)
        {
            SpawnPlaceableTowerWherePlayerStanding(DecideTowerToSpawn());
        }
        isCancellingTower = false;
    }

    private void DestroyOldTowerSpawnNewOne()
    {
        gameUi.purchaseButtons[newPlacingTowerNum].HideCancelOverlay();
        if (currentPlacingTower != null)
        {
            Destroy(currentPlacingTower.gameObject);
        }

        // If it's the same tower, then user is cancelling selection. Skip rest of function.
        if (currentPlacingTowerNum == newPlacingTowerNum)
        {
            isPlacingTower = false;
            isCancellingTower = true;
            currentPlacingTowerNum = -1;
            if (newPlacingTowerNum == 3)
            {
                _mainTower.CancelShooting();
                gameUi.isMissileReticuleActive = false;
                gameUi.ToggleMissileReticuleChanges();
            }
            gameUi.ToggleTowerColourZones();
        }
    }

    private PlaceableTower DecideTowerToSpawn()
    {
        isPlacingTower = true;
        gameUi.UpdateCancelOverlays(newPlacingTowerNum);
        switch (newPlacingTowerNum)
        {
            case 0:
                return placeablePillow;
            case 1:
                return placeableWater;
            case 2:
                return placeableFridge;
            case 3:
                return placeableMissile;
        }
        return null;
    }

    private void SpawnPlaceableTowerWherePlayerStanding(PlaceableTower towerToSpawn)
    {
        // Spawn tower where player is standing
        if (towerToSpawn != null)
        {
            currentPlacingTower = Instantiate(towerToSpawn, placeableParent);
            currentPlacingTowerNum = newPlacingTowerNum;

            // Spawn missile reticule on mouse
            if (newPlacingTowerNum == 3)
            {
                _mainTower.PrepToShoot();
                gameUi.isMissileReticuleActive = true;
                gameUi.ToggleMissileReticuleChanges();
                return;
            }
            gameUi.ToggleTowerColourZones();
        }
        else
        {
            Debug.LogError("GameManager.SpawnPlaceableTower - Unassigned or invalid PlaceableTower");
        }
    }



    private void SpawnMissileReticule()
    {

    }

}
