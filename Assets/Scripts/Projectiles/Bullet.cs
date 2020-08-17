using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 1f, lifeSpan = 5f;
    private float _lifeLeft;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _sprRenderer;
    private BoxCollider2D _collider;
    private GameManager _gM;

    private enum BulletType { Player, Pillow, Water, Fridge }

    [Header("Specific Bullet Type Attributes")]
    public float damageToEnemy;
    [SerializeField] private BulletType typeOfBullet = default;
    [SerializeField] private GameObject attachedFizzle = default;
    private GameObject attachedFizzleInstance;

    [SerializeField] private GameObject secondaryEffect = default;
    private GameObject secondaryEffectInstance;

    public static Bullet Create(Vector3 position, Quaternion rotation, Bullet typeToSpawn)
    {
        Transform bulletTransform = Instantiate(typeToSpawn, position, rotation, ObjectsInPlay.i.projectilesParent).transform;

        Bullet bullet = bulletTransform.GetComponent<Bullet>();
        return bullet;
    }

    private void Awake()
    {
        _gM = ObjectsInPlay.i.gameManager;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _sprRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _lifeLeft = lifeSpan;
        Pew();
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

        // Pretend bullet has disappeared. Stay for .25 seconds to play sound effects & explosion effects.
        PretendBulletIsGone();
    }

    private void PretendBulletIsGone()
    {
        _collider.enabled = false;
        _sprRenderer.enabled = false;
        PlayExplosionEffect();

        CreateSecondaryEffect();
        Invoke(nameof(ActuallyDestroy), 0.25f);
    }


    private void PlayExplosionEffect()
    {
        attachedFizzleInstance = Instantiate(attachedFizzle, ObjectsInPlay.i.projectilesParentExtras);
        attachedFizzleInstance.transform.position = transform.position;
    }

    // Water Balloon and Fridge towers have secondary effects from their projectiles
    private void CreateSecondaryEffect()
    {
        if (typeOfBullet == BulletType.Water || typeOfBullet == BulletType.Fridge)
        {
            secondaryEffectInstance = Instantiate(secondaryEffect, ObjectsInPlay.i.projectilesParentExtras);
            secondaryEffectInstance.transform.position = transform.position;
        }
    }

    /// <summary>
    /// Fully remove the Bullet object after attached fizzle has expired.
    /// </summary>
    private void ActuallyDestroy()
    {
        Destroy(gameObject);
        Destroy(attachedFizzleInstance);
    }
}