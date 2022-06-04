using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : Singleton<BGController>
{
    public Sprite[] sprites;

    public SpriteRenderer spriteRenderer;
    public override void Awake()
    {
        MakeSingleton(false);
    }

    public override void Start()
    {
        ChangeSprite();
    }

    public void ChangeSprite()
    {
        if (spriteRenderer != null && sprites != null && sprites.Length > 0)
        {
            int randamInx = Random.Range(0, sprites.Length);
            if (sprites[randamInx] != null)
            {
                spriteRenderer.sprite = sprites[randamInx];
            }
        }
    }
}
