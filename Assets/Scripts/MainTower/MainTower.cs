using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainTower : MonoBehaviour
{
    public int towerHealthPoints;

    public Text vida;
    
    float curTime = 0;
    float nextDamage = 1;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vida.text = towerHealthPoints.ToString();
    }

    void OnCollisionStay2D()
    {
        if (curTime <= 0)
        {
            towerHealthPoints -= 1;

            Debug.Log("Tower hit!" + towerHealthPoints);

            curTime = nextDamage;
        }
        else
        {
            curTime -= Time.deltaTime;
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
    
    
}
