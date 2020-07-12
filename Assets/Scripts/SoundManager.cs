using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    public AudioSource soundSource;
    public AudioClip hoverSound, selectSound, destroyTurret, enemyImpact1, enemyImpact2, catCannon, playerHit;
    public AudioMixer mixer;

    public void HoverButton()
    {
        soundSource.clip = hoverSound;
        soundSource.Play();
    }
    public void SelectButton()
    {
        soundSource.clip = selectSound;
        soundSource.Play();
    }

    public void SoundDestroyTurret()
    {
        soundSource.clip = destroyTurret;
        soundSource.Play();
    }
    
    public void SoundEnemyImpact()
    {
        if (Random.Range(0, 2) == 1)
            soundSource.clip = enemyImpact1;
        else 
            soundSource.clip = enemyImpact2;

        soundSource.Play();
    }
    
    public void SoundCatCannon()
    {
        soundSource.clip = catCannon;
        soundSource.Play();
    }
    
    public void SoundPlayerHit()
    {
        soundSource.clip = playerHit;
        soundSource.Play();
    }
}
