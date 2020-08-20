using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    [Header("Weapon Variables")]
    [SerializeField] private GameObject[] bulletIndicators = default;
    public AudioClip audReload;
    [HideInInspector] public bool gunCanBeUsed = true;
    private bool gunIsReloading = false;
    private GameObject ammoPanel, ammoPanelNoAmmoOverlay;
    private AudioSource ammoPanelSounds;

    private void FindComponentsPistol()
    {
        ammoPanel = transform.Find("BulletsRemaining").gameObject;
        ammoPanelSounds = ammoPanel.GetComponent<AudioSource>();
        ammoPanelNoAmmoOverlay = ammoPanel.transform.Find("NoShooting").gameObject;
    }

    /// <summary>
    /// If gun has less than 'maxBullets' bullets, allow a reload. Cannot shoot until reload is over. Loads one bullet per 'reloadTimePerBullet' until full.
    /// </summary>
    private void AttemptReload()
    {
        if (bulletsLeft < pistolMaxBullets && !gunIsReloading)
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
        float reloadSpeed = 1.5f / pistolReloadTime;
        _spriteAnimator.SetFloat("ReloadingSpeed", reloadSpeed);

        while (!gunCanBeUsed)
        {
            yield return new WaitForSeconds(pistolReloadTime / pistolMaxBullets);
            bulletsLeft++;
            bulletIndicators[bulletsLeft - 1].SetActive(true);
            if (bulletsLeft == pistolMaxBullets)
            {
                gunCanBeUsed = true;
            }
            ShowAmmoPanel();
        }
        gunIsReloading = false;
        ShowAmmoPanel();
        _spriteAnimator.SetBool("Reloading", false);
        _spriteAnimator.SetFloat("ReloadingSpeed", 1);
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
}
