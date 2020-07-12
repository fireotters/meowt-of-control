using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{
    private GameObject cancelOverlay;

    private void Start()
    {
        cancelOverlay = transform.Find("CancelOverlay").gameObject;
    }
    public void ShowCancelOverlay() {
        cancelOverlay.SetActive(true);
    }
    public void HideCancelOverlay()
    {
        cancelOverlay.SetActive(false);
    }
}
