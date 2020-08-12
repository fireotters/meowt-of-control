using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiExplosion : MonoBehaviour
{

    private CircleCollider2D rangeCollider;
    private float rangeOfExplosion;

    private float _damageLifeSpan, _objectLifeLeft = 3f;
    [SerializeField] private float missileMaxDamage = default;
    [SerializeField] private MissileReticle attachedReticle = default;

    public static ConfettiExplosion Create(Vector3 position)
    {
        Transform explosionTransform = Instantiate(GameAssets.i.pfConfettiExplosion, position, Quaternion.identity, ObjectsInPlay.i.towersParent).transform;

        ConfettiExplosion explosion = explosionTransform.GetComponent<ConfettiExplosion>();
        return explosion;
    }

    private void Start()
    {
        rangeCollider = GetComponent<CircleCollider2D>();
        rangeOfExplosion = attachedReticle.rangeOfMissile;
        transform.localScale *= attachedReticle.rangeOfMissile;
        _damageLifeSpan = _objectLifeLeft - 0.1f;
    }

    private void Update()
    {
        if (_objectLifeLeft > 0)
            _objectLifeLeft -= Time.deltaTime;
        else if (_objectLifeLeft <= 0)
            Destroy(gameObject);

        // Disable collider after a very short time
        if (_objectLifeLeft < _damageLifeSpan)
        {
            rangeCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy") && !col.CompareTag("LargeEnemy"))
        {
            return;
        }
        Enemy hitEnemy = col.GetComponent<Enemy>();
        float distFromBlast = Vector2.Distance(col.transform.position, transform.position);
        float normalizedDist = distFromBlast / rangeOfExplosion;

        float finalDamage;
        if (normalizedDist < 0.25f)
        {
            finalDamage = missileMaxDamage;
        }
        else if (normalizedDist < 0.5f)
        {
            finalDamage = missileMaxDamage * 0.75f;
        }
        else if (normalizedDist < 0.75f)
        {
            finalDamage = missileMaxDamage * 0.5f;
        }
        else
        {
            finalDamage = missileMaxDamage * 0.25f;
        }
        //Debug.Log("Missile dealt damage: " + finalDamage);
        hitEnemy.DealDamage(finalDamage);
    }
}
