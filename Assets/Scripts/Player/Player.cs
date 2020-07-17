using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    internal int bulletsLeft = 6, currentPlayerHealth = 3;
    internal bool gunEnoughAmmo = true;
    [SerializeField] private GameObject[] bulletIndicators;

    private GameObject bulletIndicatorsPanel;
    private readonly int maxPlayerHealth = 3;
    private GameManager gM;
    private SoundManager soundManager;
    private float nextPlayerDmg = 0.0f, nextBulletReload = 0.0f;

    private void Start()
    {
        bulletIndicatorsPanel = transform.Find("BulletsRemaining").gameObject;
        gM = FindObjectOfType<GameManager>();
        soundManager = GetComponent<SoundManager>();
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
            IncreaseBulletsLeft();
        }
    }

    public void ReduceBulletsLeft()
    {
        ShowAmmoPanel();
        bulletsLeft--;
        bulletIndicators[bulletsLeft].SetActive(false);
        if (bulletsLeft == 0)
        {
            gunEnoughAmmo = false;
        }
    }

    public void IncreaseBulletsLeft()
    {
        if (bulletsLeft < 6 && Time.time > nextBulletReload)
        {
            nextBulletReload = Time.time + 0.5f;
            gunEnoughAmmo = true;

            ShowAmmoPanel();
            bulletsLeft++;
            bulletIndicators[bulletsLeft - 1].SetActive(true);
        }
    }

    public void ShowAmmoPanel()
    {
        CancelInvoke(nameof(HideAmmoPanel));
        Invoke(nameof(HideAmmoPanel), 1f);
        bulletIndicatorsPanel.SetActive(true);
    }
    private void HideAmmoPanel()
    {
        bulletIndicatorsPanel.SetActive(false);
    }

    public void DamagePlayer()
    {
        if (currentPlayerHealth != 0 && Time.time > nextPlayerDmg)
        {
            nextPlayerDmg = Time.time + 3f;
            soundManager.SoundPlayerHit();
            currentPlayerHealth--;
            gM.gameUi.UpdatePlayerHealth();
        }
    }

    public void PickupItem(string type)
    {
        if (type == "milk")
        {
            if (currentPlayerHealth < maxPlayerHealth)
            {
                currentPlayerHealth = maxPlayerHealth;
                gM.gameUi.UpdatePlayerHealth();
            }
            else if (currentPlayerHealth == maxPlayerHealth)
            {
                gM.HandleMilkPickup();
            }
        }
        else if (type == "yarn")
        {
            gM.gameUi.UpdateYarn(50);
        }
        else
        {
            Debug.LogError("GameManger.PickupItem: Invalid pickup type");
        }
    }
}
