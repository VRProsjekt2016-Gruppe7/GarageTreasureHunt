using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ViveHandController : HandController {

    private Valve.VR.EVRButtonId _triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId _gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;

    private SteamVR_Controller.Device _inputDevice { get { return SteamVR_Controller.Input((int)_trackedObj.index); } }
    private SteamVR_TrackedObject _trackedObj;

    void Awake()
    {
        _trackedObj = GetComponent<SteamVR_TrackedObject>();

    }

    void Update()
    {
        if (_inputDevice == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        if (_inputDevice.GetPressDown(_triggerButton))
        {
            Debug.Log("Fire (Trigger-button pressed)");
        }

        if (_inputDevice.GetPressDown(_gripButton))
        {
            Debug.Log("Gripping (Grip-button pressed)");
        }
    }


    void OnTriggerEnter(Collider col)
    {

    }
}
