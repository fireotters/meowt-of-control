using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;

    // Rigidbody Movement
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float movementSpeed = 1, shootCadence = 0.2f;
    private float _currentSpeed;
    private float _canShootAgain;
    private Vector2 _movementVector;

    private bool _canShoot;

    // Weapon Aiming
    private Camera _cam;
    private Vector2 _mousePos;
    private Transform _bulletEmitter;
    [SerializeField] private BaseBullet bulletPrefab;
    [SerializeField] private Transform gunEnd;
    private Player _player;
    
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";
    
    //sound
    [SerializeField] private SoundManager soundManager;

    private void Start()
    {
        _canShootAgain = shootCadence;
        
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _cam = Camera.main;
        _player = GetComponent<Player>();

        _bulletEmitter = transform.GetChild(0);
    }

    private void Update()
    {
        _canShootAgain -= Time.deltaTime;
        
        // Current movement vector, set character sprite
        _movementVector.x = Input.GetAxis(HorizontalAxis);
        _movementVector.y = Input.GetAxis(VerticalAxis);

        // Current mouse position
        _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        _animator.SetFloat(HorizontalAxis, _movementVector.x);
        _animator.SetFloat(VerticalAxis, _movementVector.y);

        if (Input.GetMouseButton(1) && _canShootAgain <= 0)
        {
            if(_player.gunCanBeUsed)
            {
                _canShoot = true;
            }
            else
            {
                _player.NoAmmo();
            }
        }
    }
    
    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, gunEnd.transform.position, gunEnd.rotation);
        bullet.Pew();

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
        
        _animator.SetFloat("Speed", _rigidbody2D.velocity.magnitude);

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
