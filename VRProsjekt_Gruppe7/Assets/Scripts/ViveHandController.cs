using UnityEngine;
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

	public bool _triggerPressedLeft = false;
	public bool _gripPressedLeft = false;

	public bool _triggerPressedRight = false;
	public bool _gripPressedRight = false;

    private Valve.VR.EVRButtonId _triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId _gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;

    private SteamVR_Controller.Device InputDevice { get { return SteamVR_Controller.Input((int)_trackedObj.index); } }
    private SteamVR_TrackedObject _trackedObj;

    private TagGunBehaviour _tagGunBehaviour;
    private TagGunPlaceSticker _tagGunPlaceSticker;

	private GameObject leftCtrl;
	private GameObject rightCtrl;

    //  void Awake()
    private void Start()
    {
        _tagGunPlaceSticker = GameObject.FindGameObjectWithTag("TagGun").GetComponent<TagGunPlaceSticker>();
        _tagGunBehaviour = GameObject.FindGameObjectWithTag("TagGun").GetComponent<TagGunBehaviour>();
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
		leftCtrl = GameObject.FindGameObjectWithTag("ControllerLeft");
		rightCtrl = GameObject.FindGameObjectWithTag ("ControllerRight");

    }

    private void Update()
    {
        if (InputDevice == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        CheckButtons();

		// Quick and dirty, fix later (maybe)
		var rightVive = rightCtrl.GetComponent<ViveHandController> ();
		var leftVive = leftCtrl.GetComponent<ViveHandController> ();

		if (CurrentHand == Hand.Left) {
			rightVive._triggerPressedLeft = _gripPressedLeft;
			rightVive._gripPressedLeft = _gripPressedLeft;
		}

		if (CurrentHand == Hand.Right) {
			leftVive._triggerPressedRight = _gripPressedRight;
			leftVive._gripPressedRight = _gripPressedRight;
		}
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

		if (ConnectedObjectLeft != null)
        {
			/*
            if (ConnectedObjectLeft.transform.position.y < 0)
            {
				ConnectedObjectLeft.transform.position = new Vector3(ConnectedObjectLeft.transform.position.x, 0, ConnectedObjectLeft.transform.position.z);
            }
            */

			if (!_triggerPressedLeft && ConnectedObjectLeft.transform.tag == "Container")
            {
				if (ConnectedObjectLeft.gameObject.GetComponent<Rigidbody>())
                {
					ConnectedObjectLeft.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
                TossObjectLeft();
            }
			else if (!_gripPressedLeft && ConnectedObjectLeft.transform.tag == "TagGun")
            {
				if (ConnectedObjectLeft.gameObject.GetComponent<Rigidbody>())
                {
					ConnectedObjectLeft.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
                TossObjectLeft();
            }
        }

		if (ConnectedObjectRight != null)
		{
			if (ConnectedObjectRight.transform.position.y < 0)
			{
				ConnectedObjectRight.transform.position = new Vector3(ConnectedObjectRight.transform.position.x, 0, ConnectedObjectRight.transform.position.z);
			}

			if (!_triggerPressedRight && ConnectedObjectRight.transform.tag == "Container")
			{
				if (ConnectedObjectRight.gameObject.GetComponent<Rigidbody>())
				{
					ConnectedObjectRight.gameObject.GetComponent<Rigidbody>().isKinematic = false;
				}
				TossObjectRight();
			}
			else if (!_gripPressedRight && ConnectedObjectRight.transform.tag == "TagGun")
			{
				if (ConnectedObjectRight.gameObject.GetComponent<Rigidbody>())
				{
					ConnectedObjectRight.gameObject.GetComponent<Rigidbody>().isKinematic = false;
				}
				TossObjectRight();
			}
		}
			
		if (_triggerPressedLeft && !_tagGunBehaviour.IsPrimed)
        {
            Debug.Log("Priming the tag gun");
			_tagGunBehaviour.PrimeTagGun();
        }
		else if (_triggerPressedRight && !_tagGunBehaviour.IsPrimed)
		{
			Debug.Log("Priming the tag gun");
			_tagGunBehaviour.PrimeTagGun();
		}
    }


    private void TossObjectLeft()
    {
		ConnectedObjectLeft.GetComponent<Rigidbody>().velocity = InputDevice.velocity;
		ConnectedObjectLeft.GetComponent<Rigidbody>().angularVelocity = InputDevice.angularVelocity;
		ConnectedObjectLeft.transform.parent = null;
		ConnectedObjectLeft = null;
    }

	private void TossObjectRight()
	{
		ConnectedObjectRight.GetComponent<Rigidbody>().velocity = InputDevice.velocity;
		ConnectedObjectRight.GetComponent<Rigidbody>().angularVelocity = InputDevice.angularVelocity;
		ConnectedObjectRight.transform.parent = null;
		ConnectedObjectRight = null;
	}

    private void ButtonAction(ButtonPressed btn, bool pressed)
    {
		if (btn == ButtonPressed.Grip && CurrentHand == Hand.Left)
        {
            _gripPressedLeft = pressed;
        }
		else if (btn == ButtonPressed.Trigger && CurrentHand == Hand.Left)
        {
			_triggerPressedLeft = pressed;
        }

		if (btn == ButtonPressed.Grip && CurrentHand == Hand.Right)
		{
			_gripPressedRight = pressed;
		}
		else if (btn == ButtonPressed.Trigger && CurrentHand == Hand.Right)
		{
			_triggerPressedRight = pressed;
		}
    }

    public void OnTriggerExit(Collider col)
    {
        print(col.name + ", " + col.tag);
		if (col.transform.tag == "BoxLid" && ConnectedObjectLeft == null)
        {
            col.GetComponent<LidScript>().CloseLid();
        }
    }

    public void OnTriggerStay(Collider col)
    {
		if (_triggerPressedLeft)
        {
			if (col.transform.tag == "BoxLid" && ConnectedObjectLeft == null)
            {
                col.GetComponent<LidScript>().OpenLid();
            }
			else if (col.transform.tag == "Container" && ConnectedObjectLeft == null && !testOpenLid)
            {
				ConnectedObjectLeft = col.transform.gameObject;
				ConnectedObjectLeft.transform.parent = HandModel.transform;
                col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
		if (_triggerPressedRight) 
		{
			if (col.transform.tag == "BoxLid" && ConnectedObjectRight == null)
			{
				col.GetComponent<LidScript>().OpenLid();
			}
			else if (col.transform.tag == "Container" && ConnectedObjectRight == null && !testOpenLid)
			{
				ConnectedObjectRight = col.transform.gameObject;
				ConnectedObjectRight.transform.parent = HandModel.transform;
				col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			}
		}

		// SPECIAL CASE WHERE THE PLAYER TRIES TO TRADE CONNECTEDOBJECTS
		if (_triggerPressedLeft && ConnectedObjectLeft == null && ConnectedObjectRight != null && ConnectedObjectRight == col.gameObject) 
		{
			ConnectedObjectLeft = col.transform.gameObject;
			ConnectedObjectLeft.transform.parent = HandModel.transform;
			col.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
			ConnectedObjectRight = null;
		}
		if (_triggerPressedRight && ConnectedObjectRight == null && ConnectedObjectLeft != null && ConnectedObjectLeft == col.gameObject) 
		{
			ConnectedObjectRight = col.transform.gameObject;
			ConnectedObjectRight.transform.parent = HandModel.transform;
			col.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
			ConnectedObjectLeft = null;
		}


		if (_gripPressedLeft && col.gameObject.tag == "TagGun" && ConnectedObjectLeft == null && CurrentHand == Hand.Left )
        {
			ConnectedObjectLeft = col.transform.gameObject;
			ConnectedObjectLeft.transform.parent = HandModel.transform;
            col.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            if (!_tagGunPlaceSticker.TagGunPickedUpFirstTime)
            {
                _tagGunPlaceSticker.TagGunPickedUpFirstTime = true;
  //              FindObjectOfType<GameManager>().MaualStart = true;
            }

            Debug.Log("Tag Gun Equipped");

            if (!_tagGunPlaceSticker.TagGunPickedUpFirstTime)
            {
                _tagGunPlaceSticker.TagGunPickedUpFirstTime = true;
//                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().MaualStart = true;
				_tagGunBehaviour.GripToStartText.GetComponent<TextMesh> ().text = "";
            }
		}

		if (_gripPressedRight && col.gameObject.tag == "TagGun" && ConnectedObjectRight == null && CurrentHand == Hand.Right)
		{
			ConnectedObjectRight = col.transform.gameObject;
			ConnectedObjectRight.transform.parent = HandModel.transform;
			col.gameObject.GetComponent<Rigidbody>().isKinematic = true;

			if (!_tagGunPlaceSticker.TagGunPickedUpFirstTime)
			{
				_tagGunPlaceSticker.TagGunPickedUpFirstTime = true;
//				FindObjectOfType<GameManager>().MaualStart = true;
			}

			Debug.Log("Tag Gun Equipped");

			if (!_tagGunPlaceSticker.TagGunPickedUpFirstTime)
			{
				_tagGunPlaceSticker.TagGunPickedUpFirstTime = true;
//				GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().MaualStart = true;
				_tagGunBehaviour.GripToStartText.GetComponent<TextMesh> ().text = "";
			}
		}
    }
}
