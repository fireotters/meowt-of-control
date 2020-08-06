using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class GameUi : BaseUi
{
    [Header("Purchasing UI")]
    public TextMeshProUGUI textCash;
    public TextMeshProUGUI textHealth, textRound, textCatHealth;
    public PurchaseButton[] purchaseButtons;
    public Sprite[] catFaces;
    public Image roundIndicator, catFace;
    public GameObject buildModeTexts, launchModeTexts;


    internal void UpdateYarn(int difference)
    {
        gM.currentYarn += difference;
        textCash.text = gM.currentYarn.ToString();
    }

    internal void UpdateMainTowerHealth(int difference)
    {
        gM.mainTowerHealth += difference;
        if (gM.mainTowerHealth < 0)
        {
            gM.mainTowerHealth = 0;
        }
        textHealth.text = gM.mainTowerHealth + "%";
    }

    internal void UpdateRoundIndicator()
    {
        float roundProgressLeft = (float)gM.enemyCount / (float)gM.enemyMaxCount;
        roundIndicator.fillAmount = roundProgressLeft;
    }

    internal void UpdatePlayerHealth()
    {
        switch (gM.player.currentPlayerHealth)
        {
            case 3:
                catFace.sprite = catFaces[0];
                break;
            case 2:
                catFace.sprite = catFaces[2];
                break;
            case 1:
                catFace.sprite = catFaces[5];
                break;
            case 0:
                catFace.sprite = catFaces[6];
                break;
        }
        textCatHealth.text = gM.player.currentPlayerHealth.ToString();
    }

    public void ClickedPurchaseButton(GameManager.PurchaseType whichPurchase)
    {
        int priceToCheck = 0;
        switch (whichPurchase) {
            case GameManager.PurchaseType.PillowTower:
                priceToCheck = gM.pricePillow;
                break;
            case GameManager.PurchaseType.WaterTower:
                priceToCheck = gM.priceWater;
                break;
            case GameManager.PurchaseType.FridgeTower:
                priceToCheck = gM.priceFridge;
                break;
            case GameManager.PurchaseType.Missile:
                priceToCheck = gM.priceMissile;
                break;
        }

        if (gM.currentYarn >= priceToCheck)
        {
            gM.SpawnPurchasedObject(whichPurchase);
        }
        else
        {
            // TODO Play failure noise, blink price indicator
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
}
