using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTower : MonoBehaviour
{
    private Transform player, placementCheck;
    public bool placementIsValid = false;
    private GameManager gM;

    private void Awake()
    {
        gM = FindObjectOfType<GameManager>();
        player = GameObject.Find("Player").transform;
        placementCheck = transform.Find("PlacementCheck");
    }
    void Update()
    {
        // Move tower placement to where player stands
        transform.position = player.position;

        // Check for any red zones that block tower placement
        Vector2 plcCheckVector = new Vector2(placementCheck.position.x, placementCheck.position.y);
        Collider2D[] collidersFound = Physics2D.OverlapCircleAll(plcCheckVector, 0f);

        placementIsValid = collidersFound.Length == 1;
        gM.sprGreenArea.color = collidersFound.Length == 1 ? Color.green : Color.red;
    }
}
