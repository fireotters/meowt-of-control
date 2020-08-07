using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiExplosion : MonoBehaviour
{

    private CircleCollider2D rangeCollider;
    internal float rangeOfExplosion;

    private float _damageLifeSpan, _objectLifeLeft = 3f;
    [SerializeField] private float missileMaxDamage = default;

    private void Start()
    {
        rangeCollider = GetComponent<CircleCollider2D>();
        rangeCollider.radius *= rangeOfExplosion;
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
        float normalizedDist = distFromBlast / 2.4f; // 2.4f is approximately the maximum range of the missile

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
