using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Tweakable Variables")]
    public int bulletsLeft = 6;
    public int currentPlayerHealth = 3;
    private readonly float reloadTimePerBullet = 0.25f;
    private readonly int maxBullets = 6, maxPlayerHealth = 3;
    // If maxBullets is changed, bullet indicators need to be added in Unity, and assigned to bulletIndicators[]

    [Header("Private Components / Other Variables")]
    private float nextPlayerDmg = 0.0f;
    private const float playerDmgInterval = 2f;
    private GameManager _gM;
    private SoundManager _soundManager;
    private Animator _spriteAnimator;
    private PlayerController _playerController;

    [Header("Weapon Variables")]
    [SerializeField] private GameObject[] bulletIndicators = default;
    public AudioClip audReload;
    [HideInInspector] public bool gunCanBeUsed = true;
    private bool gunIsReloading = false;
    private GameObject ammoPanel, ammoPanelNoAmmoOverlay;
    private AudioSource ammoPanelSounds;

    private void Start()
    {
        ammoPanel = transform.Find("BulletsRemaining").gameObject;
        _gM = FindObjectOfType<GameManager>();
        _soundManager = GetComponent<SoundManager>();
        ammoPanelSounds = ammoPanel.GetComponent<AudioSource>();
        ammoPanelNoAmmoOverlay = ammoPanel.transform.Find("NoShooting").gameObject;
        _spriteAnimator = transform.GetChild(2).GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        // Hitting player damages them
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("LargeEnemy"))
        {
            DamagePlayer();
        }
        else if (col.gameObject.CompareTag("Scrap") && Input.GetKey(KeyCode.LeftShift))
        {
            if (_gM.currentYarn >= 1)
            {
                Destroy(col.gameObject);
                _gM.gameUi.UpdateYarn(-1);
            }
            else
            {
                if (_gM.gameUi.textYarn.color == Color.white)
                {
                    _gM.gameUi.BeginYarnRedFlash();
                }
            }
        }
        //else if (col.gameObject.CompareTag("MainTower"))
        //{
        //    AttemptReload();
        //}
    }
    
    /// <summary>
    /// If gun has less than 'maxBullets' bullets, allow a reload. Cannot shoot until reload is over. Loads one bullet per 'reloadTimePerBullet' until full.
    /// </summary>
    private void AttemptReload()
    {
        if (bulletsLeft < maxBullets && !gunIsReloading)
        {
            // Tell the game that gun is being reloaded
            gunIsReloading = true;
            gunCanBeUsed = false;

            // Reset bullet count to 0 each time gun is reloaded
            bulletsLeft = 0;
            foreach (GameObject bulletIndicator in bulletIndicators)
            {
                bulletIndicator.SetActive(false);
            }

            // Show ammo panel, play sounds, begin reload coroutine
            ShowAmmoPanel();
            ammoPanelSounds.clip = audReload;
            ammoPanelSounds.Play();
            StartCoroutine(ReloadAction());
        }
    }

    private IEnumerator ReloadAction()
    {
        _spriteAnimator.SetBool("Reloading", true);
        while (!gunCanBeUsed)
        {
            yield return new WaitForSeconds(reloadTimePerBullet);
            bulletsLeft++;
            bulletIndicators[bulletsLeft - 1].SetActive(true);
            if (bulletsLeft == 6)
            {
                gunCanBeUsed = true;
            }
            ShowAmmoPanel();
        }
        gunIsReloading = false;
        ShowAmmoPanel();
        _spriteAnimator.SetBool("Reloading", false);
    }
    
    /// <summary>
    /// Shown when player shoots / attempts to shoot. Hidden 1 second after last request to show it.
    /// </summary>
    private void ShowAmmoPanel()
    {
        CancelInvoke(nameof(HideAmmoPanel));
        Invoke(nameof(HideAmmoPanel), 1f);
        ammoPanel.SetActive(true);
        if (!gunCanBeUsed || gunIsReloading)
        {
            ammoPanelNoAmmoOverlay.SetActive(true);
        }
        else
        {
            ammoPanelNoAmmoOverlay.SetActive(false);
        }
    }
    private void HideAmmoPanel()
    {
        ammoPanel.SetActive(false);
    }
    
    /// <summary>
    /// Player can be reduced by one health state every 3 seconds.
    /// </summary>
    private void DamagePlayer()
    {
        if (currentPlayerHealth != 0 && Time.time > nextPlayerDmg)
        {
            nextPlayerDmg = Time.time + playerDmgInterval;
            _soundManager.SoundPlayerHit();
            currentPlayerHealth--;
            _gM.gameUi.UpdatePlayerHealth();
            if (currentPlayerHealth == 0)
            {
                PlayerIsDead();
                _gM.GameIsOverPlayEndScene();
            }
            else
            {
                StartCoroutine(BlinkDamage());
            }
        }
    }

    private IEnumerator BlinkDamage()
    {
        float elapsedTime = 0f;
        float blinkInterval = 0.2f;
        float fastBlinkInterval = blinkInterval / 4f;

        SpriteRenderer[] ArrayOfRenderers = GetComponentsInChildren<SpriteRenderer>();

        while (elapsedTime < playerDmgInterval)
        {
            // When a third of the invuln timer remains, blink much faster
            if (elapsedTime >= (playerDmgInterval / 3f) * 2)
            {
                blinkInterval = fastBlinkInterval;
            }

            SetSpriteAlphas(ArrayOfRenderers, 0.5f);
            yield return new WaitForSeconds(blinkInterval);

            SetSpriteAlphas(ArrayOfRenderers, 0.8f);
            yield return new WaitForSeconds(blinkInterval);

            elapsedTime += blinkInterval * 2;
        }
        SetSpriteAlphas(ArrayOfRenderers, 1f);
    }
    private void SetSpriteAlphas(SpriteRenderer[] inputArray, float desiredAlpha)
    {
        Color origColor;
        foreach (SpriteRenderer r in inputArray)
        {
            origColor = r.color;
            r.color = new Color(origColor.r, origColor.g, origColor.b, desiredAlpha);
        }
    }

    
    /// <summary>
    /// If no bullets, gun cannot be shot. 
    /// </summary>
    public void BulletWasShot()
    {
        ShowAmmoPanel();
        bulletsLeft--;
        bulletIndicators[bulletsLeft].SetActive(false);
        if (bulletsLeft == 0)
        {
            gunCanBeUsed = false;
            AttemptReload();
        }
    }

    public void HealPlayer()
    {
        if (currentPlayerHealth < maxPlayerHealth)
        {
            currentPlayerHealth++;
            _gM.gameUi.UpdatePlayerHealth();
        }
    }

    public void PlayerIsDead()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        _playerController.enabled = false;
        _spriteAnimator.SetBool("Dying", true);
        _gM.GameIsOverPlayEndScene();
    }

}
