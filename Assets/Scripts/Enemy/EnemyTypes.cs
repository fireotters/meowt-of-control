using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypes : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Big chungus ignores the collision with corpses
        if (collision.gameObject.tag == "Corpse")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(),  true);
            Debug.Log("aaaa");
        }


    }
}

