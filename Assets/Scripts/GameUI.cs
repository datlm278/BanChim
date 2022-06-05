using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    public GameObject homeUi;
    public GameObject gameUI;

    public Dialog gameDialog;
    public Dialog pauseDialog;

    public Image fireRateFilled;
    public Image streakFilled;
    public Text timer;
    public Text birdKilled;

    public Text health;

    Dialog m_curDialog;

    public Dialog CurDialog { get => m_curDialog; set => m_curDialog = value; }

    public override void Awake()
    {
        MakeSingleton(false);
    }

    public void showGameUi (bool isShow)
    {
        if (gameUI)
        {
            gameUI.SetActive(isShow);
        }
        if (homeUi)
        {
            homeUi.SetActive(!isShow);
        }
    }

    public void UpdateTimer(string time)
    {
       if (timer != null)
        {
            timer.text = time;
        }
    }

    public void updateBirdKilled(int killed)
    {
        if (birdKilled != null)
        {
            birdKilled.text = "x" + killed.ToString();
        }
    }

    public void updateFireRate(float rate)
    {
        if (fireRateFilled != null)
        {
            fireRateFilled.fillAmount = rate;
        }
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
        if (pauseDialog != null)
        {
            pauseDialog.show(true);
            pauseDialog.UpdateDialog("Game pause", "Best killed: x" + Prefs.bestScore);
            m_curDialog = pauseDialog;
        }
    }

    public void resumeGame()
    {
        Time.timeScale = 1f;
        if (m_curDialog != null)
        {
            m_curDialog.show(false);
        }
    }

    public void backToHome()
    {
        resumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void replay()
    {
        if (m_curDialog != null)
        {
            m_curDialog.show(false);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    public void exitGame()
    {
        resumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Application.Quit();
    }
}
