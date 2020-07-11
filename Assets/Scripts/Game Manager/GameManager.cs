using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEditorInternal;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public int currentCash = 10000, currentHealth = 100;
    public GameUi gameUi;
    public Transform placementBlockersParent;
    public PlayerController player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPlacingTower)
        {
            AttemptTowerPlacement();
        }
        if (Input.GetKeyDown(KeyCode.O)) // Debug: Show all placement blockers
        {
            foreach (Transform blocker in placementBlockersParent)
            {
                SpriteRenderer spr = blocker.GetComponent<SpriteRenderer>();
                spr.enabled = !spr.enabled;
            }
        }
    }

}
