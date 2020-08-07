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
    public TextMeshProUGUI textCash, textRound;
    public PurchaseButton[] purchaseButtons;
    public GameObject buildModeTexts, launchModeTexts;

    [Header("Cat Health UI")]
    public TextMeshProUGUI textBoxHealth;
    public TextMeshProUGUI textCatHealth;
    public Sprite[] gunCatFaces, boxCatFaces;
    public Image gunCatFace, boxCatFace;


    internal void UpdateYarn(int difference)
    {
        gM.currentYarn += difference;
        textCash.text = gM.currentYarn.ToString();
    }

    internal void UpdateBoxCatHealth(int difference)
    {
        gM.mainTowerHealth += difference;
        if (gM.mainTowerHealth < 0)
        {
            gM.mainTowerHealth = 0;
        }

        int indexOfBoxCatFace;
        if (gM.mainTowerHealth > 65) // High health
        {
            indexOfBoxCatFace = 3;
        }
        else if (gM.mainTowerHealth > 35) // Mid health
        {
            indexOfBoxCatFace = 2;
        }
        else if (gM.mainTowerHealth >0 ) // Low health
        {
            indexOfBoxCatFace = 1;
        }
        else // Dead
        {
            indexOfBoxCatFace = 0;
        }
        boxCatFace.sprite = boxCatFaces[indexOfBoxCatFace];
        textBoxHealth.text = gM.mainTowerHealth + "%";
    }

    internal void UpdateRoundIndicator()
    {
        float roundProgressLeft = (float)gM.enemyCount / (float)gM.enemyMaxCount;
        roundIndicator.fillAmount = roundProgressLeft;
    }

    internal void UpdatePlayerHealth()
    {
        gunCatFace.sprite = gunCatFaces[gM.player.currentPlayerHealth];
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
