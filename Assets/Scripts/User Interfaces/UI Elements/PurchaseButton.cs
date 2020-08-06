using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{
    private GameObject _cancelOverlay, _timerOverlay;
    private Image timerCircle;
    private Button _btn;

    public float cooldown;
    private float _cooldownRemaining = 0;

    private void Start()
    {
        _cancelOverlay = transform.Find("CancelOverlay").gameObject;
        _timerOverlay = transform.Find("TimerOverlay").gameObject;
        timerCircle = _timerOverlay.transform.Find("TimerCircle").GetComponent<Image>();
        _btn = GetComponent<Button>();
    }

    private void Update()
    {
        if (_cooldownRemaining > 0)
        {
            _cooldownRemaining -= Time.deltaTime;
            timerCircle.fillAmount = _cooldownRemaining / cooldown;
        }
        if (_cooldownRemaining < 0)
        {
            _cooldownRemaining = 0;
            timerCircle.fillAmount = 0;
            HideTimerOverlay();
        }

    }

    public void ShowCancelOverlay() {
        _cancelOverlay.SetActive(true);
    }
    public void HideCancelOverlay()
    {
        _cancelOverlay.SetActive(false);
    }

    public void ShowTimerOverlay()
    {
        _timerOverlay.SetActive(true);
        _btn.interactable = false;
        _cooldownRemaining = cooldown;
    }

    private void HideTimerOverlay()
    {
        _timerOverlay.SetActive(false);
        _btn.interactable = true;
    }
}
