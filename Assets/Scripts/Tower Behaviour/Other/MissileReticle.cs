using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileReticle : PlaceableTower
{
    [Header("Missile Attributes")]
    public float timerBeforeMissileCanLaunch = 0.5f;
    private Vector2 cursorPos, missileReticuleOrigin = new Vector2(6.5f, -4f);
    private readonly float[][] screenBounds = new float[3][]
    {
        new float[] { -9f,  6.5f}, // X bounds for playfield
        new float[] { -5f, 5f }, // Y bounds for playfield & UI
        new float[] { 6.5f, 9f} // X bounds for UI (used to tell reticule to slide up/down along UI edge)
    };
    public float rangeOfMissile;

    private void Awake()
    {
        transform.localScale *= rangeOfMissile;
        transform.position = missileReticuleOrigin;
    }

    void Update()
    {
        // Move missile reticule to cursor
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (cursorPos.y > screenBounds[1][0] && cursorPos.y < screenBounds[1][1])
        {
            if (cursorPos.x > screenBounds[0][0] && cursorPos.x < screenBounds[0][1])
            {
                transform.position = cursorPos;
            }
            else if (cursorPos.x > screenBounds[2][0] && cursorPos.x < screenBounds[2][1])
            {
                cursorPos.x = screenBounds[2][0];
                transform.position = cursorPos;
            }
        }

        timerBeforeMissileCanLaunch -= Time.deltaTime;
        if (timerBeforeMissileCanLaunch <= 0)
        {
            placementIsValid = true;
        }
    }
}
