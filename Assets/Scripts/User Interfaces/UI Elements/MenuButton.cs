using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public Image buttonImage;
    public Sprite normSprite, hoverSprite;

    public void IsHovering(bool yesOrNo)
    {
        buttonImage.sprite = yesOrNo == true ? hoverSprite : normSprite;
    }
}
