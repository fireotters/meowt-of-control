using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _spriteAnimator;

    // Rigidbody Movement
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float movementSpeed = 1, shootCadence = 0.2f;
    private float _currentSpeed;
    private float _canShootAgain;

    // First is lerped by GetAxis to ramp player speed. Second snaps between -1 and 1 for animation states
    private Vector2 _movementVector, _animationVector; 

    private bool _canShoot;

    // Weapon Aiming
    private Camera _cam;
    private Vector2 _mousePos;
    private Transform _bulletEmitter;
    [SerializeField] private Bullet bulletPrefab = default;
    private Transform _gunEnd;
    private Player _player;

    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";
    
    //sound
    [SerializeField] private SoundManager soundManager;

    private void Start()
    {
        _canShootAgain = shootCadence;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteAnimator = GetComponentInChildren<Animator>();
        _cam = Camera.main;
        _player = GetComponent<Player>();

        _bulletEmitter = transform.GetChild(0);
        _gunEnd = transform.GetChild(0).GetChild(0);
    }

    private void Update()
    {
        _canShootAgain -= Time.deltaTime;
        
        // Current movement vector, dictates how fast character is moving
        _movementVector.x = Input.GetAxis(HorizontalAxis);
        _movementVector.y = Input.GetAxis(VerticalAxis);

        // Current animation vector, flips instantly between left or right, up or down. Changes sprites
        _animationVector.x = Input.GetAxisRaw(HorizontalAxis);
        _animationVector.y = Input.GetAxisRaw(VerticalAxis);
        if (_animationVector.x == 0)
        {
            _animationVector.x = _movementVector.x;
        }
        if (_animationVector.y == 0)
        {
            _animationVector.y = _movementVector.y;
        }
        _spriteAnimator.SetFloat(HorizontalAxis, _animationVector.x);
        _spriteAnimator.SetFloat(VerticalAxis, _animationVector.y);

        // Current mouse position
        _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);


        if (Input.GetMouseButton(1) && _canShootAgain <= 0)
        {
            if(_player.gunCanBeUsed)
            {
                _canShoot = true;
            }
        }
    }
    
    private void Shoot()
    {
        Bullet.Create(_gunEnd.transform.position, _gunEnd.rotation, bulletPrefab);

        _player.BulletWasShot();
        _canShoot = false;
        _canShootAgain = shootCadence;
    }

    private void FixedUpdate()
    {
        _currentSpeed = movementSpeed;
        
        _rigidbody2D.velocity = new Vector2(
            Mathf.Lerp(0, _movementVector.x * _currentSpeed, 0.8f),
            Mathf.Lerp(0, _movementVector.y * _currentSpeed, 0.8f)
        );
        
        _spriteAnimator.SetFloat("Speed", _rigidbody2D.velocity.magnitude);

        // Bullet emitter direction
        Vector2 lookDir = _mousePos - _rigidbody2D.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Vector3 rotationDir = new Vector3(0, 0, angle);
        _bulletEmitter.rotation = Quaternion.Euler(rotationDir);

        if (_canShoot)
        {
            Shoot();
        }
        
        // When loose health, call soundManager.soundPlayerHit();
    }
}
