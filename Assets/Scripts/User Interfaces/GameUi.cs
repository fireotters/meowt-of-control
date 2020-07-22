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
    private GameManager gM;
    public GameObject player;

    [Header("Unity Inspector Organisation")]
    public Transform dropsInPlayParent;

    private void Awake()
    {
        gM = FindObjectOfType<GameManager>();
        sprTowerInvalidArea = gM.placementBlockersParent.Find("RedArea").GetComponent<SpriteRenderer>();
        sprTowerRange = gM.placementBlockersParent.Find("TowerRangeArea").GetComponent<SpriteRenderer>();

        musicManager = FindObjectOfType<MusicManager>();
        if (!musicManager)
        {
            Instantiate(musicManagerIfNotFoundInScene);
            musicManager = FindObjectOfType<MusicManager>();
        }
    }

    void Start()
    {
        // Change music track
        musicManager.ChangeMusicTrack(choiceOfMusic);

        // Fade in the level
        StartCoroutine(FadeBlack("from"));

        // Initialise UI values
        UpdateMainTowerHealth(0);
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

    public void GameIsOverShowUi()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
    }

    public void ExitLevel()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
