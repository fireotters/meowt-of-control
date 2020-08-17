using System.Collections;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public abstract class BaseUi : MonoBehaviour
{
    [Header("Base UI")]
    public GameObject fullUiFadeBlack;
    public GameObject musicManagerIfNotFoundInScene;


    /* ------------------------------------------------------------------------------------------------------------------
     * Fading Effect - UI fades to or from black. The 'from' transition can be interrupted by 'to' transition.
     * ------------------------------------------------------------------------------------------------------------------ */
    private bool fadingInAlready = false;
    private float fadingAlpha = 0f;
    internal enum FadeType { ToBlack, FromBlack }
    internal IEnumerator FadeBlack(FadeType type, GameObject objectToFade, float delay = 0f)
    {
        objectToFade.SetActive(true);
        yield return new WaitForSeconds(delay);

        // If 'fade to' is trigger during 'fade from', this variable stops the while loop that does 'fade from'
        if (!fadingInAlready)
        {
            fadingInAlready = true;
            fadingAlpha = 0f;
        }

        Image tempFade = objectToFade.GetComponent<Image>();
        Color origColor = tempFade.color;
        float speedOfFade = 1.2f;

        if (type == FadeType.FromBlack)
        {
            fadingAlpha = 1f;
            while (fadingAlpha > 0f && fadingInAlready)
            {
                fadingAlpha -= speedOfFade * Time.deltaTime;
                tempFade.color = new Color(origColor.r, origColor.g, origColor.b, fadingAlpha);
                yield return null;
            }
            objectToFade.SetActive(false);
            fadingInAlready = false;
        }

        else if (type == FadeType.ToBlack)
        {
            while (fadingAlpha < 1f)
            {
                fadingAlpha += speedOfFade * Time.deltaTime;
                tempFade.color = new Color(origColor.r, origColor.g, origColor.b, fadingAlpha);
                yield return null;
            }
        }
        yield return new WaitForSeconds(1);
    }

    public virtual void SwapFullscreen()
    {
        print("Fullscreen Toggled");
        if (Screen.fullScreen)
        {
            Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, false);
        }
        else
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
    }
}