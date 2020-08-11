using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Player.PickupType typeOfDrop;
    private GameManager gM;

    private static Vector3 droppedItemOffset = new Vector2(0f, -0.5f);
    private Vector3 correctingPlacementOffset = new Vector2(0f, 1f);

    public static DroppedItem Create(Vector3 position, Player.PickupType pickupType)
    {
        GameObject selectedItem = default;
        if (pickupType == Player.PickupType.Yarn) selectedItem = GameAssets.i.pfDropYarn;
        else if (pickupType == Player.PickupType.Milk) selectedItem = GameAssets.i.pfDropMilk;
        Debug.Log(selectedItem);

        Transform droppedItemTransform = Instantiate(selectedItem, position, Quaternion.identity, ObjectsInPlay.i.dropsParent).transform;
        droppedItemTransform.position += droppedItemOffset;

        DroppedItem droppedItem = droppedItemTransform.GetComponent<DroppedItem>();
        return droppedItem;
    }

    private void Awake()
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
            transform.position += correctingPlacementOffset;
        }
    }
}
