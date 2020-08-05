using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 1f, lifeSpan = 5f;
    private float _lifeLeft;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _sprRenderer;
    private BoxCollider2D _collider;

    private enum BulletType { Player, Pillow, Water, Fridge }

    [Header("Specific Bullet Type Attributes")]
    [SerializeField] private BulletType typeOfBullet = default;
    [SerializeField] private GameObject attachedExplosion = default;
    private GameObject attachedExplosionInstance;

    [SerializeField] private GameObject secondaryEffect = default;
    private GameObject secondaryEffectInstance;

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
        // Ignore colliders that are not enemies
        if (!other.collider.CompareTag("Enemy") && !other.collider.CompareTag("LargeEnemy"))
        {
            return;
        }

        // Pretend to be gone. Stay for .25 seconds to play sound effects & explosion effects
        _collider.enabled = false;
        _sprRenderer.enabled = false;
        PlayExplosionEffect();

        CreateSecondaryEffect();
        Invoke(nameof(ActuallyDestroy), .25f);
    }

    // Only the player bullet has an explosion effect right now
    private void PlayExplosionEffect()
    {
        if (typeOfBullet == BulletType.Player)
        {
            attachedExplosionInstance = Instantiate(attachedExplosion);
            attachedExplosionInstance.transform.position = transform.position;
        }
    }

    // Water Balloon and Fridge towers have secondary effects from their projectiles
    private void CreateSecondaryEffect()
    {
        if (typeOfBullet == BulletType.Water || typeOfBullet == BulletType.Fridge)
        {
            secondaryEffectInstance = Instantiate(secondaryEffect);
            secondaryEffectInstance.transform.position = transform.position;
        }
    }

    private void ActuallyDestroy()
    {
        Destroy(gameObject);
        Destroy(attachedExplosionInstance);
    }
}