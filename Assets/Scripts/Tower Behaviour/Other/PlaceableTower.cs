using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTower : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        transform.position = player.position;
    }
}
