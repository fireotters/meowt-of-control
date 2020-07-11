using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField] private float speed = 1f, lifeSpan = 5f;
    private float _lifeLeft;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _lifeLeft = lifeSpan;
        _sprite = GetComponent<SpriteRenderer>();
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
        _rigidbody2D.simulated = false;
        _sprite.enabled = false;
    }
}
