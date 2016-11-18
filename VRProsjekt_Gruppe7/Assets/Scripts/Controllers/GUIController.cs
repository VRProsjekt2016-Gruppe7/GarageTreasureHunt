using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class GUIController : MonoBehaviour {

		public Text CurrentScore;
		public Text TimeLeft;
        public Text FinalScore;
        public GameObject HowToPlayPanel;
	    public GameObject GamePlayPanel;
	    public GameObject GameOverPanel;

   	    private int _lastTime = 0;
	    private float _gameDuration;

		public void Init(float gameDuration)
		{
		    _gameDuration = gameDuration;
            SwapPanels(true, false, false);
            SetTimeLeft(0);
            SetScore(0);
        }

        public void SetTextInfo(int charges, int timeLeft, int score)
		{
            SetTimeLeft(timeLeft);
            SetScore(score);
        }

        public void SetTimeLeft(float timeLeft)
        {
            float t = timeLeft / _gameDuration;

            Color colorLerped = Color.Lerp(Color.red, Color.green, t);
            TimeLeft.color = colorLerped;

            TimeLeft.text = "" + ((int)timeLeft >= 0 ? (int)timeLeft : 0);

            if ((int) timeLeft <= 10 && _lastTime != (int)timeLeft)
            {
                _lastTime = (int) timeLeft;
                GetComponent<SoundController>().PlaySoundAtSourceOnce(SoundSource.GuiSource, Sounds.CountDownBeep);
            }
        }

        public void SetScore(int score)
		{
            CurrentScore.text = "$ " + (score >= 0 ? score : 0);
		}

        public void SetFinalScore(int score)
        {
            FinalScore.text = "$ " + (score >= 0 ? score : 0);
        }

        public void StartGame()
	    {
	        ResetScores();
            SwapPanels(false, true, false);
	    }

	    private void ResetScores()
	    {
	        SetScore(0);
            SetFinalScore(0);
	    }

	    public void GameOver(int finalScore)
	    {
	        SwapPanels(false, false, true);
            SetFinalScore(finalScore);
	    }

	    private void SwapPanels(bool howTo, bool gamePlay, bool gameOver)
	    {
            HowToPlayPanel.SetActive(howTo);
            GamePlayPanel.SetActive(gamePlay);
            GameOverPanel.SetActive(gameOver);
        }
    }
}
