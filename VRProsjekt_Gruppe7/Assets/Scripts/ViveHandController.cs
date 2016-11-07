﻿using UnityEngine;
using Assets.Scripts;


public enum ButtonPressed
{
    Grip,
    Trigger
}

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ViveHandController : HandController
{

    public GameObject HandModel;

    public bool testOpenLid = false;

	// Public for test purposes
    public bool _triggerPressed = false;
    public bool _gripPressed = false;

    private Valve.VR.EVRButtonId _triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId _gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;

    private SteamVR_Controller.Device InputDevice { get { return SteamVR_Controller.Input((int)_trackedObj.index); } }
    private SteamVR_TrackedObject _trackedObj;

    private TagGunBehaviour _tagGunBehaviour;
    private TagGunPlaceSticker _tagGunPlaceSticker;

    //  void Awake()
    private void Start()
    {
        _tagGunPlaceSticker = GameObject.FindGameObjectWithTag("TagGun").GetComponent<TagGunPlaceSticker>();
        _tagGunBehaviour = GameObject.FindGameObjectWithTag("TagGun").GetComponent<TagGunBehaviour>();
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

        if (ConnectedObject != null)
        {
            if (ConnectedObject.transform.position.y < 0)
            {
                ConnectedObject.transform.position = new Vector3(ConnectedObject.transform.position.x, 0, ConnectedObject.transform.position.z);
            }

            if (!_triggerPressed && ConnectedObject.transform.tag == "Container")
            {
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
                _tagGunBehaviour.IsTagGunEquipped = false;
            }
        }

        if (_tagGunBehaviour.IsTagGunEquipped && _triggerPressed && !_tagGunBehaviour.IsPrimed)
        {
            Debug.Log("Priming the tag gun");
            _tagGunBehaviour.PrimeTagGun();
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

    public void OnTriggerExit(Collider col)
    {

        print(col.name + ", " + col.tag);
        if (col.transform.tag == "BoxLid" && ConnectedObject == null)
        {
            col.GetComponent<OpenBox>().Close();
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.transform.tag == "BoxLid" && ConnectedObject == null)
        {
            col.GetComponent<OpenBox>().Open(transform);
            return;
        }

        if (_triggerPressed)
        {
            /*
            if (col.transform.tag == "BoxLid" && ConnectedObject == null)
            {
                col.GetComponent<LidScript>().OpenLid();
            }
            else
            */ 
            if (col.transform.tag == "Container" && ConnectedObject == null && !testOpenLid)
            {
                ConnectedObject = col.transform.gameObject;
                ConnectedObject.transform.parent = HandModel.transform;
                col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        if (_gripPressed && col.gameObject.tag == "TagGun" && ConnectedObject == null)
        {
            ConnectedObject = col.transform.gameObject;
            ConnectedObject.transform.parent = HandModel.transform;
            col.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            if (!_tagGunPlaceSticker.TagGunPickedUpFirstTime)
            {
                _tagGunPlaceSticker.TagGunPickedUpFirstTime = true;
                FindObjectOfType<GameManager>().MaualStart = true;
				_tagGunBehaviour.GetComponent<MeshRenderer>().enabled = false;
            }

            Debug.Log("Tag Gun Equipped");

            if (!_tagGunPlaceSticker.TagGunPickedUpFirstTime)
            {
                _tagGunPlaceSticker.TagGunPickedUpFirstTime = true;
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().MaualStart = true;
                Destroy(GameObject.FindGameObjectWithTag("Spawn Pillar")); //Destroys the object with the two boxes that are present before the game starts.

            }
            col.GetComponent<MeshRenderer>().enabled = false;

            _tagGunBehaviour.IsTagGunEquipped = true;
        }
    }
}
