using UnityEngine;
using System.Collections;

public enum CurrentState
{
    Paused,
    Running,
    Quit
}

public class GameManager : MonoBehaviour {

    public int CurrentScore;
    public float TimeLeft;

    private float _startTime;
    private CurrentState _currentState;

    void Awake()
    {

    }

    void Start () {
	
	}
	
	void Update () {
	
	}

        
}
