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
    private GameManager _gM;
    public GameObject player;

    [Header("Unity Inspector Organisation")]
    public Transform dropsInPlayParent;

    private void Awake()
    {
        _gM = FindObjectOfType<GameManager>();
        sprTowerInvalidArea = _gM.placementBlockersParent.Find("RedArea").GetComponent<SpriteRenderer>();
        sprTowerRange = _gM.placementBlockersParent.Find("TowerRangeArea").GetComponent<SpriteRenderer>();

        musicManager = FindObjectOfType<MusicManager>();
        if (!musicManager)
        {
            Instantiate(musicManagerIfNotFoundInScene);
            musicManager = FindObjectOfType<MusicManager>();
        }
    }

    private void Start()
    {
        // Change music track
        musicManager.ChangeMusicTrack(choiceOfMusic);
        musicManager.SetMixerVolumes();

        // Fade in the level
        StartCoroutine(FadeBlack("from"));

        // Initialise UI values
        _healthBarRect = _healthBar.GetComponent<RectTransform>();
        _healthBarFullSize = _healthBarRect.sizeDelta;
        UpdateBoxCatHealth(0);
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
        if (!_gM.gameIsOver)
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
    }

    public void ExitGameFromPause()
    {
        if (IsThisAHighScore(_gM.currentRound))
        {
            GameIsOverShowUi();
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
            Time.timeScale = 1;
        }
    }
}
