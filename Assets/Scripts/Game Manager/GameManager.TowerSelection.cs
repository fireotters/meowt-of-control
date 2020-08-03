using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("Placeable Towers Logic")]
    public Vector3 spritePivotOffset = new Vector3(0, 0.5f, 0);
    [HideInInspector] public bool isAlreadyPlacingObject = false;
    public Transform placeableParent, towersInPlayParent;
    public PlaceableTower placeablePillow, placeableWater, placeableFridge, placeableMissile;

    private bool isCancellingTower = false;
    private PlaceableTower currentPlacingTower;
    private PurchaseType currentPurchase = PurchaseType.NoPurchaseActive, newPurchase = PurchaseType.NoPurchaseActive;
    public Transform placementBlockersParent;
    

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
                _mainTower.CancelShooting();
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
                return placeablePillow;
            case PurchaseType.WaterTower:
                return placeableWater;
            case PurchaseType.FridgeTower:
                return placeableFridge;
        }
        return null;
    }

    private void SpawnPlaceableTower(PlaceableTower towerToSpawn)
    {
        // Spawn tower where player is standing
        if (towerToSpawn != null)
        {
            currentPlacingTower = Instantiate(towerToSpawn, placeableParent);
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

        currentPlacingTower = Instantiate(placeableMissile, placeableParent);
        currentPurchase = newPurchase;
        indexOfCurrentPurchase = Array.IndexOf(Enum.GetValues(currentPurchase.GetType()), currentPurchase);

        _mainTower.PrepToShoot();

        gameUi.isMissileReticuleActive = true;
        gameUi.ToggleTowerColourZones();
        gameUi.ToggleMissileReticuleChanges();
    }

}
