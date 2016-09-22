﻿using UnityEngine;
using System.Collections;
using System;


public enum ButtonPressed
{
    Grip,
	Trigger
}

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ViveHandController : HandController {

    public GameObject HandModel;

    private bool _triggerPressed = false;
    private bool _gripPressed = false;

    private Valve.VR.EVRButtonId _triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId _gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;

    private SteamVR_Controller.Device _inputDevice { get { return SteamVR_Controller.Input((int)_trackedObj.index); } }
    private SteamVR_TrackedObject _trackedObj;

//  void Awake()
	private void Start()
    {
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

	private void Update()
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
            ButtonAction(ButtonPressed.Trigger, true);
        }

        if (_inputDevice.GetPressUp(_triggerButton))
        {
            ButtonAction(ButtonPressed.Trigger, false);
        }

        if (_inputDevice.GetPressDown(_gripButton))
        {
            ButtonAction(ButtonPressed.Grip, true);
        }

        if (_inputDevice.GetPressUp(_gripButton))
        {
            ButtonAction(ButtonPressed.Grip, false);
        }

        if (!_triggerPressed && ConnectedObject != null)
        {
            Debug.Log("Released trigger-button -> Releasing attached object (" + ConnectedObject.name + ")");
            ConnectedObject.transform.parent = null;
            ConnectedObject = null;
        }
		if(!_gripPressed && ConnectedObject != null)
		{
			ConnectedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			ConnectedObject.transform.parent = null;
			ConnectedObject = null;
		}
    }

    private void ButtonAction(ButtonPressed btn, bool pressed)
    {
        string msg = "Button ";

        if (btn == ButtonPressed.Grip)
        {
            _gripPressed = pressed;
            msg += "Grip ";
        }
        else if (btn == ButtonPressed.Trigger)
        {
            _triggerPressed = pressed;
            msg += "Trigger ";
        }

        msg += (pressed ? "pressed" : "released");

        Debug.Log(msg + "!");
    }

    void OnTriggerStay(Collider col)
    {
        if(_triggerPressed && col.transform.parent.tag == "Container" && ConnectedObject == null)
        {
            ConnectedObject = col.transform.parent.gameObject;
            ConnectedObject.transform.parent = HandModel.transform;
        }

		if(_inputDevice.GetPressDown(_gripButton) && col.gameObject.tag == "TagGun" && ConnectedObject == null)
		{
			ConnectedObject = col.transform.gameObject;
			ConnectedObject.transform.parent = HandModel.transform;
			col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			GetComponentInChildren<SteamVR_RenderModel>().enabled = false;
		}
    }

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "TagGun")
		{
			GetComponentInChildren<SteamVR_RenderModel>().enabled = true;
		}
	}
}
