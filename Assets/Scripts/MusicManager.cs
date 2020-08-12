using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
    {

    public AudioMixer mixer;
    public AudioSource sfxDemo, currentMusicPlayer, currentMusicPlayerDrums;
    public AudioClip musicMainMenu, stageMusicDrums, stageMusic, stageGameOver;
    public AudioClip hoverButton, selectButton;
    public float stressFadeSpeed = 0.08f;
    public float maxPitch = 1.1f;
    private bool stressMode;
    private int lastTrackRequested = -1; // When first created, pick the scene's chosen song

    public static MusicManager instance;
    public void ChangeMusic(float sliderValue) {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("Music", sliderValue);
    }

    public void ChangeSFX(float sliderValue) {
        mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SFX", sliderValue);
        if (!sfxDemo.isPlaying) {
            sfxDemo.Play();
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetMixerVolumes()
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("Music")) * 20);
        mixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFX")) * 20);
    }

    public void ChangeStressMode()
    {
        if (currentMusicPlayer.clip != stageGameOver)
        {
            stressMode = true;
        }
    }

    public void ExitStressMode()
    {
        stressMode = false;
    }

    public void ChangeToGameOverMusic()
    {
        stressMode = false;
        currentMusicPlayer.clip = stageGameOver;
        currentMusicPlayer.pitch = 1;
        currentMusicPlayer.Play();
        currentMusicPlayer.loop = false;
        currentMusicPlayerDrums.pitch = 1;
        currentMusicPlayerDrums.Stop();
    }

    public void ResetStageMusicOnRetry()
    {
        currentMusicPlayer.Stop();
        currentMusicPlayerDrums.Stop();
    }

    public void FindAllSfxAndPlayPause(bool intent)
    {
        List<GameObject> listOfSfxObjects = new List<GameObject>();
        // TODO List objects used in this certain game

        if (intent == true) // Pause
        {
            foreach (GameObject sfxObject in listOfSfxObjects)
            {
                if (sfxObject.GetComponent<AudioSource>().isPlaying)
                {
                    sfxObject.GetComponent<AudioSource>().Pause();
                }
            }
        }
        if (intent == false) // Resume
        {
            foreach (GameObject sfxObject in listOfSfxObjects)
            {
                if (!sfxObject.GetComponent<AudioSource>().isPlaying)
                {
                    sfxObject.GetComponent<AudioSource>().UnPause();
                }
            }
        }
    }

    public void ButtonHoverSound()
    {
        // sfxDemo.clip = 
    }
    
    private void Update()
    {
        if (stressMode && currentMusicPlayer.pitch < maxPitch)
        {
            currentMusicPlayer.pitch += Time.deltaTime * stressFadeSpeed;
            currentMusicPlayerDrums.pitch += Time.deltaTime * stressFadeSpeed;
        }
        else if (!stressMode && currentMusicPlayer.pitch > 1)
        {
            currentMusicPlayer.pitch -= Time.deltaTime * stressFadeSpeed;
            currentMusicPlayerDrums.pitch -= Time.deltaTime * stressFadeSpeed;
        }
    }

    public void ChangeMusicTrack(int index)
    {
        Debug.Log($"Music requested: {index} Music last played: {lastTrackRequested}");

        // If the new track does not equal current track, reset player
        if (index != lastTrackRequested)
        {
            currentMusicPlayer.enabled = true;
            if (currentMusicPlayer.isPlaying)
            {
                currentMusicPlayer.Stop();
            }
        }

        // Play main menu music and stop drums
        if (index == 0 && currentMusicPlayer.clip != musicMainMenu)
        {
            currentMusicPlayer.clip = musicMainMenu;
            currentMusicPlayer.Play();
            currentMusicPlayerDrums.Stop();
        }
        // Play stage music and start drums
        else if (index == 1)
        {
            currentMusicPlayer.clip = stageMusic;
            currentMusicPlayerDrums.clip = stageMusicDrums;
            currentMusicPlayer.Play();
            currentMusicPlayerDrums.Play();
        }
        // Play nothing
        if (lastTrackRequested == -2) { }

        lastTrackRequested = index;
    }

    public void MusicIsPaused(bool intent)
    {
        if (intent == true && currentMusicPlayer.isPlaying)
        {
            currentMusicPlayer.Pause();
            currentMusicPlayerDrums.Pause();
        }
        if (intent == false && !currentMusicPlayer.isPlaying)
        {
            currentMusicPlayer.UnPause();
            currentMusicPlayerDrums.UnPause();
        }
    }

}
