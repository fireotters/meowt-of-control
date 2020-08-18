using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class GameUi : BaseUi
{
    [HideInInspector] public SpriteRenderer sprTowerInvalidArea, sprTowerRange;
    [HideInInspector] public bool isMissileReticuleActive = false;
    private Color missileRangeOrange = new Color(0.81f, 0.4f, 0.08f, 0.4f);
    private Color towerRangeBlue = new Color(0.34f, 0.45f, 1f, 0.4f);
    public GameObject demolishText, enemyCountTextObject;
    public TextMeshProUGUI enemyCountText;

    /// <summary>
    /// Enables or disables build Mode hints on placement limits and building ranges.
    /// </summary>
    public void ToggleTowerColourZones()
    {
        sprTowerInvalidArea.enabled = _gM.isAlreadyPlacingObject;
        sprTowerRange.enabled = _gM.isAlreadyPlacingObject;
        buildModeTexts.SetActive(_gM.isAlreadyPlacingObject);
        enemyCountTextObject.SetActive(!_gM.isAlreadyPlacingObject);
    }

    /// <summary>
    /// Swaps elements of the screen if Missile Mode is selected or unselected
    /// </summary>
    public void ToggleMissileReticuleChanges()
    {
        if (isMissileReticuleActive)
        {
            sprTowerInvalidArea.enabled = false;
            sprTowerRange.enabled = true;
            buildModeTexts.SetActive(false);

            sprTowerRange.color = missileRangeOrange;
            launchModeTexts.SetActive(true);
        }
        else
        {
            ToggleTowerColourZones();

            sprTowerRange.color = towerRangeBlue;
            launchModeTexts.SetActive(false);
        }
    }

    public void ToggleDemolishText()
    {
        demolishText.SetActive(Input.GetKey(KeyCode.LeftShift));
    }
}
