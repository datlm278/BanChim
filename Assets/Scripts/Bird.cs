using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float xSpeed;
    public float minYSpeed;
    public float maxYSpeed;

    public GameObject deathVfx;

    Rigidbody2D m_rigidbody2D;
    bool m_isMoveLeft;
    bool m_isDead;
    private void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        RandomMovingDirection();
    }

    private void Update()
    {
        m_rigidbody2D.velocity = m_isMoveLeft ? new Vector2(-xSpeed, Random.Range(minYSpeed, maxYSpeed)) : new Vector2(xSpeed, Random.Range(minYSpeed, maxYSpeed));
        Flip();
    }

    public void RandomMovingDirection()
    {
        m_isMoveLeft = transform.position.x > 0 ? true : false;
    }

    public void Flip()
    {
        if (m_isMoveLeft)
        {
            if (transform.localScale.x < 0)
            {
                return;
            } 
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        } 
        else
        {
            if (transform.localScale.x > 0)
            {
                return;
            }
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    public void Dead()
    {
        m_isDead = true;

        GameController.Ins.BirdKilled++;

        Destroy(gameObject);

        if (deathVfx != null)
        {
            Instantiate(deathVfx, transform.position, Quaternion.identity);
        }
        GameUI.Ins.updateBirdKilled(GameController.Ins.BirdKilled);
    }
}
