using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlaceableTower : MonoBehaviour
{
    private Transform player, placementCheck, rangeSpriteMask;
    public bool placementIsValid = false;
    private GameManager gM;
    private Color towerFailRed = new Color(0.66f, 0f, 0f, 0.4f);
    private Color towerRangeBlue = new Color(0.34f, 0.45f, 1f, 0.4f);

    [Header("Missile Only Attributes")]
    public bool isMissileReticule = false;
    public float timerBeforeMissileCanLaunch = 0.5f;
    private Vector2 cursorPos, missileReticuleOrigin = new Vector2(6.5f, -4f);
    private readonly float[][] screenBounds = new float[3][]
    {
        new float[] { -9f,  6.5f}, // X bounds for playfield
        new float[] { -5f, 5f }, // Y bounds for playfield & UI
        new float[] { 6.5f, 9f} // X bounds for UI (used to tell reticule to slide up/down along UI edge)
    };

    private void Awake()
    {
        gM = FindObjectOfType<GameManager>();
        player = GameObject.Find("Player").transform;
        placementCheck = transform.Find("PlacementCheck");
        rangeSpriteMask = transform.Find("RangeSpriteMask");
        ResizeRangeIndicator();
        transform.position = missileReticuleOrigin;
    }

    void Update()
    {

        if (isMissileReticule)
        {
            // Move missile reticule to cursor
            cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (cursorPos.y > screenBounds[1][0] && cursorPos.y < screenBounds[1][1])
            {
                if (cursorPos.x > screenBounds[0][0] && cursorPos.x < screenBounds[0][1])
                {
                    transform.position = cursorPos;
                }
                else if (cursorPos.x > screenBounds[2][0] && cursorPos.x < screenBounds[2][1])
                {
                    cursorPos.x = screenBounds[2][0];
                    transform.position = cursorPos;
                }
            }

            timerBeforeMissileCanLaunch -= Time.deltaTime;
            if (timerBeforeMissileCanLaunch <= 0)
            {
                placementIsValid = true;
            }
        }
        else
        {
            // Move tower placement to where player stands
            transform.position = player.position + gM.spritePivotOffset;

            // Check for any red zones that block tower placement
            Vector2 plcCheckVector = new Vector2(placementCheck.position.x, placementCheck.position.y);
            Collider2D[] collidersFound = Physics2D.OverlapCircleAll(plcCheckVector, 0f);
            List<Collider2D> interferingColliders = new List<Collider2D>();

            foreach (Collider2D col in collidersFound)
            {
                if (col.name.StartsWith("Barrier"))
                {
                    interferingColliders.Add(col);
                }
            }

            // If there are no barriers blocking placement, turn the overlay colour for the turret's range to blue.
            // If at least one barrier, turn the overlay colour to red.
            placementIsValid = interferingColliders.Count == 0;
            gM.gameUi.sprTowerRange.color = interferingColliders.Count == 0 ? towerRangeBlue : towerFailRed;
        }
    }

    private void ResizeRangeIndicator() //TODO maybe ask GameManager.TowerSelection which is being placed instead
    {
        if (gameObject.name.EndsWith("Pillow(Clone)"))
        {
            rangeSpriteMask.localScale *= gM.towerManager.rangeOfPillow;
        }
        else if (gameObject.name.EndsWith("Water(Clone)"))
        {
            rangeSpriteMask.localScale *= gM.towerManager.rangeOfWater;
        }
        else if (gameObject.name.EndsWith("Fridge(Clone)"))
        {
            rangeSpriteMask.localScale *= gM.towerManager.rangeOfFridge;
        }
        else if (gameObject.name.EndsWith("Reticule(Clone)"))
        {
            rangeSpriteMask.localScale *= gM.towerManager.rangeOfMissileExpl;
        }
        else
        {
            Debug.LogError("Invalid name for determining PlaceableTower range indicator: " + gameObject.name);
        }
    }
}
