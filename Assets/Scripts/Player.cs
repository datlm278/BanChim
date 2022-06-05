using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public float fireRate;
    float m_curFireRate;
    bool isShooted;
    bool isStop = false;

    public GameObject viewFinder;
    GameObject m_viewFinder;

    public bool IsStop { get => isStop; set => isStop = value; }

    public override void Awake()
    {
        MakeSingleton(false);
        m_curFireRate = fireRate;
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
            m_curFireRate -= Time.deltaTime;
            if (m_curFireRate <= 0)
            {
                isShooted = false;
                m_curFireRate = fireRate;
            }

            GameUI.Ins.updateFireRate(m_curFireRate / fireRate);
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
        Vector3 shootDir = Camera.main.transform.position - mousePos;
        shootDir.Normalize();
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, shootDir);

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
