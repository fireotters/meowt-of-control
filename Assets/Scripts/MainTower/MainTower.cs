﻿using System;
using UnityEngine;

public class MainTower : MonoBehaviour
{
    public MusicManager musicManager;
    private GameManager _gameManager;
    float _curTime = 0;
    float nextDamage = 1;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionStay2D()
    {
        if (_curTime <= 0)
        {
            _gameManager.gameUi.UpdateHealth(-1);
            
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
            musicManager.ChangeStressMode();
    }


    }

    private void Lose()
    {
        Destroy(gameObject);
    }
}
