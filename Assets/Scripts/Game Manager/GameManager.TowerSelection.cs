using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("Placeable Towers Logic")]
    [HideInInspector] public bool isAlreadyPlacingObject = false;

    private bool isCancellingTower = false;
    private PlaceableTower currentPlacingTower;
    private PurchaseType currentPurchase = PurchaseType.NoPurchaseActive, newPurchase = PurchaseType.NoPurchaseActive;
    
    public enum PurchaseType { PillowTower, WaterTower, FridgeTower, Missile, NoPurchaseActive }
    private int indexOfNewPurchase, indexOfCurrentPurchase;

    /// <summary>
    /// When player selects a tower purchase, spawn a placeable version of a tower prefab on top of them.<br/>
    ///  - If they're swapping from another tower, remove the old and spawn a new placeable tower.<br/>
    /// <br/>
    /// When player selects a missile purchase, activate Missile Mode.
    /// </summary>
    /// <param name="whichPurchase">Purchase that's been approved by GameUi.Purchase()</param>
    public void SpawnPurchasedObject(PurchaseType whichPurchase)
    {
        newPurchase = whichPurchase;
        indexOfNewPurchase = Array.IndexOf(Enum.GetValues(newPurchase.GetType()), newPurchase);

        if (isAlreadyPlacingObject)
        {
            DestroyOldPurchase();
            IfSamePurchaseThenCancelPurchase();
        }
        if (!isCancellingTower)
        {
            if (newPurchase != PurchaseType.Missile)
            {
                SpawnPlaceableTower(DecideTowerToSpawn());
            }
            else
            {
                SpawnMissileReticule();
            }
        }
        isCancellingTower = false;
    }

    private void DestroyOldPurchase()
    {
        // If missile selection is replaced with another selection, toggle missile mode changes
        if (currentPurchase == PurchaseType.Missile)
        {
            mainTower.CancelShooting();
            gameUi.isMissileReticuleActive = false;
            gameUi.ToggleMissileReticuleChanges();

            gameUi.purchaseButtons[indexOfNewPurchase].FlagMissileCancel();
        }

        gameUi.purchaseButtons[indexOfNewPurchase].HideCancelOverlay();

        if (currentPlacingTower != null)
        {
            Destroy(currentPlacingTower.gameObject);
        }
    }

    private void IfSamePurchaseThenCancelPurchase()
    {
        // If it's the same tower, then user is cancelling selection. Skip rest of function.
        if (currentPurchase == newPurchase)
        {
            isAlreadyPlacingObject = false;
            isCancellingTower = true;
            currentPurchase = PurchaseType.NoPurchaseActive;
            if (newPurchase == PurchaseType.Missile)
            {
                mainTower.CancelShooting();
                gameUi.isMissileReticuleActive = false;
                gameUi.ToggleMissileReticuleChanges();
            }
            gameUi.ToggleTowerColourZones();
        }
    }

    private PlaceableTower DecideTowerToSpawn()
    {
        isAlreadyPlacingObject = true;
        gameUi.UpdateCancelOverlays(indexOfNewPurchase);
        switch (newPurchase)
        {
            case PurchaseType.PillowTower:
                return GameAssets.i.pfPlaceablePillow;
            case PurchaseType.WaterTower:
                return GameAssets.i.pfPlaceableWater;
            case PurchaseType.FridgeTower:
                return GameAssets.i.pfPlaceableFridge;
        }
        return null;
    }

    private void SpawnPlaceableTower(PlaceableTower towerToSpawn)
    {
        // Spawn tower where player is standing
        if (towerToSpawn != null)
        {
            currentPlacingTower = Instantiate(towerToSpawn, ObjectsInPlay.i.placeableParent);
            currentPurchase = newPurchase;
            indexOfCurrentPurchase = Array.IndexOf(Enum.GetValues(currentPurchase.GetType()), currentPurchase);
            gameUi.ToggleTowerColourZones();
        }
        else
        {
            Debug.LogError("GameManager.SpawnPlaceableTower - Unassigned or invalid PlaceableTower");
        }
    }



    private void SpawnMissileReticule()
    {
        isAlreadyPlacingObject = true;
        gameUi.UpdateCancelOverlays(indexOfNewPurchase);

        currentPlacingTower = Instantiate(GameAssets.i.pfPlaceableMissile, ObjectsInPlay.i.placeableParent);
        currentPurchase = newPurchase;
        indexOfCurrentPurchase = Array.IndexOf(Enum.GetValues(currentPurchase.GetType()), currentPurchase);

        mainTower.PrepToShoot();

        gameUi.ToggleTowerColourZones();
        gameUi.isMissileReticuleActive = true;
        gameUi.ToggleMissileReticuleChanges();
    }


    private void ResetPurchaseState()
    {
        gameUi.BlockBuildPanel();
        if (currentPurchase != PurchaseType.NoPurchaseActive)
        {
            SpawnPurchasedObject(currentPurchase);
        }
    }
}
