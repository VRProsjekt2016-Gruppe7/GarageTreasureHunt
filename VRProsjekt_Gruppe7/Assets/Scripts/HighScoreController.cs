using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScoreController : MonoBehaviour
{
    public Text[] HighScoreText;
    private int _initScoreValue = 0;

    // Debug variables
    public bool UpdateNewScore = false;
    public int NewTestScore = 0;

    void Update()
    {
        if (UpdateNewScore)
        {
            UpdateNewScore = false;
            GameEnd(NewTestScore);
        }
    }


    // Debug end

    void Start ()
	{
	    Init();
	}

    private void Init()
    {
        for (int i = 1; i <= 10; i++)
        {
            string index = GetPosIndex(i);
            HighScoreText[i - 1].GetComponent<Text>().color = Color.white;

            if (!PlayerPrefs.HasKey(index))
            {
                PlayerPrefs.SetInt(index, _initScoreValue);
                HighScoreText[i - 1].text = index + ": " + _initScoreValue;
            }
            else
            {
                HighScoreText[i - 1].text = index + ": " + PlayerPrefs.GetInt(index);
            }
        }
    }

    public void GameEnd(int playerScore)
    {
        int currentScore = playerScore;
        int[] scores = new int[HighScoreText.Length];
        bool newHighScore = false;
        int newHighScorePos = 10;        

        for (int i = 0; i < scores.Length; i++)
        {
            print("GameEndLoop: " + i);

            string index = GetPosIndex(i + 1);
            scores[i] = PlayerPrefs.GetInt(index);

            if (scores[i] < currentScore && !newHighScore)
            {
                newHighScore = true;
                newHighScorePos = i;
            }
        }

        if (newHighScore)
        {
            scores = AddNewScore(scores, currentScore, newHighScorePos);
            UpdateHighScoreBoard(scores, newHighScorePos);
        }
    }

    private void UpdateHighScoreBoard(int[] scores, int newHighScorePos)
    {
        for (int i = 0; i < scores.Length; i++)
        {
            print("UpdateHighScoreBoard: " + i);

            string index = GetPosIndex(i + 1);
            HighScoreText[i].text = index + ": " + PlayerPrefs.GetInt(index);

            HighScoreText[i].GetComponent<Text>().color = (i == newHighScorePos) ? Color.green : Color.white;
        }
    }

    

    private int[] AddNewScore(int[] currentHighScores, int newScore, int pos)
    {
        int[] newScores = currentHighScores;

        for (int i = currentHighScores.Length - 1; i >= pos; i--)
        {
            print("AddNewScore: " + i);
            // Current key
            string index = GetPosIndex(i + 1);

            // Move highscore at next pos to current pos
            if (i == pos)
            {
                PlayerPrefs.SetInt(index, newScore);
                newScores[i] = newScore;
                break;
            }

            PlayerPrefs.SetInt(index, currentHighScores[i - 1]);
            newScores[i] = newScores[i - 1];
        }
        return newScores;
    }

    private string GetPosIndex(int i)
    {
        return (i < 10) ? ("0" + i) : "" + i;
    }
}
