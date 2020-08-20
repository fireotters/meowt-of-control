using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [Header("Dropped Item Expiry")]
    [SerializeField] private float _lifeSpan = 10f, _endBlinkDuration = 3f;


    public PickupType typeOfDrop;
    private GameManager _gM;

    private static Vector3 droppedItemOffset = new Vector2(0f, -0.5f);
    private Vector3 correctingPlacementOffset = new Vector2(0f, 1f);


    public static DroppedItem Create(Vector3 position, PickupType pickupType)
    {
        GameObject selectedItem = default;
        if (pickupType == PickupType.Yarn) selectedItem = GameAssets.i.pfDropYarn;
        else if (pickupType == PickupType.Milk) selectedItem = GameAssets.i.pfDropMilk;
        else if (pickupType == PickupType.Tape) selectedItem = GameAssets.i.pfDropTape;

        Transform droppedItemTransform = Instantiate(selectedItem, position, Quaternion.identity, ObjectsInPlay.i.dropsParent).transform;
        droppedItemTransform.position += droppedItemOffset;

        DroppedItem droppedItem = droppedItemTransform.GetComponent<DroppedItem>();
        return droppedItem;
    }

    private void Awake()
    {
        _gM = ObjectsInPlay.i.gameManager;
        StartCoroutine(nameof(BlinkBeforeDisappearance));
    }

    /// <summary>
    /// When touched by a player, dropped item will be handled. When touching the main tower or a player collider, despawn it.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickupItem(typeOfDrop);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("MainTower") || collision.CompareTag("PlayerCollider"))
        {
            transform.position += correctingPlacementOffset;
        }
    }

    public enum PickupType { Milk, Yarn, Tape }

    /// <summary>
    /// Function that handles picking up items.
    /// 
    /// If milk is picked up: heal the player, or send to GameManager (heal main tower or sell for yarn).
    /// If yarn is picked up: deposit 50 yarn.
    /// </summary>
    /// <param name="type">Item type</param>
    public void PickupItem(PickupType type)
    {
        switch (type)
        {
            case PickupType.Milk:
                _gM.player.HealPlayer();
                break;
            case PickupType.Yarn:
                _gM.gameUi.UpdateYarn(50 * ((_gM.yarnMultiplier + 1) / 2));
                break;
            case PickupType.Tape:
                _gM.HandleTapePickup();
                break;
            default:
                Debug.LogError("GameManger.PickupItem: Invalid pickup type");
                break;
        }
    }
    private IEnumerator BlinkBeforeDisappearance()
    {
        // Wait until item is about to expire
        yield return new WaitForSeconds(_lifeSpan - _endBlinkDuration);

        float elapsedTime = 0f;
        float blinkInterval = 0.2f;
        float fastBlinkInterval = blinkInterval / 4f;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        while (elapsedTime < _endBlinkDuration)
        {
            // When a third of the blinking timer remains, blink much faster
            if (elapsedTime >= (_endBlinkDuration / 3f) * 2)
            {
                blinkInterval = fastBlinkInterval;
            }

            SetSpriteAlphas(renderer, 0.5f);
            yield return new WaitForSeconds(blinkInterval);

            SetSpriteAlphas(renderer, 0.8f);
            yield return new WaitForSeconds(blinkInterval);

            elapsedTime += blinkInterval * 2;
        }
        Destroy(gameObject);
    }
    private void SetSpriteAlphas(SpriteRenderer input, float desiredAlpha)
    {
        Color origColor = input.color;
        input.color = new Color(origColor.r, origColor.g, origColor.b, desiredAlpha);
    }
}
