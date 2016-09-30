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
    public GameObject PlayerCameraPrefab;

    private readonly float _defaultStartTime = 60f;
    private readonly int _defaultCharges = 12;
    private float _timeLeft;
    private int _currentScore;
    private int _chargesLeft;
    private State _currentState;
    private GuiController _guiController;

    void Awake()
    {
        _guiController = GetComponent<GuiController>();
        Init();
    }

    void Start ()
    {
        // (Show intro?)
	    // Show main menu
        
    }
	
	void Update ()
    {
        if(_currentState == State.Running)
        {
            Countdown();
        }	
	}

    private void UpdateTimeLeft()
    {
        _guiController.SetTimeLeft(_timeLeft);
    }

    private void Countdown()
    {
        if(_timeLeft>0)
            _timeLeft -= 1f * Time.deltaTime;

        if (_timeLeft <= 0 && _currentState == State.Running)
            GameStop();
    }


    private void Init()
    {
        _chargesLeft = 0;
        _timeLeft = 0;
        _currentScore = 0;
        GameStop();
    }

    public void StartGame()
    {
        _currentState = State.Running;
        _timeLeft = _defaultStartTime;
        _chargesLeft = _defaultCharges;
        Debug.Log("The Game Begins");
    }

    private void GameStop()
    {
        _currentState = State.Paused;
    }
}
