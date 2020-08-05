using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    private enum AoeType { Water, Ice }
    [SerializeField] private AoeType typeOfEffect = default;
    [HideInInspector] public float lifeLeft;
    [SerializeField] private float lifeSpan;

    private void Start()
    {
        lifeLeft = lifeSpan;
    }

    private void Update()
    {
        if (lifeLeft > 0)
            lifeLeft -= Time.deltaTime;
        else if (lifeLeft <= 0)
            Destroy(gameObject);
    }


    /// <summary>
    /// When touched by another AOE, check if it's the same type, and too close. If both are true, remove it. Eg: water touching water in close proximity.<br/>
    /// When touched by an enemy, set their status depending on the AOE type.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("AOE") && col.GetComponent<AreaOfEffect>().typeOfEffect == typeOfEffect)
        {
            Debug.Log(Vector3.Distance(col.transform.position, transform.position));
            if (Vector3.Distance(col.transform.position, transform.position) < 0.2f)
            {
                // Destroy the older copy of the AOE
                if (lifeLeft > col.GetComponent<AreaOfEffect>().lifeLeft)
                {
                    Destroy(col.gameObject);
                }
            }
        }
        if (col.CompareTag("Enemy") || col.CompareTag("LargeEnemy"))
        {
            if (typeOfEffect == AoeType.Ice)
            {
                col.GetComponent<Enemy>().SetFreezeStatus(true);
            }
            else if (typeOfEffect == AoeType.Water)
            {
                col.GetComponent<Enemy>().SetWaterStatus(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") || col.CompareTag("LargeEnemy"))
        {
            if (typeOfEffect == AoeType.Ice)
            {
                col.GetComponent<Enemy>().SetFreezeStatus(false);
            }
            else if (typeOfEffect == AoeType.Water)
            {
                col.GetComponent<Enemy>().SetWaterStatus(false);
            }
        }
    }
}
