using UnityEngine;
using NewtonVR;
using System;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TagGunBehaviour : MonoBehaviour
    {
        public int NumStickers = 7;
	
        public bool IsPrimed;
        public bool HasStickers = true;
        public bool IsPickedUpFirstTime = false;
        private TagGunPlaceSticker _placeSticker;
        private SoundController _sC;

		public GameObject GripToStartText;
		public Transform CameraTransform;

        public Text display;

		private NVRInteractableItem nvrInteractable;

	    void Start()
	    {
	        _sC = FindObjectOfType<SoundController>();
			nvrInteractable = GetComponent<NVRInteractableItem> ();
        }

        public void Init(int nrOfSticker)
        {
            _placeSticker = GetComponent<TagGunPlaceSticker>();
            HasStickers = true;
        }

        public void Update()
        {
            /*
			if (FindObjectOfType<GameManager> ()._currentState == State.Running)
				GripToStartText.GetComponent<MeshRenderer> ().enabled = false;

    
            // Update rotation of the text mesh
			GripToStartText.transform.rotation = CameraTransform.rotation;
		    */
			// TODO start game DONE, check if works WORKS
            if(nvrInteractable.AttachedHand != null && !IsPickedUpFirstTime)
            {
                IsPickedUpFirstTime = true;
                FindObjectOfType<GameManager>().MaualStart = true;
            }

            // TODO prime tag gun when triggeris pressed DONE check if works WORKS
            if (nvrInteractable.AttachedHand != null && nvrInteractable.AttachedHand.HoldButtonPressed == true && nvrInteractable.AttachedHand.UseButtonDown) {
				PrimeTagGun ();
			}
            //Mattias was here
            //added siplay to the tag gun created method to update
            UpdateDipslay();

		}

        private void UpdateDipslay()
        {
            display.text = NumStickers.ToString();
        }

        public void PrimeTagGun()
        {
			if (!HasStickers)
				return;	

            IsPrimed = true;
            _sC.PlaySoundAtSourceOnce(SoundSource.TagGun, Sounds.PrimeGun);
        }

        public void OnCollisionEnter(Collision col)
        {
            if (col.transform.tag != "Container" || !HasStickers)
                return;

            if (col.transform.GetComponent<BoxInfo>().HasSticker)
                return;

            if (IsPrimed)
            {
                col.transform.GetComponent<BoxInfo>().HasSticker = true;
                NumStickers--;
                IsPrimed = false;
				_placeSticker.StickToObject(col.gameObject);
            }

            if (NumStickers <= 0)
            {
                HasStickers = false;
            }
            Debug.Log("Collision happend with " + col.transform.tag);
        }

		public void OnTriggerEnter(Collider col)
		{
			if (col.transform.tag != "Container" || !HasStickers)
				return;

			if (col.transform.GetComponent<BoxInfo>().HasSticker)
				return;

			if (IsPrimed)
			{
				col.transform.GetComponent<BoxInfo>().HasSticker = true;
				NumStickers--;
				IsPrimed = false;
				_placeSticker.StickToObject(col.gameObject);
			}
			if (NumStickers <= 0)
			{
				HasStickers = false;
			}
			Debug.Log("Trigger Happend " + col.transform.tag);
		}

    }
}
