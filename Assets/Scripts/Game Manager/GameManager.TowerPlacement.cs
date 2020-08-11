using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    /// <summary>
    /// When player confirms placement, check if the PlaceableTower gameobject says placement is valid.
    /// If yes, place a tower and deduct cost.
    /// </summary>
    private void AttemptTowerPlacement()
    {
        if (currentPlacingTower.placementIsValid)
        {
            CompletePurchase();
        }
    }

    private void CompletePurchase()
    {
        Tower towerToSpawn = null;
        switch (currentPurchase)
        {
            case PurchaseType.PillowTower:
                towerToSpawn = GameAssets.i.pfTowerPillow;
                gameUi.UpdateYarn(-pricePillow);
                break;
            case PurchaseType.WaterTower:
                towerToSpawn = GameAssets.i.pfTowerWater;
                gameUi.UpdateYarn(-priceWater);
                break;
            case PurchaseType.FridgeTower:
                towerToSpawn = GameAssets.i.pfTowerFridge;
                gameUi.UpdateYarn(-priceFridge);
                break;
            case PurchaseType.Missile:
                mainTower.AnimateShooting(currentPlacingTower);
                gameUi.UpdateYarn(-priceMissile);
                break;
        }
        gameUi.BeginPurchaseCooldown(indexOfCurrentPurchase);

        if (towerToSpawn != null || currentPurchase == PurchaseType.Missile)
        {
            ActuallyPlaceThePurchase(towerToSpawn);
        }
        else
        {
            Debug.LogError("GameManager.PlaceTower - Invalid tower type");
        }
    }

    private void ActuallyPlaceThePurchase(Tower towerToSpawn)
    {
        if (currentPurchase != PurchaseType.Missile)
        {
            Tower.Create(player.transform.position, towerToSpawn);
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
        currentPurchase = PurchaseType.NoPurchaseActive;
        gameUi.ToggleTowerColourZones();
    }
}
