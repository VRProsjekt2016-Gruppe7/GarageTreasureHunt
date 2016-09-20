using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ViveHandController : HandController {

    public GameObject HandModel;

    private bool _triggerPressed = false;
    private bool _gripPressed = false;

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

        CheckButtons();
    }

    private void CheckButtons()
    {
        if (_inputDevice.GetPressDown(_triggerButton))
        {
            _triggerPressed = true;
            Debug.Log("Fire (Trigger-button pressed)");
        }

        if (_inputDevice.GetPressUp(_triggerButton))
        {
            _triggerPressed = false;
        }

        if (_inputDevice.GetPressDown(_gripButton))
        {
            _gripPressed = true;
            Debug.Log("Gripping (Grip-button pressed)");
        }

        if (_inputDevice.GetPressUp(_gripButton))
        {
            _gripPressed = false;
        }

        if(!_triggerPressed && ConnectedObject != null)
        {
            ConnectedObject.transform.parent = null;
            ConnectedObject = null;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if(_triggerPressed && col.transform.parent.tag == "Container" && ConnectedObject == null)
        {
            ConnectedObject = col.transform.parent.gameObject;
            ConnectedObject.transform.parent = HandModel.transform;
        }
    }
}
