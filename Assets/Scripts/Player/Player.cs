using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Tweakable Variables")]
    public int bulletsLeft = 6;
    public int currentPlayerHealth = 3;
    private readonly float reloadTimePerBullet = 0.1f;
    private readonly int maxBullets = 6, maxPlayerHealth = 3;
    // If maxBullets is changed, bullet indicators need to be added in Unity, and assigned to bulletIndicators[]

    [Header("Private Components / Other Variables")]
    private float nextPlayerDmg = 0.0f;
    private GameManager gM;
    private SoundManager soundManager;

    [Header("Weapon Variables")]
    [SerializeField] private GameObject[] bulletIndicators;
    public AudioClip audNoAmmo, audReload;
    [HideInInspector] public bool gunCanBeUsed = true;
    private bool gunIsReloading = false;
    private GameObject ammoPanel, ammoPanelNoAmmoOverlay;
    private AudioSource ammoPanelSounds;

    private void Start()
    {
        ammoPanel = transform.Find("BulletsRemaining").gameObject;
        gM = FindObjectOfType<GameManager>();
        soundManager = GetComponent<SoundManager>();
        ammoPanelSounds = ammoPanel.GetComponent<AudioSource>();
        ammoPanelNoAmmoOverlay = ammoPanel.transform.Find("NoShooting").gameObject;
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        // Hitting player damages them
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("LargeEnemy"))
        {
            DamagePlayer();
        }
        else if (col.gameObject.CompareTag("MainTower"))
        {
            AttemptReload();
        }
    }
    
    /// <summary>
    /// If gun has less than 'maxBullets' bullets, allow a reload. Cannot shoot until reload is over. Loads one bullet per 'reloadTimePerBullet' until full.
    /// </summary>
    private void AttemptReload()
    {
        if (bulletsLeft < maxBullets && !gunIsReloading)
        {
            gunIsReloading = true;
            gunCanBeUsed = false;
            ShowAmmoPanel();
            ammoPanelSounds.clip = audReload;
            ammoPanelSounds.Play();

            StartCoroutine(ReloadAction());
        }
    }

    private IEnumerator ReloadAction()
    {
        while (!gunCanBeUsed)
        {
            bulletsLeft++;
            bulletIndicators[bulletsLeft - 1].SetActive(true);
            yield return new WaitForSeconds(reloadTimePerBullet);
            if (bulletsLeft == 6)
            {
                gunCanBeUsed = true;
            }
        }
        gunIsReloading = false;
        ShowAmmoPanel();
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
            nextPlayerDmg = Time.time + 3f;
            soundManager.SoundPlayerHit();
            currentPlayerHealth--;
            gM.gameUi.UpdatePlayerHealth();
        }
    }
    
    /// <summary>
    /// If no bullets, gun cannot be shot. If shooting is attempted with no bullets, NoAmmo shows ammo panel and plays a sound effect.
    /// </summary>
    public void BulletWasShot()
    {
        ShowAmmoPanel();
        bulletsLeft--;
        bulletIndicators[bulletsLeft].SetActive(false);
        if (bulletsLeft == 0)
        {
            gunCanBeUsed = false;
        }
    }

    public void NoAmmo()
    {
        ShowAmmoPanel();
        if (!ammoPanelSounds.isPlaying && !gunIsReloading)
        {
            ammoPanelSounds.clip = audNoAmmo;
            ammoPanelSounds.Play();
        }
    }

    /// <summary>
    /// Function that handles picking up items.
    /// 
    /// If milk is picked up: heal the player, or send to GameManager (heal main tower or sell for yarn).
    /// If yarn is picked up: deposit 50 yarn.
    /// </summary>
    /// <param name="type">Item type</param>
    /// 

    public enum PickupType { Milk, Yarn }
    public void PickupItem(PickupType type)
    {
        switch (type)
        {
            case PickupType.Milk:
                if (currentPlayerHealth < maxPlayerHealth)
                {
                    currentPlayerHealth = maxPlayerHealth;
                    gM.gameUi.UpdatePlayerHealth();
                }
                else if (currentPlayerHealth == maxPlayerHealth)
                {
                    gM.HandleMilkPickup();
                }
                break;
            case PickupType.Yarn:
                gM.gameUi.UpdateYarn(50);
                break;
            default:
                Debug.LogError("GameManger.PickupItem: Invalid pickup type");
                break;
        }
    }

}
