using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpMenuText : MonoBehaviour
{
    [TextArea(5,10)]
    public string helpTextVerbose, helpTextBrief;
    private TextMeshProUGUI helpText;
    [SerializeField] private HelpMenuUi helpMenuUi;

    private void Awake()
    {
        helpText = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        ChangeTextMode();
    }

    public void ChangeTextMode()
    {
        helpText.text = helpMenuUi.helpMenuVerbose ? helpTextVerbose : helpTextBrief;
    }
}
