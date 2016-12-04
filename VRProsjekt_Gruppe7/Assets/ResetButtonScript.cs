using UnityEngine;
using System.Collections;
using NewtonVR;

public class ResetButtonScript : MonoBehaviour {

    public NVRButton Button;

    GameManager _gM;

    void Start()
    {
        _gM = GameObject.Find("_SCRIPTS").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Button.ButtonDown)
        {
			_gM.GetComponent<GameManager>().RestartGame();
        }
    }
}
