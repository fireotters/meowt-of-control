using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField] private float speed = 1f, lifeSpan = 5f;
    private float _lifeLeft;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _sprRenderer;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _sprRenderer = GetComponent<SpriteRenderer>();
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
        //Destroy scrap TODO improve
        if (other.collider.CompareTag("Scrap"))
        {
            Destroy(other.gameObject);
        }
        else if (!other.collider.CompareTag("Enemy") && !other.collider.CompareTag("LargeEnemy"))
        {
            return;
        }
        _collider.enabled = false;
        _sprRenderer.enabled = false;
        Invoke(nameof(ActuallyDestroy), 1f);
    }

    private void ActuallyDestroy()
    {
        Destroy(gameObject);
    }
}