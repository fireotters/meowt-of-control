using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class MainMenuUi : BaseUi
{
    // Option Screen
    public GameObject optionsDialog;
    public Slider optionMusicSlider, optionSFXSlider;
    public TextMeshProUGUI txtFullscreenToggleBtn;

    // Audio
    public AudioMixer mixer;
    private MusicManager musicManager;
    public AudioSource sfxDemoSlider;

    private DiscordManager discordManager;

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
        }
        if (!PlayerPrefs.HasKey("Music") || !PlayerPrefs.HasKey("SFX"))
        {
            PlayerPrefs.SetFloat("Music", 0.5f);
            PlayerPrefs.SetFloat("SFX", 0.5f);
        }
        mixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("Music")) * 20);
        mixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFX")) * 20);
        StartCoroutine(FadeBlack("from"));
    }

    void Update()
    {

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

    public void SwapFullscreen()
    {
        if (Screen.fullScreen)
        {
            Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, false);
        }
        else
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
        Invoke(nameof(SetBtnFullscreenText), 0.1f);
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
        SceneManager.LoadScene("Level1");
        /*if (discordManager.UpdateDiscordRp(DiscordActivities.StartGameActivity()))
        {
            Debug.Log("Rich presence has been updated.");
        }*/
    }

    public void DoLevelLoad()
    {
        SceneManager.LoadScene("Level" + ""); //TODO Code for level select
    }
}
