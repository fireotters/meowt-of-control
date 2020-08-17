using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fizzle : MonoBehaviour
{

    void Start()
    {
        AudioSource audSource = GetComponent<AudioSource>();

        int randClipChoice = Random.Range(0, 2);
        if (randClipChoice == 0) audSource.clip = GameAssets.i.audEnemyHit1;
        else if (randClipChoice == 1) audSource.clip = GameAssets.i.audEnemyHit2;


        int randPitchChoice = Random.Range(0, 3);
        audSource.pitch = 0.6f + randPitchChoice * 0.1f;

        audSource.Play();
    }

}
