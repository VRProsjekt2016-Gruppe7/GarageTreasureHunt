using UnityEngine;
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
    public GameObject TagGun;

    private readonly float _defaultStartTime = 60f;
    private readonly int _defaultCharges = 12;
    private float _timeLeft;
    private int _currentScore;
    private int _chargesLeft;
    private State _currentState;
    private GUIController _guiController;


    public bool MaualStart = false;

    void Awake()
    {
        _guiController = GetComponent<GUIController>();
        Init();
        TagGun.GetComponent<TagGunBehaviour>().Init(_defaultCharges);
    }

    void Update()
    {
        // Debug
        if (_currentState == State.Paused && MaualStart)
        {
            MaualStart = false;
            StartGame();

        }
        //Debug end

        if (_currentState == State.Running)
        {
            Countdown();
        }
    }

    private void UpdateTimeLeft()
    {
        _guiController.SetTimeLeft(_timeLeft);
    }

    private void UpdateScore()
    {
        _guiController.SetScore(_currentScore);
    }

    public void AddScore(int points)
    {
        if (points < 0)
            return;

        _currentScore += points;

        UpdateScore();
    }

    private void Countdown()
    {
        if (_timeLeft > 0)
        {
            _timeLeft -= 1f * Time.deltaTime;
            UpdateTimeLeft();
        }

        if (_timeLeft <= 0 && _currentState == State.Running)
        {
            GameStop();
        }
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
        GetComponent<RoomGenerator>().GenerateRoom();
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
