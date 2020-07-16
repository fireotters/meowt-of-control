using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTower : MonoBehaviour
{
    private Transform player, placementCheck;
    public bool placementIsValid = false;
    public bool isMissileReticule = false;
    private GameManager gM;
    private Color towerFailRed = new Color(0.66f, 0f, 0f, 0.4f);
    private Color towerRangeBlue = new Color(0.34f, 0.45f, 1f, 0.4f);

    private void Awake()
    {
        gM = FindObjectOfType<GameManager>();
        player = GameObject.Find("Player").transform;
        placementCheck = transform.Find("PlacementCheck");
    }

    void Update()
    {
        // Move tower placement to where player stands
        transform.position = player.position + gM.spritePivotOffset;

        if (!isMissileReticule)
        {
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
            /*if (interferingColliders.Count > 0)
            {
                print("PlaceableTower: " + interferingColliders.Count + " barrier colliders found");
            }*/

            placementIsValid = interferingColliders.Count == 0;
            gM.sprTowerRange.color = interferingColliders.Count == 0 ? towerRangeBlue : towerFailRed;
        }
    }
}
