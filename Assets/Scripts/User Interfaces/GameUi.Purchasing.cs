using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class GameUi : BaseUi
{
    [Header("Purchasing UI")]
    public TextMeshProUGUI textCash;
    public TextMeshProUGUI textHealth;

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

    public void Purchase(int whichPurchase)
    {
        switch (whichPurchase) {
            case 0:
                UpdateCash(-100);
                break;
            case 1:
                UpdateCash(-200);
                break;
            case 2:
                UpdateCash(-300);
                break;
            case 3:
                UpdateCash(-400);
                break;
        }
    }
}
