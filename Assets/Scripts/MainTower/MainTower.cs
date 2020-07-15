using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTower : MonoBehaviour
{
    private GameManager _gameManager;
    float _curTime = 0;
    float nextDamage = 1;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (_curTime <= 0)
        {
            if (col.gameObject.tag == "LargeEnemy")
            {
                _gameManager.gameUi.UpdateHealth(-5);
            }
            else if(col.gameObject.tag != "Player")
            {
                _gameManager.gameUi.UpdateHealth(-1);
            }

           
            
            Debug.Log("Tower hit!" + _gameManager.currentHealth);

            _curTime = nextDamage;
        }
       
        else
        {
            _curTime -= Time.deltaTime;
        }

        if (_gameManager.currentHealth <= 0)
        {
            Lose();
        }

        if (_gameManager.currentHealth <= 25)
        {
            _gameManager.gameUi.musicManager.ChangeStressMode();
    }


    }

    private void Lose()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}
