using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEditorInternal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentCash = 10000, currentHealth = 100;

    [Header("Placeable Towers Logic")]
    private bool isPlacingTower = false;
    public Transform placeableParent;
    public PlaceableTower basicPlaceable, cannonPlaceable, snowPlaceable;

    private PlaceableTower currentPlacingTower;
    private int currentPlacingTowerNum = -1;

    public GameUi gameUi;

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
}
