using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Animator _animator;

    // Rigidbody Movement
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float movementSpeed = 1;
    private float _currentSpeed;
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";
    private Vector2 _movementVector;

    // Weapon Aiming
    private Camera _cam;
    private Vector2 mousePos;
    private Transform bulletEmitter;
    

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _cam = Camera.main;

        bulletEmitter = transform.GetChild(0);
    }

    private void Update()
    {
        // Current movement vector, set character sprite
        _movementVector.x = Input.GetAxis(HorizontalAxis);
        _movementVector.y = Input.GetAxis(VerticalAxis);

        // Current mouse position
        mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        _animator.SetFloat(HorizontalAxis, _movementVector.x);
        _animator.SetFloat(VerticalAxis, _movementVector.y);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void Attack()
    {
        
    }

    private void FixedUpdate()
    {
        _currentSpeed = movementSpeed;
        
        _rigidbody2D.velocity = new Vector2(
            Mathf.Lerp(0, _movementVector.x * _currentSpeed, 0.8f),
            Mathf.Lerp(0, _movementVector.y * _currentSpeed, 0.8f)
        );
        
        _animator.SetFloat("Speed", _rigidbody2D.velocity.magnitude);

        // Bullet emitter direction
        Vector2 lookDir = mousePos - _rigidbody2D.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Vector3 rotationDir = new Vector3(0, 0, angle);
        bulletEmitter.rotation = Quaternion.Euler(rotationDir);
    }
}
