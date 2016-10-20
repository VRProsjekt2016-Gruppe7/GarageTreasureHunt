using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts;


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

    private SteamVR_Controller.Device InputDevice { get { return SteamVR_Controller.Input((int)_trackedObj.index); } }
    private SteamVR_TrackedObject _trackedObj;

	private TagGunBehaviour _tagGun;
	
//  void Awake()
	private void Start()
	{
		_tagGun = GameObject.FindGameObjectWithTag("TagGun").GetComponent<TagGunBehaviour>();
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

	private void Update()
    {
        if (InputDevice == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        CheckButtons();
    }

    private void CheckButtons()
    {
        if (InputDevice.GetPressDown(_triggerButton))
        {
            ButtonAction(ButtonPressed.Trigger, true);
        }

        if (InputDevice.GetPressUp(_triggerButton))
        {
            ButtonAction(ButtonPressed.Trigger, false);
        }

        if (InputDevice.GetPressDown(_gripButton))
        {
            ButtonAction(ButtonPressed.Grip, true);
        }

        if (InputDevice.GetPressUp(_gripButton))
        {
            ButtonAction(ButtonPressed.Grip, false);
        }

        if(ConnectedObject != null)
        {
            if(ConnectedObject.transform.position.y < 0)
            {
                ConnectedObject.transform.position = new Vector3(ConnectedObject.transform.position.x, 0, ConnectedObject.transform.position.z);
            }

            if (!_triggerPressed && ConnectedObject.transform.tag == "Container")
            {
                //Debug.Log("Released trigger-button -> Releasing attached object (" + ConnectedObject.name + ")");
                if (ConnectedObject.gameObject.GetComponent<Rigidbody>())
                {
                    ConnectedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
				TossObject();
            }
            else if (!_gripPressed && ConnectedObject.transform.tag == "TagGun")
            {
                if (ConnectedObject.gameObject.GetComponent<Rigidbody>())
                {
                    ConnectedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
				TossObject();
	            _tagGun.IsTagGunEquipped = false;
            }
        }

	    if (_tagGun.IsTagGunEquipped && _triggerPressed)
	    {
		    Debug.Log("Priming the tag gun");
			_tagGun.PrimeTagGun();
	    }
    }

	private void TossObject()
	{
		ConnectedObject.GetComponent<Rigidbody>().velocity = InputDevice.velocity;
		ConnectedObject.GetComponent<Rigidbody>().angularVelocity = InputDevice.angularVelocity;
		ConnectedObject.transform.parent = null;
		ConnectedObject = null;
	}

	private void ButtonAction(ButtonPressed btn, bool pressed)
    {
        if (btn == ButtonPressed.Grip)
        {
            _gripPressed = pressed;
        }
        else if (btn == ButtonPressed.Trigger)
        {
            _triggerPressed = pressed;
        }
    }

	public void OnTriggerStay(Collider col)
    {

        if(_triggerPressed && col.transform.tag == "Container" && ConnectedObject == null)
        {
            ConnectedObject = col.transform.gameObject;
            ConnectedObject.transform.parent = HandModel.transform;
            col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (_gripPressed && col.gameObject.tag == "TagGun" && ConnectedObject == null)
		{
			ConnectedObject = col.transform.gameObject;
			ConnectedObject.transform.parent = HandModel.transform;
			col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			Debug.Log("Tag Gun Equipped");
			_tagGun.IsTagGunEquipped = true;
			GetComponentInChildren<SteamVR_RenderModel>().enabled = false;
		}
    }
}
