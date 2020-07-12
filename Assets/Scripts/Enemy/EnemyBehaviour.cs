using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject corpse;
    public int bigChungusLife = 5;
    public int basicLife = 2;
    public int sanicLife = 1;

    private Vector2 enemyLastPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D (Collision2D col){
        if (col.gameObject.tag == "Enemy")
        {
            basicLife--;
            Debug.Log(("AAAAAAAAAAAA"));
        }
        if (col.gameObject.tag == "Enemy" && basicLife == 0) {
            Vector2 enemyLastPos = new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.y);
            Destroy (col.gameObject);
            corpse.transform.position = enemyLastPos;
            Instantiate(corpse);
        }

        if (col.gameObject.tag == "Corpse")
        {
            Destroy(col.gameObject);
        }
        
        
    }
    
}
