﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    private GameManager _gM;
    [SerializeField] private float speed = 1f;
    private Rigidbody2D _rigidbody2D;
    [HideInInspector] public Vector2 landingCoords;
    private bool falling = false;

    private void Start()
    {
        _gM = ObjectsInPlay.i.gameManager;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        LaunchUp();
    }

    private void Update()
    {
        if (falling)
        {
            if (transform.position.y <= landingCoords.y)
            {
                Explode();
            }
        }
    }

    private void LaunchUp()
    {
        _rigidbody2D.AddRelativeForce(Vector2.up * speed);
        Invoke(nameof(FallDown), 0.5f);
    }

    private void FallDown()
    {
        falling = true;

        // Determine falling origin
        Vector2 fallFromPoint = transform.position;
        fallFromPoint.x = landingCoords.x;
        fallFromPoint.y = 8f;

        // Reverse confetti
        transform.position = fallFromPoint;
        _rigidbody2D.rotation = 180;
        _rigidbody2D.AddRelativeForce(Vector2.up * speed * 2);
    }

    private void Explode()
    {
        ConfettiExplosion.Create(transform.position);
        Destroy(gameObject);
    }
}
