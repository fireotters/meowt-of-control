using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{

    /// <summary>
    /// Player can be reduced by one health state every 3 seconds.
    /// </summary>
    private void DamagePlayer()
    {
        if (currentPlayerHealth != 0 && Time.time > nextPlayerDmg)
        {
            nextPlayerDmg = Time.time + playerDmgInterval;
            _soundManager.SoundPlayerHit();
            currentPlayerHealth--;
            _gM.gameUi.UpdatePlayerHealth();
            if (currentPlayerHealth == 0)
            {
                PlayerIsDead();
            }
            else
            {
                StartCoroutine(BlinkDamage());
            }
        }
    }

    private IEnumerator BlinkDamage()
    {
        float elapsedTime = 0f;
        float blinkInterval = 0.2f;
        float fastBlinkInterval = blinkInterval / 4f;

        SpriteRenderer[] ArrayOfRenderers = GetComponentsInChildren<SpriteRenderer>();

        while (elapsedTime < playerDmgInterval)
        {
            // When a third of the invuln timer remains, blink much faster
            if (elapsedTime >= (playerDmgInterval / 3f) * 2)
            {
                blinkInterval = fastBlinkInterval;
            }

            SetSpriteAlphas(ArrayOfRenderers, 0.5f);
            yield return new WaitForSeconds(blinkInterval);

            SetSpriteAlphas(ArrayOfRenderers, 0.8f);
            yield return new WaitForSeconds(blinkInterval);

            elapsedTime += blinkInterval * 2;
        }
        SetSpriteAlphas(ArrayOfRenderers, 1f);
    }
    private void SetSpriteAlphas(SpriteRenderer[] inputArray, float desiredAlpha)
    {
        Color origColor;
        foreach (SpriteRenderer r in inputArray)
        {
            origColor = r.color;
            r.color = new Color(origColor.r, origColor.g, origColor.b, desiredAlpha);
        }
    }

    public void HealPlayer()
    {
        if (currentPlayerHealth < maxPlayerHealth)
        {
            currentPlayerHealth++;
            _gM.gameUi.UpdatePlayerHealth();
        }
    }

    public void PlayerIsDead()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        _playerController.enabled = false;
        _spriteAnimator.SetBool("Dying", true);
        _gM.GameIsOverPlayEndScene();
    }
}
