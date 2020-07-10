using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _movementVector;
    private SpriteRenderer _sprite;

    [SerializeField] private GameObject emitterPos;

    private Animator _animator;
    
    private float _currentSpeed;

    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        
        if (emitterPos == null)
        {
            Debug.LogError("No emitter pos, can't shoot!");
        }
    }

    private void Update()
    {
        _movementVector.x = Input.GetAxis(HorizontalAxis);
        _movementVector.y = Input.GetAxis(VerticalAxis);
        
        emitterPos.transform.position = SetEmitterPos();

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

    private Vector3 SetEmitterPos()
    {
        var position = transform.position;
        var result = Vector3.zero;
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            result = new Vector3(position.x, _sprite.bounds.extents.y, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            result = new Vector3(position.x, -_sprite.bounds.extents.y, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            result = new Vector3(_sprite.bounds.extents.x, position.y, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            result = new Vector3(_sprite.bounds.extents.x, position.y, 0f);
        }

        return result;
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
