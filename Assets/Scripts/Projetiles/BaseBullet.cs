﻿using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField] private float speed = 1f, lifeSpan = 5f;
    private float _lifeLeft;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _lifeLeft = lifeSpan;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_lifeLeft > 0)
            _lifeLeft -= Time.deltaTime;
        else if (_lifeLeft <= 0)
            Destroy(gameObject);
    }

    public void Pew()
    {
        _rigidbody2D.AddRelativeForce(Vector2.right * speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player")
            || other.collider.CompareTag("Projectile")) return;
        //Added to destroy the corpses
        Destroy(gameObject);
        if (other.collider.CompareTag("Corpse"))
        {
            Destroy(other.gameObject);
        }
    }
}