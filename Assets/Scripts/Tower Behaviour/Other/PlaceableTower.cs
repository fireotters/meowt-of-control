using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTower : MonoBehaviour
{
    [Header("Placeable Tower Attributes")]
    [SerializeField] private Tower attachedTower = default;
    private Transform player, placementCheck, rangeSpriteMask;
    [HideInInspector] public bool placementIsValid = false;
    private GameManager gM;
    private Color towerFailRed = new Color(0.66f, 0f, 0f, 0.4f);
    private Color towerRangeBlue = new Color(0.34f, 0.45f, 1f, 0.4f);

    private void Awake()
    {
        gM = FindObjectOfType<GameManager>();
        player = GameObject.Find("Player").transform;
        placementCheck = transform.Find("PlacementCheck");
        rangeSpriteMask = transform.Find("RangeSpriteMask");
        rangeSpriteMask.localScale *= attachedTower.rangeOfTower;
    }

    void Update()
    {
        // Move tower placement to where player stands
        transform.position = player.position + Tower.spritePivotOffset;

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
