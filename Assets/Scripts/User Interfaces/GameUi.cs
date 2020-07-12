using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public partial class GameUi : BaseUi
{
    [Header("Game UI")]
    public MusicManager musicManager;
    public int choiceOfMusic;

    public GameObject gamePausePanel;
    public GameObject gameOverPanel;
    public GameManager gM;
    public GameObject player;

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

        // Initialise UI values
        UpdateCash(0);
        UpdateHealth(0);
    }

    void Update()
    {
        CheckKeyInputs();
        // if (player == null)
        // {
        //     gameOver(!gamePausePanel.activeInHierarchy);
        // }
        
    }
    private void CheckKeyInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Pause if pause panel isn't open, resume if it is open
            GameIsPaused(!gamePausePanel.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SwapFullscreen();
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

    // public void gameOver(bool intent)
    // {
    //     gameOverPanel.SetActive(intent);
    //     Time.timeScale = (intent == true) ? 0 : 1;
    // }

    // public void restart()
    // {
    //     SceneManager.LoadScene(2);
    //     Time.timeScale = 1;
    //     gamePausePanel.SetActive(false);
    // }

    public void ExitLevel()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
