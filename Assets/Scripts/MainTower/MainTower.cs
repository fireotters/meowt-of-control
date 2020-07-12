using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainTower : MonoBehaviour
{
    public int towerHealthPoints;
    
    float _curTime = 0;
    float nextDamage = 1;
    
    // Health bar
    private float _healthBarFullSize;
    public Transform healthBar;

    // Start is called before the first frame update
    void Start()
    {
        _healthBarFullSize = healthBar.localScale.x;
    }

    void OnCollisionStay2D()
    {
        if (_curTime <= 0)
        {
            towerHealthPoints -= 1;
            
            ChangeHealthBar(towerHealthPoints / _healthBarFullSize);
            
            Debug.Log("Tower hit!" + towerHealthPoints);

            _curTime = nextDamage;
        }
        else
        {
            _curTime -= Time.deltaTime;
        }
       

        if (towerHealthPoints <= 0)
        {
            lose();
        }
    }

    void lose()
    {
        Destroy(gameObject);
    }
    
    private void ChangeHealthBar(float percentLifeLeft)
    {
        Vector3 scaleChange = healthBar.localScale;
        scaleChange.x = percentLifeLeft;
        healthBar.localScale = scaleChange;
    }
}
