using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public float spawnTime;
    public Bird[] birdPrefabs;
    public int timeLimit;
    public GameUI gameUI;
    private List<Bird> birds = new List<Bird>();
    int m_curTimeLimit;
    int m_score = 0;
    bool m_isGameOver = false;
    int m_health = 4;

    public List<Bird> Birds { get => birds; set => birds = value; }
    public int CurTimeLimit { get => m_curTimeLimit; set => m_curTimeLimit = value; }
    public int Score { get => m_score; set => m_score = value; }
    public bool IsGameOver { get => m_isGameOver; set => m_isGameOver = value; }
    public int Health { get => m_health; set => m_health = value; }

    public override void Awake()
    {
        MakeSingleton(false);
        m_curTimeLimit = timeLimit;
    }

    public override void Start()
    {
        GameUI.Ins.showGameUi(false);
        GameUI.Ins.UpdateHealth(m_health);
        GameUI.Ins.updateScore(m_score);
    }

    public void PlayGame()
    {
        StartCoroutine(GameSpawn());
        StartCoroutine(TimeCountDown());
        GameUI.Ins.showGameUi(true);
    }

    IEnumerator TimeCountDown()
    {
        while (m_curTimeLimit > 0 && !IsGameOver)
        {
            yield return new WaitForSeconds(1f);
            m_curTimeLimit--;
            GameUI.Ins.UpdateTimer(IntToTime(m_curTimeLimit));
        }

        m_isGameOver = true;

        if (m_score > Prefs.bestScore)
        {
            GameUI.Ins.gameDialog.UpdateDialog("New best", "Best killed: x" + m_score);
        }
        else if (m_score < Prefs.bestScore)
        {
            GameUI.Ins.gameDialog.UpdateDialog("Game over", "your score: x" + m_score);
        }

        Prefs.bestScore = m_score;

        GameUI.Ins.gameDialog.show(true);
        GameUI.Ins.CurDialog = GameUI.Ins.gameDialog;

        StopAllBirds();
        Player.Ins.IsStop = true;
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
            spawnPos = new Vector3(4, Random.Range(0, 2.5f), 0);
        }
        else
        {
            spawnPos = new Vector3(-15, Random.Range(0, 2.5f), 0);
        }

        if (birdPrefabs != null && birdPrefabs.Length > 0)
        {
            int randIdx = Random.Range(0, birdPrefabs.Length);
            if (birdPrefabs[randIdx] != null)
            {
                Bird bird = Instantiate(birdPrefabs[randIdx], spawnPos, Quaternion.identity);
                birds.Add(bird);
            }
        }
    }

    public void StopAllBirds()
    {
        foreach (var bird in birds)
        {
            if (bird != null)
            {
                bird.Stop();
            }
        }
    }
    string IntToTime(int time)
    {
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.RoundToInt(time % 60);

        return minutes.ToString("00") + " : " + seconds.ToString("00");
    }
}
