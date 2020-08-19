using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameUi : BaseUi
{
    [Header("Game UI")]
    public MusicManager musicManager;

    public GameObject gamePausePanel;
    public GameObject gameOverPanel;
    private GameManager _gM;
    public GameObject player;

    private void Awake()
    {
        _gM = ObjectsInPlay.i.gameManager;
        sprTowerInvalidArea = ObjectsInPlay.i.placementBlockersParent.Find("RedArea").GetComponent<SpriteRenderer>();
        sprTowerRange = ObjectsInPlay.i.placementBlockersParent.Find("TowerRangeArea").GetComponent<SpriteRenderer>();

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
        musicManager.ChangeMusicTrack(1);
        musicManager.SetMixerVolumes();

        // Fade in the level
        StartCoroutine(FadeBlack(FadeType.FromBlack, fullUiFadeBlack));

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
        // If the purchase button is interactible, then allow binds to click purchase buttons
        if (Input.GetKeyDown(KeyCode.Alpha1) && purchaseButtons[0].btn.interactable)
        {
            ClickedPurchaseButton(GameManager.PurchaseType.PillowTower);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && purchaseButtons[1].btn.interactable)
        {
            ClickedPurchaseButton(GameManager.PurchaseType.WaterTower);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && purchaseButtons[2].btn.interactable)
        {
            ClickedPurchaseButton(GameManager.PurchaseType.FridgeTower);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && purchaseButtons[3].btn.interactable)
        {
            ClickedPurchaseButton(GameManager.PurchaseType.Missile);
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
