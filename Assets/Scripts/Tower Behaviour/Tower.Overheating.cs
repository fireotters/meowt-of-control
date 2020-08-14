using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Tower : MonoBehaviour
{
    /// <summary>
    /// Increase overheat by 'penaltyPerShot', divided 3 times to make the timer UI appear to tick up toward a value.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShotIncreasesOverheat()
    {
        for (int i = 0; i < 3; i++)
        {
            currentOverheat += penaltyPerShot / 3;
            yield return new WaitForSeconds(shootCadence / 10);
        }
    }

    private void UpdateTimerUi()
    {
        attachedTimerUi.UpdateValue(currentOverheat / maxOverheat);
        if (currentOverheat >= maxOverheat && !towerGone)
        {
            towerGone = true;
            DisableTurretPlayDestroySound();
        }
    }

    private void DisableTurretPlayDestroySound()
    {
        // Play destruction sound
        AudioSource audioSrc = GetComponent<AudioSource>();
        audioSrc.clip = GameAssets.i.audTowerDestroy;
        audioSrc.Play();

        // Set tower to not shoot, pretend to be gone
        _canShootAgain = 3f;
        transform.Find("tower").gameObject.SetActive(false);
        transform.Find("base").gameObject.SetActive(false);
        Destroy(attachedTimerUi.gameObject);
        Destroy(attachedPlacementBlocker);

        // Drop a differently coloured piece of scrap
        Instantiate(_attachedScrap, transform.position, Quaternion.identity, ObjectsInPlay.i.dropsParent);

        Invoke(nameof(DestroyTurret), 1f);
    }

    private void DestroyTurret()
    {
        Destroy(gameObject);
    }

    public void BigEnemyDestroysTower()
    {
        currentOverheat = maxOverheat;
    }

    public void EndOfRoundDestroyTurret()
    {
        Destroy(attachedTimerUi.gameObject);
        Destroy(attachedPlacementBlocker);
        Destroy(gameObject);
    }
}
