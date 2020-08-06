using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("Tower Placement Variables")]
    public GameObject towerBarrierMask;
    public Tower towerPillow, towerWater, towerFridge;
    
    /// <summary>
    /// When player confirms placement, check if the PlaceableTower gameobject says placement is valid.
    /// If yes, place a tower and deduct cost.
    /// </summary>
    private void AttemptTowerPlacement()
    {
        if (currentPlacingTower.placementIsValid)
        {
            PlaceTower();
        }
    }

    private void PlaceTower()
    {
        Tower towerToSpawn = null;
        switch (currentPurchase)
        {
            case PurchaseType.PillowTower:
                towerToSpawn = towerPillow;
                gameUi.UpdateYarn(-pricePillow);
                break;
            case PurchaseType.WaterTower:
                towerToSpawn = towerWater;
                gameUi.UpdateYarn(-priceWater);
                break;
            case PurchaseType.FridgeTower:
                towerToSpawn = towerFridge;
                gameUi.UpdateYarn(-priceFridge);
                break;
            case PurchaseType.Missile:
                mainTower.AnimateShooting();
                gameUi.UpdateYarn(-priceMissile);
                break;
        }
        gameUi.BeginPurchaseCooldown(indexOfCurrentPurchase);

        if (towerToSpawn != null || currentPurchase == PurchaseType.Missile)
        {
            if (currentPurchase != PurchaseType.Missile)
            {
                // Place tower and a barrier spritemask
                Tower towerPlaced = Instantiate(towerToSpawn, towersInPlayParent);
                towerPlaced.transform.position = player.transform.position + spritePivotOffset;
                GameObject newBarrier = Instantiate(towerBarrierMask, placementBlockersParent);
                newBarrier.transform.position = player.transform.position + spritePivotOffset;
            }
            else
            {
                // Disable Missile Mode changes
                gameUi.isMissileReticuleActive = false;
                gameUi.ToggleMissileReticuleChanges();
            }

            // Once tower is placed or missile fired, disable cancel button, destroy placeable version, and disable Build / Missile Mode.
            gameUi.purchaseButtons[indexOfCurrentPurchase].HideCancelOverlay();
            Destroy(currentPlacingTower.gameObject);
            isAlreadyPlacingObject = false;
            gameUi.ToggleTowerColourZones();
        }
        else
        {
            Debug.LogError("GameManager.PlaceTower - Invalid tower type");
        }
    }
}
