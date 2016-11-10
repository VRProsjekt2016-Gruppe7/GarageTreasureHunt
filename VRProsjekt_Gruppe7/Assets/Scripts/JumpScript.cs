using UnityEngine;
using System.Collections;
// Code snippet taken from https://docs.unity3d.com/ScriptReference/AudioSettings-dspTime.html with modifications to change it from emitting spund to make a game object jump instead
public class JumpScript : MonoBehaviour {

    public float MaxRotationalForce = 0.1f;
    public float MaxRotate = 5f;
    public double bpm = 140.0F;
    public int jumpTo_bpmRatio = 4;
    private double nextTick = 0.0F;
    private double sampleRate = 0.0F;

    private bool running = false;
    private Rigidbody rb;
    private bool _jump = false;

    void Start()
    {
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick * sampleRate;
        running = true;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (_jump)
        {
            _jump = false;
            Jump();
        }
    }

    void Jump()
    {

        GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);

        GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, 0, Random.Range(-MaxRotationalForce, MaxRotationalForce)), ForceMode.Impulse);

    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!running)
            return;

        double samplesPerTick = sampleRate * 60.0F / bpm * 4.0F / jumpTo_bpmRatio;
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
