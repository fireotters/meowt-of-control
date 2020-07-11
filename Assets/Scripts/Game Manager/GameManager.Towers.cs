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
    public PlaceableTower basicPlaceable, cannonPlaceable, snowPlaceable;
    public Tower basicTower, cannonTower, snowTower;

    private PlaceableTower currentPlacingTower;
    private int currentPlacingTowerNum = -1;
    [HideInInspector] public SpriteRenderer sprRedArea, sprGreenArea;
    public Transform placementBlockersParent;

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
                towerToSpawn = basicPlaceable;
                break;
            case 1:
                towerToSpawn = cannonPlaceable;
                break;
            case 2:
                towerToSpawn = snowPlaceable;
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
                towerToSpawn = basicTower;
                gameUi.UpdateCash(-100);
                break;
            case 1:
                towerToSpawn = cannonTower;
                gameUi.UpdateCash(-200);
                break;
            case 2:
                towerToSpawn = snowTower;
                gameUi.UpdateCash(-300);
                break;
        }
        if (towerToSpawn != null)
        {
            // Place tower and a barrier spritemask
            Tower towerPlaced = Instantiate(towerToSpawn, towersInPlayParent);
            towerPlaced.transform.position = player.transform.position;
            GameObject newBarrier = Instantiate(towerBarrierMask, placementBlockersParent);
            newBarrier.transform.position = player.transform.position;

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
        sprRedArea.enabled = isPlacingTower;
        sprGreenArea.enabled = isPlacingTower;
    }
}
