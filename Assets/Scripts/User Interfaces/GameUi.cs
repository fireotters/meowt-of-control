using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUi : BaseUi
{
    public MusicManager musicManager;
    public int choiceOfMusic;

    public GameObject gamePausePanel;

    void Start()
    {
        // Change music track
        musicManager = FindObjectOfType<MusicManager>();
        if (!musicManager)
        {
            Instantiate(musicManagerIfNotFoundInScene);
            musicManager = FindObjectOfType<MusicManager>();
        }
        musicManager.ChangeMusicTrack(choiceOfMusic);

        // Fade in the level
        StartCoroutine(FadeBlack("from"));
    }

    void Update()
    {
        CheckKeyInputs();
    }
    private void CheckKeyInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Pause if pause panel isn't open, resume if it is open
            GameIsPaused(!gamePausePanel.activeInHierarchy);
        }
    }

    public void GameIsPaused(bool intent)
    {
        // Show or hide pause panel and set timescale
        gamePausePanel.SetActive(intent);
        Time.timeScale = (intent == true) ? 0 : 1;

        // If music manager is present, pause or resume music/sfx
        if (musicManager != null)
        {
            musicManager.MusicIsPaused(intent);
            musicManager.FindAllSfxAndPlayPause(intent);
        }
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
    }

    public void ExitLevel()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
