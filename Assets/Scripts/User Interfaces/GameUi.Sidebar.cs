using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class GameUi : BaseUi
{
    [Header("Main Sidebar UI")]
    public Image roundIndicator;
    public TextMeshProUGUI textYarn, textRound;
    public PurchaseButton[] purchaseButtons;
    public GameObject buildModeTexts, launchModeTexts;

    [Header("Cat Health UI")]
    public TextMeshProUGUI textBoxHealth;
    public Sprite[] gunCatFaces, boxCatFaces;
    public Image gunCatFace, boxCatFace;
    public GameObject[] gunCatLives;

    // Health bar
    [SerializeField] private Transform _healthBar = default;
    private Vector2 _healthBarFullSize;
    private RectTransform _healthBarRect;

    internal void UpdateYarn(int difference)
    {
        _gM.currentYarn += difference;
        textYarn.text = _gM.currentYarn.ToString();
    }

    internal void UpdateBoxCatHealth(int difference)
    {
        _gM.mainTowerHealth += difference;
        if (_gM.mainTowerHealth < 0)
        {
            _gM.mainTowerHealth = 0;
        }

        int indexOfBoxCatFace;
        if (_gM.mainTowerHealth > 65) // High health
        {
            indexOfBoxCatFace = 3;
        }
        else if (_gM.mainTowerHealth > 35) // Mid health
        {
            indexOfBoxCatFace = 2;
        }
        else if (_gM.mainTowerHealth >0 ) // Low health
        {
            indexOfBoxCatFace = 1;
        }
        else // Dead
        {
            indexOfBoxCatFace = 0;
        }
        boxCatFace.sprite = boxCatFaces[indexOfBoxCatFace];
        textBoxHealth.text = _gM.mainTowerHealth + "%";

        float percentOfLifeLeft = (float)_gM.mainTowerHealth / (float)100;
        Vector2 scaleChange = _healthBarFullSize;
        scaleChange.x = percentOfLifeLeft * _healthBarFullSize.x;
        _healthBarRect.sizeDelta = scaleChange;
    }

    internal void UpdateRoundIndicator()
    {
        float roundProgressLeft = (float)_gM.enemyCount / (float)_gM.enemyMaxCount;
        roundIndicator.fillAmount = roundProgressLeft;
    }

    internal void UpdatePlayerHealth()
    {
        gunCatFace.sprite = gunCatFaces[_gM.player.currentPlayerHealth];
        // Disable all life icons, and re-enable ones that match player's health
        foreach (GameObject lifeIcon in gunCatLives)
        {
            lifeIcon.SetActive(false);
        }
        for (int i = 0; i < _gM.player.currentPlayerHealth; i++)
        {
            gunCatLives[i].SetActive(true);
        }
    }

    public void ClickedPurchaseButton(GameManager.PurchaseType whichPurchase)
    {
        int priceToCheck = 0;
        switch (whichPurchase) {
            case GameManager.PurchaseType.PillowTower:
                priceToCheck = _gM.pricePillow;
                break;
            case GameManager.PurchaseType.WaterTower:
                priceToCheck = _gM.priceWater;
                break;
            case GameManager.PurchaseType.FridgeTower:
                priceToCheck = _gM.priceFridge;
                break;
            case GameManager.PurchaseType.Missile:
                priceToCheck = _gM.priceMissile;
                break;
        }

        if (_gM.currentYarn >= priceToCheck)
        {
            _gM.SpawnPurchasedObject(whichPurchase);
        }
        else
        {
            textYarn.color = Color.red;
            textYarn.GetComponent<AudioSource>().Play();
            CancelInvoke(nameof(EndYarnRedFlash));
            Invoke(nameof(EndYarnRedFlash), 0.5f);
        }
    }

    public void PurchaseFromButton(string whichPurchase)
    {
        GameManager.PurchaseType purchaseType;
        if (Enum.TryParse(whichPurchase, true, out purchaseType))
        {
            ClickedPurchaseButton(purchaseType);
        }
        else
        {
            Debug.LogError("GameUi.Sidebar: PurchaseFromButton passed invalid string (" + whichPurchase + ")");
        }
    }

    public void BeginPurchaseCooldown(int whichPurchase)
    {
        purchaseButtons[whichPurchase].ShowTimerOverlay();
    }


    public void UpdateCancelOverlays(int whichTower)
    {
        foreach (PurchaseButton btn in purchaseButtons)
        {
            btn.HideCancelOverlay();
        }
        purchaseButtons[whichTower].ShowCancelOverlay();
    }

    public void BlockPurchaseUi()
    {
        foreach (PurchaseButton btn in purchaseButtons)
        {
            btn.BlockClicking();
        }
    }
    private void EndYarnRedFlash()
    {
        textYarn.color = Color.white;
    }
}
