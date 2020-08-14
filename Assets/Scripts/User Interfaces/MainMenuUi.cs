using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
using System;

public class MainMenuUi : BaseUi
{
    // High Score display
    public TextMeshProUGUI highScoreNum, highScoreName;

    // Option Screen
    public GameObject optionsDialog;
    public Slider optionMusicSlider, optionSFXSlider;
    public TextMeshProUGUI txtFullscreenToggleBtn;

    // Audio
    public AudioMixer mixer;
    private MusicManager musicManager;
    public AudioSource sfxDemoSlider;

    //private DiscordManager discordManager;

    void Start()
    {
        /* Uncomment when Discord Manager is set up
        discordManager = FindObjectOfType<DiscordManager>();
        if (discordManager.UpdateDiscordRp(DiscordActivities.MainMenuActivity))
        {
            Debug.Log("Rich presence updated.");
        }*/
        musicManager = FindObjectOfType<MusicManager>();
        if (!musicManager)
        {
            Instantiate(musicManagerIfNotFoundInScene);
            musicManager = FindObjectOfType<MusicManager>();
        }
        if (musicManager)
        {
            musicManager.sfxDemo = optionSFXSlider.GetComponent<AudioSource>();
            musicManager.ChangeMusicTrack(0);
            musicManager.SetMixerVolumes();
        }
        // Set up PlayerPrefs when game is first ever loaded
        if (PlayerPrefs.GetInt("FirstLoad") != 1)
        {
            PlayerPrefs.SetInt("FirstLoad", 1);
            PlayerPrefs.SetFloat("Music", 0.5f);
            PlayerPrefs.SetFloat("SFX", 0.5f);
            PlayerPrefs.SetInt("HighscoreNum", 0);
            PlayerPrefs.SetString("HighscoreName", "No Highscore Yet");
        }
        // Fill in high score section and fade in from black
        FillHighScoreArea();
        StartCoroutine(FadeBlack(FadeType.FromBlack, fullUiFadeBlack));
    }

    private void FillHighScoreArea()
    {
        highScoreNum.text = "(Round " + PlayerPrefs.GetInt("HighscoreNum") + ")";
        highScoreName.text = PlayerPrefs.GetString("HighscoreName");
    }

    // Functions related to options menu
    public void OptionsOpen()
    {
        optionsDialog.SetActive(true);

        SetBtnFullscreenText();

        optionMusicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("Music"));
        optionSFXSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFX"));
    }

    public void OptionsClose()
    {
        optionsDialog.SetActive(false);

        optionMusicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("Music"));
        optionSFXSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFX"));
    }

    public override void SwapFullscreen()
    {
        base.SwapFullscreen();
        Invoke(nameof(SetBtnFullscreenText), 0.1f);
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighscoreNum", 0);
        PlayerPrefs.SetString("HighscoreName", "No Highscore Yet");
        SceneManager.LoadScene("MainMenu");
    }

    public void SetBtnFullscreenText()
    {
        if (Screen.fullScreen)
        {
            txtFullscreenToggleBtn.text = "Fullscreen ON";
        }
        else
        {
            txtFullscreenToggleBtn.text = "Fullscreen OFF";
        }
    }

    // Other functions
    public void OpenGame()
    {
        StartCoroutine(FadeBlack(FadeType.ToBlack, fullUiFadeBlack));
        Invoke(nameof(OpenGame2), 1f);
    }
    public void OpenGame2()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void DoAboutDevLoad()
    {
        SceneManager.LoadScene("HelpMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
