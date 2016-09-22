using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts;

public enum State
{
    Paused,
    Running,
    Quit
}

public class GameManager : MonoBehaviour
{
    public int CurrentScore;
    public float TimeLeft;

    public GameObject PlayerCameraPrefab;

    private float _startTime = 60f;
    private State _currentState;

	public GuiController GuiController;

    void Awake()
    {
        Init();
    }

    void Start ()
    {
        // (Show intro?)
	    // Show main menu
	    var gO = GameObject.FindGameObjectWithTag("GameManager");
	    GuiController = gO.GetComponent<GuiController>();
    }
	
	void Update ()
    {
        if(_currentState == State.Running)
        {
            Countdown();
	        GuiController.SetTextInfo("Score: "+CurrentScore, "Time: "+(int)TimeLeft);
        }	
	}

    private void Countdown()
    {
        if(TimeLeft>0)
            TimeLeft -= 1f * Time.deltaTime;

        if (TimeLeft <= 0 && _currentState == State.Running)
            GameStop();
    }

    private void GameStop()
    {
        _currentState = State.Paused;
        TimeLeft = 0;
    }

    private void Init()
    {
        CurrentScore = 0;
        GameStop();
    }

    public void StartGame()
    {
        _currentState = State.Running;
        TimeLeft = _startTime;
		Debug.Log("The Game Begins");
    }

}
