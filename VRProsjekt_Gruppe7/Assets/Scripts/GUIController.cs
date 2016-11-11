using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class GUIController : MonoBehaviour {

		public Text CurrentScore;
		public Text TimeLeft;
	    public GameObject HowToPlayPanel;
	    public GameObject GamePlayPanel;

        private readonly string _chargesText = "Avaliable charges: ";
   	    private int _lastTime = 0;


        void Start()
		{
			Init();
		}

		public void Init()
		{
            SetCharges(0);
            SetTimeLeft(0);
            SetScore(0);
        }

        public void SetTextInfo(int charges, int timeLeft, int score)
		{
            //SetCharges(charges);
            SetTimeLeft(timeLeft);
            SetScore(score);
        }

        public void SetCharges(int charges)
        {
            
        }

        public void SetTimeLeft(float timeLeft)
        {
            float t = timeLeft / 60;

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
            CurrentScore.text = "" + (score >= 0 ? score : 0);
		}

	    public void StartGame()
	    {
            HowToPlayPanel.SetActive(false);
            GamePlayPanel.SetActive(true);
	    }
	}
}
