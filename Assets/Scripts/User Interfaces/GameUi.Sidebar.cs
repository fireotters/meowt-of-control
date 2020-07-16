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
    public GameObject buildModeTexts;


    internal void UpdateYarn(int difference)
    {
        gM.currentYarn += difference;
        textCash.text = gM.currentYarn.ToString();
    }

    internal void UpdateMainTowerHealth(int difference)
    {
        gM.mainTowerHealth += difference;
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

    public void Purchase(int whichPurchase)
    {
        int priceToCheck = 0;
        switch (whichPurchase) {
            case 0:
                priceToCheck = gM.pricePillow;
                break;
            case 1:
                priceToCheck = gM.priceWater;
                break;
            case 2:
                priceToCheck = gM.priceFridge;
                break;
        }

        if (gM.currentYarn >= priceToCheck)
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
