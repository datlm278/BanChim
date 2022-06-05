using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Text titleText;
    public Text contentText;
    public Text highScoreText;

    public void show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
    public void UpdateDialog(string title, string content)
    {
        if (titleText != null)
        {
            titleText.text = title;
        }
        if (contentText != null)
        {
            contentText.text = content;
        }
    }

    public void UpdateDialog(string title, string content, string highScore)
    {
        if (titleText != null)
        {
            titleText.text = title;
        }
        if (contentText != null)
        {
            contentText.text = content;
        }
        if (contentText != null)
        {
            highScoreText.text = highScore;
        }
    }
}
