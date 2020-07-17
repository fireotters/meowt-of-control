using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public GameObject towerBarrierMask;
    public Tower towerPillow, towerWater, towerFridge;

    /* ------------------------------------------------------------------------------------------------------------------
     * Tower Placement
     * 
     * When player confirms placement, check if the PlaceableTower gameobject says placement is valid.
     * If yes, place a tower and deduct cost.
     * ------------------------------------------------------------------------------------------------------------------ */
    public void AttemptTowerPlacement()
    {
        if (currentPlacingTower.placementIsValid)
        {
            PlaceTower();
        }
    }

    private void PlaceTower()
    {
        Tower towerToSpawn = null;
        switch (currentPlacingTowerNum)
        {
            case 0:
                towerToSpawn = towerPillow;
                gameUi.UpdateYarn(-pricePillow);
                break;
            case 1:
                towerToSpawn = towerWater;
                gameUi.UpdateYarn(-priceWater);
                break;
            case 2:
                towerToSpawn = towerFridge;
                gameUi.UpdateYarn(-priceFridge);
                break;
            case 3:
                _mainTower.AnimateShooting();
                break;
        }
        if (towerToSpawn != null)
        {
            if (currentPlacingTowerNum != 3)
            {
                // Place tower and a barrier spritemask
                Tower towerPlaced = Instantiate(towerToSpawn, towersInPlayParent);
                towerPlaced.transform.position = player.transform.position + spritePivotOffset;
                GameObject newBarrier = Instantiate(towerBarrierMask, placementBlockersParent);
                newBarrier.transform.position = player.transform.position + spritePivotOffset;
            }
            else
            {
                gameUi.isMissileReticuleActive = false;
                gameUi.ToggleMissileReticuleChanges();
            }

            // Once tower is placed or missile fired, disable cancel button, destroy placeable version, and toggle red zones.
            gameUi.purchaseButtons[currentPlacingTowerNum].HideCancelOverlay();
            Destroy(currentPlacingTower.gameObject);
            isPlacingTower = false;
            gameUi.ToggleTowerColourZones();
        }
        else
        {
            Debug.LogError("GameManager.PlaceTower - Invalid tower type");
        }
    }
}
