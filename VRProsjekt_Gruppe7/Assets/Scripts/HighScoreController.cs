using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScoreController : MonoBehaviour
{
    public Text[] HighScoreText;
	
	void Start ()
	{
	    Init();
	}

    private void Init()
    {
        for (int i = 1; i <= 10; i++)
        {
            print(i);
            string index = (i < 10) ? ("0" + i) : "" + i;  
            HighScoreText[i - 1].text = index + ": " + (10 - i + "00");
        }
    }

    void Update () {
	
	}
}
