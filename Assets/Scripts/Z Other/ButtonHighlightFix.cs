using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Fixes having two buttons highlighted if mouse and keyboard/controller input is used at the same time.
// Code credit to 'daterre' on Unity forums
// https://forum.unity.com/threads/button-keyboard-and-mouse-highlighting.294147/

[RequireComponent(typeof(Selectable))]
public class ButtonHighlightFix : MonoBehaviour, IPointerEnterHandler, IDeselectHandler {
    public void OnPointerEnter(PointerEventData eventData) {
        if (!EventSystem.current.alreadySelecting)
            EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData) {
        this.GetComponent<Selectable>().OnPointerExit(null);
    }
}
