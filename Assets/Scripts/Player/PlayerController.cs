using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _movementVector;

    [SerializeField] private Animator _animator;
    
    private float _currentSpeed;

    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _movementVector.x = Input.GetAxis(HorizontalAxis);
        _movementVector.y = Input.GetAxis(VerticalAxis);

        _animator.SetFloat(HorizontalAxis, _movementVector.x);
        _animator.SetFloat(VerticalAxis, _movementVector.y);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("attack!");
    }
    
    private void FixedUpdate()
    {
        _currentSpeed = movementSpeed;
        
        _rigidbody2D.velocity = new Vector2(
            Mathf.Lerp(0, _movementVector.x * _currentSpeed, 0.8f),
            Mathf.Lerp(0, _movementVector.y * _currentSpeed, 0.8f)
        );
        
        _animator.SetFloat("Speed", _rigidbody2D.velocity.magnitude);
    }
}
