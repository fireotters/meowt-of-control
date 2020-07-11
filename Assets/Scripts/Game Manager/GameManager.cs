using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEditorInternal;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [Header("GameManager")]
    public PlayerController player;
    public int currentCash = 10000, currentHealth = 100;
    public GameUi gameUi;

    private void Start()
    {
        sprRedArea = placementBlockersParent.Find("RedArea").GetComponent<SpriteRenderer>();
        sprGreenArea = placementBlockersParent.Find("ValidPlacements").Find("GreenArea").GetComponent<SpriteRenderer>();
        ToggleTowerColourZones();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPlacingTower)
        {
            AttemptTowerPlacement();
        }
    }

}
