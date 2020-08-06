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
    private bool gameOverButtonBlocked = false;

    [Header("Missile-Only Attributes")]
    public float missileButtonCooldown;
    [SerializeField] private bool isMissileButton = false;
    private bool inMissileCancelAnim = false;

    private void Start()
    {
        _cancelOverlay = transform.Find("CancelOverlay").gameObject;
        _timerOverlay = transform.Find("TimerOverlay").gameObject;
        timerCircle = _timerOverlay.transform.Find("TimerCircle").GetComponent<Image>();
        _btn = GetComponent<Button>();
    }

    private void Update()
    {
        // Each frame, reduce the cooldown if there is one
        if (_cooldownRemaining > 0)
        {
            _cooldownRemaining -= Time.deltaTime;
            // If button is for the missile, and in CancelAnimation cooldown, calculate timerCircle percentage differently
            timerCircle.fillAmount = _cooldownRemaining / (inMissileCancelAnim ? missileButtonCooldown : cooldown);
        }

        if (_cooldownRemaining < 0)
        {
            _cooldownRemaining = 0;
            timerCircle.fillAmount = 0;
            HideTimerOverlay();
        }

    }

    /// <summary>
    /// Show a cancel overlay on the button.<br/>
    /// - Called by GameUi.Sidebar to the currently selected button, to allow cancellation of purchase.
    /// </summary>
    public void ShowCancelOverlay() {
        _cancelOverlay.SetActive(true);
    }

    /// <summary>
    /// Hide the cancel overlay on the button. Called in three situations.<br/>
    /// - When GameUi.Sidebar refreshes all purchase button states<br/>
    /// - When GameManager completes a purchase or replaces a purchase decision<br/>
    /// </summary>
    public void HideCancelOverlay()
    {
        CheckIfCancellingMissile();
        _cancelOverlay.SetActive(false);
    }

    /// <summary>
    /// Begins a short cooldown upon cancelling the Missile purchase button.<br/>
    /// - Allows an animation to finish playing. Prevents anim mismatch if a missile is launched during Box's Cancel animation.
    /// </summary>
    private void CheckIfCancellingMissile()
    {
        // If missile is cancelled, put a cooldown of 2sec on the button
        if (isMissileButton && _cancelOverlay.activeInHierarchy == true)
        {
            inMissileCancelAnim = true;
            ShowTimerOverlay();
            _cooldownRemaining = missileButtonCooldown;
        }
    }

    /// <summary>
    /// Show a timer overlay on the button, prevents repeat purchases until a cooldown is complete.<br/>
    /// - Called by GameUi.Sidebar to the most recent purchase.
    /// </summary>
    public void ShowTimerOverlay()
    {
        _timerOverlay.SetActive(true);
        _cooldownRemaining = cooldown;
        _btn.interactable = false;
    }

    /// <summary>
    /// Cancel the timer overlay on the button. Purchase can be made again.<br/>
    /// - Doesn't re-enable button if game is over
    /// </summary>
    private void HideTimerOverlay()
    {
        _timerOverlay.SetActive(false);
        inMissileCancelAnim = false;
        if (!gameOverButtonBlocked)
        {
            _btn.interactable = true;
        }
    }

    public void BlockClicking()
    {
        _btn.interactable = false;
        gameOverButtonBlocked = true;
    }
}
