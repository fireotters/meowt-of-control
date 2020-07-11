using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("Placeable Towers Logic")]
    private bool isPlacingTower = false;
    public Transform placeableParent, towersInPlayParent;
    public PlaceableTower basicPlaceable, cannonPlaceable, snowPlaceable;
    public Tower basicTower, cannonTower, snowTower;

    private PlaceableTower currentPlacingTower;
    private int currentPlacingTowerNum = -1;

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
        }
        else
        {
            Debug.LogError("GameManager.SpawnPlaceableTower - Invalid tower type");
        }
    }

    public void AttemptTowerPlacement()
    {
        Transform placementCheck = currentPlacingTower.transform.Find("PlacementCheck");
        Vector2 plcCheckVector = new Vector2(placementCheck.position.x, placementCheck.position.y);
        Collider2D[] collidersFound = Physics2D.OverlapCircleAll(plcCheckVector, 0f);

        if (collidersFound.Length > 1)
        {
            // Colliders found, invalid placement. TODO alert player
            print("Colliders found, not placing tower");
        }
        else
        {
            PlaceTower();
            print("No colliders found, placing tower");
        }
    }

    private void PlaceTower()
    {
        Tower towerToSpawn = null;
        switch (currentPlacingTowerNum)
        {
            case 0:
                towerToSpawn = basicTower;
                break;
            case 1:
                towerToSpawn = cannonTower;
                break;
            case 2:
                towerToSpawn = snowTower;
                break;
        }
        if (towerToSpawn != null)
        {
            Tower towerPlaced = Instantiate(towerToSpawn, towersInPlayParent);
            towerPlaced.transform.position = player.transform.position;

            gameUi.purchaseButtons[currentPlacingTowerNum].HideCancelOverlay();
            Destroy(currentPlacingTower.gameObject);
            isPlacingTower = false;

        }
        else
        {
            Debug.LogError("GameManager.PlaceTower - Invalid tower type");
        }
    }
}
