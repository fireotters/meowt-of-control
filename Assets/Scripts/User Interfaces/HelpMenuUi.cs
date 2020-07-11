using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpMenuUi : BaseUi
{
    void Start()
    {
        StartCoroutine(FadeBlack("from"));
    }
    public void LeaveHelp()
    {
        StartCoroutine(FadeBlack("to"));
        Invoke(nameof(LeaveHelp2), 1f);
    }
    private void LeaveHelp2()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
