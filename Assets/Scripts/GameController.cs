using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public float spawnTime;
    public Bird[] birdPrefabs;
    public int timeLimit;

    int m_curTimeLimit;
    int m_birdKilled;
    bool m_isGameOver;

    public int CurTimeLimit { get => m_curTimeLimit; set => m_curTimeLimit = value; }
    public int BirdKilled { get => m_birdKilled; set => m_birdKilled = value; }
    public bool IsGameOver { get => m_isGameOver; set => m_isGameOver = value; }

    public override void Awake()
    {
        MakeSingleton(false);
        m_curTimeLimit = timeLimit;
    }

    public override void Start()
    {
        GameUI.Ins.showGameUi(false);
        GameUI.Ins.updateBirdKilled(m_birdKilled);
    }

    public void PlayGame()
    {
        StartCoroutine(GameSpawn());
        StartCoroutine(TimeCountDown());
        GameUI.Ins.showGameUi(true);
    }

    IEnumerator TimeCountDown()
    {
        while (m_curTimeLimit > 0)
        {
            yield return new WaitForSeconds(1f);
            m_curTimeLimit--;
            if (m_curTimeLimit <= 0)
            {
                m_isGameOver = true;

                if (m_birdKilled > Prefs.bestScore)
                {
                    GameUI.Ins.gameDialog.UpdateDialog("New best", "Best killed: x" + m_birdKilled);
                } 
                else if (m_birdKilled < Prefs.bestScore)
                {
                    GameUI.Ins.gameDialog.UpdateDialog("Game over", "your score: x" + m_birdKilled);
                }

                Prefs.bestScore = m_birdKilled;

                GameUI.Ins.gameDialog.show(true);
                GameUI.Ins.CurDialog = GameUI.Ins.gameDialog;

            }
            GameUI.Ins.UpdateTimer(IntToTime(m_curTimeLimit));

        }
    }

    IEnumerator GameSpawn()
    {
        while (!IsGameOver)
        {
            SpawnBird();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    void SpawnBird()
    {
        Vector3 spawnPos = Vector3.zero;

        float randCheck = Random.Range(0f, 1f);
        if (randCheck >= 0.5f)
        {
            spawnPos = new Vector3(15, Random.Range(1.5f, 4f), 0);
        }
        else
        {
            spawnPos = new Vector3(-15, Random.Range(1.5f, 4f), 0);
        }

        if (birdPrefabs != null && birdPrefabs.Length > 0)
        {
            int randIdx = Random.Range(0, birdPrefabs.Length);
            if (birdPrefabs[randIdx] != null)
            {
                Bird bird = Instantiate(birdPrefabs[randIdx], spawnPos, Quaternion.identity);
            }
        }
    }

    string IntToTime(int time)
    {
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.RoundToInt(time% 60);

        return minutes.ToString("00") + " : " + seconds.ToString("00");
    }

}
