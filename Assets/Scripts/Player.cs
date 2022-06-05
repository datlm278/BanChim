using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public float fireRate;
    float m_curFireRate;
    bool isShooted;
    bool isStreaked;
    int streakLevel;
    float currStreakTime;
    int[] streakLevels = { 10, 7, 5, 3 };
    int[] multipleStreak = { 2, 3, 5, 8 };
    bool isStop = false;

    public GameObject viewFinder;
    GameObject m_viewFinder;

    public bool IsStop { get => isStop; set => isStop = value; }
    public float CurFireRate { get => m_curFireRate; set => m_curFireRate = value; }
    public int GetMultipleStreakPoint()
    {
        if (isStreaked)
        {
            return multipleStreak[streakLevel - 1];
        }

        return 1;
    }

    public override void Awake()
    {
        MakeSingleton(false);
        CurFireRate = fireRate;
    }

    public override void Start()
    {
        if (viewFinder)
        {
            m_viewFinder = Instantiate(viewFinder, Vector3.zero, Quaternion.identity);
        }
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !isShooted)
        {
            shoot(mousePos);
        }
        if (isShooted)
        {
            CurFireRate -= Time.deltaTime;
            if (CurFireRate <= 0)
            {
                isShooted = false;
                CurFireRate = fireRate;
            }

            GameUI.Ins.updateFireRate(CurFireRate / fireRate);
        }
        if (isStreaked)
        {
            currStreakTime -= Time.deltaTime;
            if (currStreakTime <= 0)
            {
                isStreaked = false;
                streakLevel = 0;
                currStreakTime = 0;
            }
            GameUI.Ins.UpdateStreakTime(streakLevel == 0 ? 0 : currStreakTime / streakLevels[streakLevel - 1]);
            GameUI.Ins.UpdateStreakLabel(streakLevel == 0 ? 1 : multipleStreak[streakLevel - 1]);
        }
        if (m_viewFinder)
        {
            m_viewFinder.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }
    void shoot(Vector3 mousePos)
    {
        if (IsStop)
        {
            return;
        }

        isShooted = true;

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, new Vector3(0, 1f, 0));

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                Bird bird = hit.collider.GetComponent<Bird>();
                if (bird != null)
                {
                    bird.Dead();
                }
            }
            isStreaked = true;
            streakLevel = streakLevel == streakLevels.Length ? streakLevel : streakLevel + 1;
            currStreakTime = streakLevels[streakLevel - 1];
        }
        else
        {
            GameController.Ins.Health--;
            GameUI.Ins.UpdateHealth(GameController.Ins.Health);
            if (GameController.Ins.Health == 0)
            {
                GameController.Ins.IsGameOver = true;
            }
        }

        CineController.Ins.ShakeTrigger();
        AudioController.Ins.PlaySound(AudioController.Ins.shooting);
    }
}
