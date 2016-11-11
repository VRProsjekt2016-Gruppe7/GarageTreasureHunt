using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// Code snippet taken from https://docs.unity3d.com/ScriptReference/AudioSettings-dspTime.html with modifications to change it from emitting spund to make a game object jump instead
public class RadioScript : MonoBehaviour {

    public float MaxRotationalForce = 0.1f;
    public float MaxRotate = 5f;
    public double Bpm = 140.0F;
    public int JumpToBpmRatio = 4;
    private double _nextTick = 0.0F;
    private double _sampleRate = 0.0F;

    private bool _running = false;
    private bool _jump = false;

    //For the _radioAudioSource drop on collision
    private float _pitch;
    private float _counter = 0;
    AudioSource _radioAudioSource;
    private bool _broken = false;
    private float _durationOfDeath = 500;
    private float _maxImpactForceBeforeBroken = 2;

    //The sound is supposed to speed up a teeeensy bit when the cloc is _running low
    GameManager _gM;

    void Start()
    {
        _gM = GameObject.Find("_SCRIPTS").GetComponent<GameManager>();
        double startTick = AudioSettings.dspTime;
        _sampleRate = AudioSettings.outputSampleRate;
        _nextTick = startTick * _sampleRate;
        _running = true;

        //For breaking
        _radioAudioSource = GetComponent<AudioSource>();
        _pitch = _radioAudioSource.pitch;


        
    }

    void FixedUpdate()
    {
        if (_jump)
        {
            _jump = false;
            Jump();
            MaxRotationalForce = MaxRotationalForce * _pitch; //this is so that when the radio dies, the jumping stops
            
        }
        // increasing stress if game is near the end --- did mess with gameManager script to make this happen

        if (_gM.Get_timeLeft() <= 15 && _gM.Get_timeLeft() > 0)
        {
            _radioAudioSource.pitch = 1.05f;
        }
        else if (!_broken) { _radioAudioSource.pitch = 1; }

        //Debug.Log("the radio is _broken == " + _broken);
        if (_broken)
        {
            _radioAudioSource.pitch = Mathf.Lerp(1f, 0, Mathf.Lerp(0f, 1, _counter / _durationOfDeath));
            //Debug.Log("_counter is: " + _counter + " and _durationOfDeath is: " + _durationOfDeath);
            //Debug.Log("Therefore _counter/durationofdeath is: " + _counter / _durationOfDeath);
            //Debug.Log("Therefore Lerp 1f -> 0 is at " + (Mathf.Lerp(0, 1, _counter/_durationOfDeath)));
        } else _counter = 0;
        _counter++;
        _pitch = _radioAudioSource.pitch;
        if (_counter > _durationOfDeath) { Destroy(this); }
        //if (Input.GetButtonDown("Jump")) { _broken = true; }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > _maxImpactForceBeforeBroken)
            _broken = true;
    }

    void Jump()
    {

        GetComponent<Rigidbody>().AddForce(Vector3.up * _pitch, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, 0, Random.Range(-MaxRotationalForce, MaxRotationalForce)), ForceMode.Impulse);

    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!_running)
            return;

        double samplesPerTick = _sampleRate * 60.0F / (Bpm * _pitch) * 4.0F / JumpToBpmRatio;
        double sample = AudioSettings.dspTime * _sampleRate;
        int dataLen = data.Length / channels;
        int n = 0;
        while (n < dataLen)
        {
            while (sample + n >= _nextTick)
            {
                _nextTick += samplesPerTick;
                _jump = true;
                
            }
            n++;
        }
    }
}
