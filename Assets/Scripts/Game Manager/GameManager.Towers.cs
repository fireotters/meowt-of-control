using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("Placeable Towers Logic")]
    public GameObject towerBarrierMask;
    private bool isPlacingTower = false, isPlacingMissile = false;
    public Transform placeableParent, towersInPlayParent;
    public PlaceableTower placeablePillow, placeableWater, placeableFridge, placeableMissile;
    public Tower towerPillow, towerWater, towerFridge;

    private PlaceableTower currentPlacingTower;
    private int currentPlacingTowerNum = -1;
    [HideInInspector] public SpriteRenderer sprTowerInvalidArea, sprTowerRange;
    public Transform placementBlockersParent;
    public Vector3 spritePivotOffset = new Vector3(0, 0.5f, 0);


    public void SpawnPlaceableTower(int whichTower)
    {
        // If already placing a tower, destroy old choice.
        if (isPlacingTower)
        {
            gameUi.purchaseButtons[whichTower].HideCancelOverlay();
            if (currentPlacingTower != null)
            {
                Destroy(currentPlacingTower.gameObject);
            }

            // If it's the same tower, then user is cancelling selection. Skip rest of function.
            if (currentPlacingTowerNum == whichTower)
            {
                isPlacingTower = false;
                currentPlacingTowerNum = -1;
                if (whichTower == 3)
                {
                    _mainTower.CancelShooting();
                    isMissileReticuleActive = false;
                    ToggleMissileReticuleChanges();
                }
                ToggleTowerColourZones();
                return;
            }
        }

        // Placeable tower instantiation
        isPlacingTower = true;
        gameUi.UpdateCancelOverlays(whichTower);
        PlaceableTower towerToSpawn = null;
        switch (whichTower)
        {
            case 0:
                towerToSpawn = placeablePillow;
                break;
            case 1:
                towerToSpawn = placeableWater;
                break;
            case 2:
                towerToSpawn = placeableFridge;
                break;
            case 3:
                towerToSpawn = placeableMissile;
                break;
        }

        // Spawn tower where player is standing
        if (towerToSpawn != null)
        {
            currentPlacingTower = Instantiate(towerToSpawn, placeableParent);
            currentPlacingTowerNum = whichTower;

            // Spawn missile reticule on mouse
            if (whichTower == 3)
            {
                _mainTower.PrepToShoot();
                isMissileReticuleActive = true;
                ToggleMissileReticuleChanges();
                return;
            }
            ToggleTowerColourZones();
        }
        else
        {
            Debug.LogError("GameManager.SpawnPlaceableTower - Invalid tower type or unassigned PlaceableTower");
        }
    }
    private void SpawnMissileReticule()
    {

    }

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
            // Place tower and a barrier spritemask
            Tower towerPlaced = Instantiate(towerToSpawn, towersInPlayParent);
            towerPlaced.transform.position = player.transform.position + spritePivotOffset;
            GameObject newBarrier = Instantiate(towerBarrierMask, placementBlockersParent);
            newBarrier.transform.position = player.transform.position + spritePivotOffset;

            // Once tower is placed, disable cancel button, destroy placeable version, and toggle red zones
            gameUi.purchaseButtons[currentPlacingTowerNum].HideCancelOverlay();
            Destroy(currentPlacingTower.gameObject);
            isPlacingTower = false;
            ToggleTowerColourZones();
        }
        else if (currentPlacingTowerNum == 3)
        {
            gameUi.purchaseButtons[currentPlacingTowerNum].HideCancelOverlay();
            Destroy(currentPlacingTower.gameObject);
            isPlacingTower = false;
            ToggleTowerColourZones();

            isMissileReticuleActive = false;
            ToggleMissileReticuleChanges();
        }
        else
        {
            Debug.LogError("GameManager.PlaceTower - Invalid tower type");
        }
    }

    private void ToggleTowerColourZones()
    {
        sprTowerInvalidArea.enabled = isPlacingTower;
        sprTowerRange.enabled = isPlacingTower;
        gameUi.buildModeTexts.SetActive(isPlacingTower);
    }

    private bool isMissileReticuleActive = false;
    private Color missileRangeOrange = new Color(0.81f, 0.4f, 0.08f, 0.4f);
    private Color towerRangeBlue = new Color(0.34f, 0.45f, 1f, 0.4f);
    private void ToggleMissileReticuleChanges()
    {
        if (isMissileReticuleActive) {
            sprTowerInvalidArea.enabled = false;
            sprTowerRange.enabled = true;
            gameUi.buildModeTexts.SetActive(false);

            sprTowerRange.color = missileRangeOrange;
            gameUi.launchModeTexts.SetActive(true);
        }
        else
        {
            sprTowerInvalidArea.enabled = isPlacingTower;
            sprTowerRange.enabled = isPlacingTower;
            gameUi.buildModeTexts.SetActive(isPlacingTower);

            sprTowerRange.color = towerRangeBlue;
            gameUi.launchModeTexts.SetActive(false);
        }
    }
}
