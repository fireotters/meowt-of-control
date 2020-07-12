using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("Placeable Towers Logic")]
    public GameObject towerBarrierMask;
    private bool isPlacingTower = false;
    public Transform placeableParent, towersInPlayParent;
    public PlaceableTower placeablePillow, placeableWater, placeableFridge;
    public Tower towerPillow, towerWater, towerFridge;

    private PlaceableTower currentPlacingTower;
    private int currentPlacingTowerNum = -1;
    [HideInInspector] public SpriteRenderer sprTowerInvalidArea, sprTowerRange;
    public Transform placementBlockersParent;
    public Vector3 spritePivotOffset = new Vector3(0, 0.5f, 0);

    [HideInInspector] public int pricePillow = 10, priceWater = 30, priceFridge = 50;

    public void SpawnPlaceableTower(int whichTower)
    {
        // If already placing a tower, destroy old choice.
        if (isPlacingTower)
        {
            gameUi.purchaseButtons[whichTower].HideCancelOverlay();
            Destroy(currentPlacingTower.gameObject);

            // If it's the same tower, then user is cancelling selection. Skip rest of function.
            if (currentPlacingTowerNum == whichTower)
            {
                isPlacingTower = false;
                currentPlacingTowerNum = -1;
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
        }
        if (towerToSpawn != null)
        {
            currentPlacingTower = Instantiate(towerToSpawn, placeableParent);
            currentPlacingTowerNum = whichTower;
            ToggleTowerColourZones();
        }
        else
        {
            Debug.LogError("GameManager.SpawnPlaceableTower - Invalid tower type");
        }
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
                gameUi.UpdateCash(-pricePillow);
                break;
            case 1:
                towerToSpawn = towerWater;
                gameUi.UpdateCash(-priceWater);
                break;
            case 2:
                towerToSpawn = towerFridge;
                gameUi.UpdateCash(-priceFridge);
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
}
