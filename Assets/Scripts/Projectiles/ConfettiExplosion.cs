using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiExplosion : MonoBehaviour
{

    private CircleCollider2D rangeCollider;
    internal float rangeOfExplosion;

    private float _damageLifeSpan, _objectLifeLeft = 3f;

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
        hitEnemy.DealDamage(4);
    }
}
