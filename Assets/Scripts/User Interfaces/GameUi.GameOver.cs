using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Mostly copied from CrossfireCam's Chaotic Comets project

public partial class GameUi : BaseUi
{
    private TMP_InputField currentNameField;
    private TextMeshProUGUI textRoundReached;

    public void GameIsOverShowUi()
    {
        gameOverPanel.SetActive(true);
        RectTransform gameOverRt = gameOverPanel.transform.Find("Dialog").GetComponent<RectTransform>();

        // Fill out the 'Reached Round' section
        textRoundReached = gameOverPanel.transform.Find("RoundReached").Find("RoundImage").Find("TextRound").GetComponent<TextMeshProUGUI>();
        textRoundReached.text = gM.currentRound.ToString();

        // If the game doesn't detect a high score, then shrink the game over window and remove unneeded parts
        if (!IsThisAHighScore(gM.currentRound))
        {
            gameOverRt.sizeDelta = new Vector2(gameOverRt.sizeDelta.x, 360);
            gameOverRt.Find("NewScoreParts").gameObject.SetActive(false);
        }
        // If high score is detected, then perform actions
        else
        {
            FindFieldAndLoadPreviousName();
        }


        // This function also handles if a game is ended prematurely, but a high score is accomplished
        // If game is not actually over when UI is called, then remove Retry button and change some strings
        if (!gM.gameIsOver)
        {
            gamePausePanel.SetActive(false);
            gameOverRt.sizeDelta = new Vector2(gameOverRt.sizeDelta.x, 380);

            TextMeshProUGUI textGameOverTitle = gameOverRt.Find("GameOverTitleText").GetComponent<TextMeshProUGUI>();
            textGameOverTitle.text = "Game Cancelled!";
            TextMeshProUGUI textGameOverSubtitle = gameOverRt.Find("NewScoreParts").Find("EnterNameText").GetComponent<TextMeshProUGUI>();
            textGameOverSubtitle.text = "...but you still got a high score! Enter a name.";
            GameObject btnGameOverRetry = gameOverRt.Find("RetryButton").gameObject;
            btnGameOverRetry.SetActive(false);
        }

        Time.timeScale = 0;
    }

    private bool IsThisAHighScore(int roundReached)
    {
        if (roundReached > PlayerPrefs.GetInt("HighscoreNum"))
        {
            return true;
        }
        return false;
    }

    private void FindFieldAndLoadPreviousName()
    {
        // Find the currentNameField
        currentNameField = gameOverPanel.transform.Find("Dialog").Find("NewScoreParts").Find("NameField").GetComponent<TMP_InputField>();

        // If name is not a default value, load the last used name into the textbox
        string previousName = PlayerPrefs.GetString("HighscoreName");
        if (previousName != "No Highscore Yet" && previousName != "Anonymous Cat")
        {
            currentNameField.text = PlayerPrefs.GetString("HighscoreName");
        }
    }
    private void CheckAndSaveHighscore()
    {
        if (IsThisAHighScore(gM.currentRound))
        {
            // Set highscore's name depending on which panel's InputField is being used.
            string nameFromField = currentNameField.text;

            // Renames blank names to 'Anonymous Cat'
            if (string.IsNullOrWhiteSpace(nameFromField))
            {
                nameFromField = "Anonymous Cat";
            }

            // Submit high score
            PlayerPrefs.SetInt("HighscoreNum", gM.currentRound);
            PlayerPrefs.SetString("HighscoreName", nameFromField);
        }
    }

    public void RestartGame()
    {
        CheckAndSaveHighscore();
        musicManager.ResetStageMusicOnRetry();
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
    }

    public void ExitLevelFromGameOver()
    {
        CheckAndSaveHighscore();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
