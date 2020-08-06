using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Player.PickupType typeOfDrop;
    private GameManager gM;

    private Vector3 correctingPlacementOffset = new Vector2(0f, 1f);

    void Start()
    {
        gM = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// When touched by a player, dropped item will be handled by Player gameobject. When touching the main tower or a player collider, despawn it.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gM.player.PickupItem(typeOfDrop);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("MainTower") || collision.CompareTag("PlayerCollider"))
        {
            Destroy(gameObject);
        }
    }
}
