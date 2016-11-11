using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// Code snippet taken from https://docs.unity3d.com/ScriptReference/AudioSettings-dspTime.html with modifications to change it from emitting spund to make a game object jump instead
public class RadioScript : MonoBehaviour {

    public float MaxRotationalForce = 0.1f;
    public float MaxRotate = 5f;
    public double bpm = 140.0F;
    public int jumpTo_bpmRatio = 4;
    private double nextTick = 0.0F;
    private double sampleRate = 0.0F;

    private bool running = false;
    private Rigidbody rb;
    private bool _jump = false;

    //For the audio drop on collision
    private float pitch;
    float counter = 0;
    AudioSource audio;
    bool broken = false;
    public float durationOfDeath = 500;
    public float MaxImpactForceBeforeBroken = 2;

    //The sound is supposed to speed up a teeeensy bit when the cloc is running low
    GameManager GM_script;

    void Start()
    {
        GM_script = GameObject.Find("_SCRIPTS").GetComponent<GameManager>();
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick * sampleRate;
        running = true;
        rb = gameObject.GetComponent<Rigidbody>();

        //For breaking
        audio = GetComponent<AudioSource>();
        pitch = audio.pitch;


        
    }

    void FixedUpdate()
    {
        if (_jump)
        {
            _jump = false;
            Jump();
            MaxRotationalForce = MaxRotationalForce * pitch; //this is so that when the radio dies, the jumping stops
            
        }
        // increasing stress if game is near the end --- did mess with gameManager script to make this happen
        Debug.Log("GM_script.Get_timeLeft is: " + GM_script.Get_timeLeft());

        if (GM_script.Get_timeLeft() <= 15 && GM_script.Get_timeLeft() > 0)
        {
            audio.pitch = 1.05f;
        }
        else if (!broken) { audio.pitch = 1; }

        //Debug.Log("the radio is broken == " + broken);
        if (broken)
        {
            audio.pitch = Mathf.Lerp(1f, 0, Mathf.Lerp(0f, 1, counter / durationOfDeath));
            //Debug.Log("counter is: " + counter + " and durationOfDeath is: " + durationOfDeath);
            //Debug.Log("Therefore counter/durationofdeath is: " + counter / durationOfDeath);
            //Debug.Log("Therefore Lerp 1f -> 0 is at " + (Mathf.Lerp(0, 1, counter/durationOfDeath)));
        } else counter = 0;
        counter++;
        pitch = audio.pitch;
        if (counter > durationOfDeath) { Destroy(this); }
        //if (Input.GetButtonDown("Jump")) { broken = true; }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > MaxImpactForceBeforeBroken)
            broken = true;
    }

    void Jump()
    {

        GetComponent<Rigidbody>().AddForce(Vector3.up * pitch, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, 0, Random.Range(-MaxRotationalForce, MaxRotationalForce)), ForceMode.Impulse);

    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!running)
            return;

        double samplesPerTick = sampleRate * 60.0F / (bpm * pitch) * 4.0F / jumpTo_bpmRatio;
        double sample = AudioSettings.dspTime * sampleRate;
        int dataLen = data.Length / channels;
        int n = 0;
        while (n < dataLen)
        {
            while (sample + n >= nextTick)
            {
                nextTick += samplesPerTick;
                _jump = true;
                
            }
            n++;
        }
    }
}
