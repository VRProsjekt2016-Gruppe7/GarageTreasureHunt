using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class GUIController : MonoBehaviour {

		//private TextMesh _currentScore;
		//private TextMesh _timeLeft;
		public Text CurrentScore;
		public Text TimeLeft;

        private readonly string _chargesText = "Avaliable charges: ";
        private readonly string _timeLeftText = "TIME LEFT: ";
        private readonly string _curScoreText = "Current score: ";

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
            SetCharges(charges);
            SetTimeLeft(timeLeft);
            SetScore(score);
        }

        public void SetCharges(int charges)
        {
            TimeLeft.text = _chargesText + (charges >= 0 ? charges : 0);
        }

        public void SetTimeLeft(float timeLeft)
        {
            TimeLeft.text = _timeLeftText + ((int)timeLeft >= 0 ? timeLeft : 0);
        }

        public void SetScore(int score)
		{
            CurrentScore.text = _curScoreText + (score >= 0 ? score : 0);
		}
	}
}
