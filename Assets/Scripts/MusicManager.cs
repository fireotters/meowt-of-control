using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
    {

    public AudioMixer mixer;
    public AudioSource sfxDemo, currentMusicPlayer;
    public AudioClip musicMainMenu, musicTrack1;
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

    public void ChangeMusicTrack(int index)
    {
        Debug.Log($"Music requested: {index} Music last played: {lastTrackRequested}");

        // If lastTrackRequested = -2, then play nothing
        if (lastTrackRequested == -2) { }

        // If the new track does not equal current track, replace the track
        // If it does, then ignore this and continue previous track
        // If both old and new are 0, then restart playback anyway
        else if (index != lastTrackRequested || index == 0)
        {
            currentMusicPlayer.enabled = true;
            if (currentMusicPlayer.isPlaying)
            {
                currentMusicPlayer.Stop();
            }
            switch (index)
            {
                case 0:
                    currentMusicPlayer.clip = musicMainMenu;
                    break;

                case 1:
                    currentMusicPlayer.clip = musicTrack1;
                    break;
            }
            currentMusicPlayer.Play();
            lastTrackRequested = index;
        }
    }

    public void MusicIsPaused(bool intent)
    {
        if (intent == true && currentMusicPlayer.isPlaying)
        {
            currentMusicPlayer.Pause();
        }
        if (intent == false && !currentMusicPlayer.isPlaying)
        {
            currentMusicPlayer.UnPause();
        }
    }

}
