using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GUIController : MonoBehaviour {

    //private TextMesh _currentScore;
    //private TextMesh _timeLeft;
    public Text CurrentScore;
    public Text TimeLeft;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        SetScore("0");
        SetTimeLeft("0");
    }

    public void SetTextInfo(string score, string timeLeft)
    {
        SetScore(score);
        SetTimeLeft(timeLeft);
    }

    private void SetScore(string score)
    {
        CurrentScore.text = score;
    }

    private void SetTimeLeft(string timeLeft)
    {
        TimeLeft.text = timeLeft;
    }
}
