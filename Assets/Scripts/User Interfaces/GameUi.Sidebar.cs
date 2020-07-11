﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class GameUi : BaseUi
{
    [Header("Purchasing UI")]
    public TextMeshProUGUI textCash;
    public TextMeshProUGUI textHealth, textRound;
    public PurchaseButton[] purchaseButtons;
    public Image roundIndicator;


    internal void UpdateCash(int difference)
    {
        gM.currentCash += difference;
        textCash.text = gM.currentCash.ToString();
    }

    internal void UpdateHealth(int difference)
    {
        gM.currentHealth += difference;
        textHealth.text = gM.currentHealth + "%";
    }

    internal void UpdateRoundIndicator()
    {
        float roundProgressLeft = (float)gM.enemyCount / (float)gM.enemyMaxCount;
        roundIndicator.fillAmount = roundProgressLeft;
    }

    public void Purchase(int whichPurchase)
    {
        int priceToCheck = 0;
        switch (whichPurchase) {
            case 0:
                priceToCheck = 100;
                break;
            case 1:
                priceToCheck = 200;
                break;
            case 2:
                priceToCheck = 300;
                break;
        }

        if (gM.currentCash >= priceToCheck)
        {
            gM.SpawnPlaceableTower(whichPurchase);
        }
        else
        {
            // TODO Play failure noise, blink price indicator
        }
    }

    public void UpdateCancelOverlays(int whichTower)
    {
        foreach (PurchaseButton btn in purchaseButtons)
        {
            btn.HideCancelOverlay();
        }
        purchaseButtons[whichTower].ShowCancelOverlay();
    }
}