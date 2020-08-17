using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpMenuUi : BaseUi
{
    private int currentPage = 0;
    public GameObject firstPage, secondPage, thirdPage;
    void Start()
    {
        StartCoroutine(FadeBlack(FadeType.FromBlack, fullUiFadeBlack));
    }

    public void UpdatePage(int diff)
    {
        currentPage += diff;
        firstPage.SetActive(currentPage == 0);
        secondPage.SetActive(currentPage == 1);
        thirdPage.SetActive(currentPage == 2);
    }

    public void VisitSite(string who)
    {
        switch (who)
        {
            case "Alfa":
                Application.OpenURL("https://alfaleon.itch.io/");
                break;
            case "Benchi":
                Application.OpenURL("https://benchi99.itch.io/");
                break;
            case "Cross":
                Application.OpenURL("https://crossfirecam.itch.io/");
                break;
            case "Darelt":
                Application.OpenURL("https://darelt.itch.io/");
                break;
            case "Tesla":
                Application.OpenURL("https://teslasp2.itch.io/");
                break;
        }
    }
    public void LeaveHelp()
    {
        StartCoroutine(FadeBlack(FadeType.ToBlack, fullUiFadeBlack));
        Invoke(nameof(LeaveHelp2), 1f);
    }
    private void LeaveHelp2()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
