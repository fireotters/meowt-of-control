using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Player.PickupType typeOfDrop;
    private GameManager gM;

    void Start()
    {
        gM = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gM.player.PickupItem(typeOfDrop);
            Destroy(gameObject);
        }
    }
}
