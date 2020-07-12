using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public string typeOfDrop;
    private GameManager gM;

    void Start()
    {
        gM = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
        {
            gM.PickupItem("milk");
            Destroy(gameObject);
        }
    }
}
