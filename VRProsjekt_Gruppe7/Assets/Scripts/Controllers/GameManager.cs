using UnityEngine;
using Assets.Scripts;
using System;

public enum State
{
    Stopped,
    Running,
    Quit
}

public class GameManager : MonoBehaviour
{
    public GameObject TagGunPrefab;
    public GameObject TagGun;
    public GameObject HighScoreClipboard;
    public GameObject RadioPrefab;
    public GameObject Radio;
    public static State CurrentState;

    private readonly float _defaultStartTime = 60f;
    private readonly int _defaultCharges = 7;
    private readonly Vector3 _clipboardStartPos = new Vector3(-0.165f, 0.936f, 1.08f);
    private readonly Vector3 _clipboardStartRot = new Vector3(10f, 0f, 0f);
    private readonly Vector3 _radioStartPos = new Vector3(0f, 1.483f, -3.07f);
    private readonly Vector3 _radioStartRot = new Vector3(0f, 180f, 0f);

    private float _timeLeft;
    private int _currentScore;

    private GUIController _guiController;
    private HighScoreController _hsController;

    /*
     * NEW WAY TO FORCE GAME START
     * IS MOVED TO TAGGUNBEHAVIOUR
     * DUE TO NEW IMPLEMENTATIONS
     * REGARDING HOW THE GAME STARTS
     */

    void Awake()
    {
        _guiController = GetComponent<GUIController>();
        _hsController = GetComponent<HighScoreController>();
        SpawnNewTagGun();
        SpawnNewRadio();
        Init();
        PositionClipBoard();
        InitShelves();
    }

    private void SpawnNewTagGun()
    {
        if(TagGun != null)
            Destroy(TagGun);

        TagGun = (GameObject) Instantiate(TagGunPrefab, transform.position, transform.rotation);
        TagGun.GetComponent<TagGunBehaviour>().Init(_defaultCharges);
    }

    private void SpawnNewRadio()
    {
        if (Radio != null)
            Destroy(Radio);

        Radio = (GameObject)Instantiate(RadioPrefab, transform.position, transform.rotation);
        TagGun.GetComponent<TagGunBehaviour>().Init(_defaultCharges);
        Radio.transform.position = _radioStartPos;
        Radio.transform.eulerAngles = _radioStartRot;
    }

    void Start()
    {
        _guiController.Init(_defaultStartTime);
    }

    void Update()
    {
        if (CurrentState == State.Running)
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

        if (_timeLeft <= 0 && CurrentState == State.Running)
        {
            EndGame();
        }
    }

    private void Init()
    {
        ResetValues();
        EndGame();
    }

    public void StartGame()
    {
        GetComponent<RoomGenerator>().SpawnBoxesAndContents();
        ResetValues();
        _guiController.StartGame();
        CurrentState = State.Running;
    }

    private void ResetValues()
    {
        _timeLeft = _defaultStartTime;
        _currentScore = 0;
    }

    private void InitShelves()
    {
        GetComponent<RoomGenerator>().ResetAndSpawnShelves();
    }

    public void RestartGame()
    {
        SpawnNewTagGun();
        SpawnNewRadio();
        RemoveContents();
        EndGame();
    }

    public void EndGame()
    {
        CurrentState = State.Stopped;
        PositionClipBoard();
        _guiController.GameOver(_currentScore);
        _hsController.GameEnd(_currentScore);
    }

    private void RemoveContents()
    {
        GetComponent<RoomGenerator>().CleanRoom();
    }

    public float GetDefaultStartTime()
    {
        return _defaultStartTime;
    }

    public float GetTimeLeft()
    {
        return _timeLeft;
    }

    private void PositionClipBoard()
    {
        HighScoreClipboard.transform.position = _clipboardStartPos;
        HighScoreClipboard.transform.eulerAngles = _clipboardStartRot;
    }
}
