using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerTimerUi : MonoBehaviour
{
    private static Vector3 timerUiOffset = new Vector2(0f, -0.5f);
    private Image progressFill;

    public static TowerTimerUi Create(Vector3 position)
    {
        Transform towerTimerUiTransform = Instantiate(GameAssets.i.pfTowerOverheatTimer, position, Quaternion.identity, ObjectsInPlay.i.timerUiParent).transform;
        towerTimerUiTransform.position += timerUiOffset;

        TowerTimerUi towerTimerUi = towerTimerUiTransform.GetComponent<TowerTimerUi>();
        return towerTimerUi;
    }

    private void Awake()
    {
        progressFill = transform.Find("Fill").GetComponent<Image>();
    }

    public void UpdateValue(float progress)
    {
        progressFill.fillAmount = progress;
    }
}
