using UnityEngine;
using NewtonVR;
using System;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TagGunBehaviour : MonoBehaviour
    {
        // NEW OVERIDE TO START GAME!!!
        public bool ForceGameStart = false;
        // END NEW FORCE START

        public int NumStickers;
	
        public bool IsPrimed;
        public bool HasStickers = true;
        public bool IsPickedUpFirstTime = false;

		public GameObject GripToStartText;
		public Transform CameraTransform;
        public GameObject Sticker;

        public Text TagGunDisplay;

        private SoundController _sC;
        private NVRInteractableItem _nvrInteractable;
        private GameManager _gM;

        private readonly Vector3 _tagGunStartPos = new Vector3(0f, 0.81f, 0.91f);
        private readonly Vector3 _tagGunStartRot = new Vector3(270f, 180f, 0f);
        
        void Start()
        {
            _gM = FindObjectOfType<GameManager>();
            _sC = FindObjectOfType<SoundController>();
			_nvrInteractable = GetComponent<NVRInteractableItem> ();
        }

        public void Init(int nrOfStickers)
        {
            NumStickers = nrOfStickers;
            HasStickers = true;
            IsPickedUpFirstTime = false;
            ResetTagGun();
        }

        private void ResetTagGun()
        {
            if (transform.parent)
                transform.parent = null;

            transform.position = _tagGunStartPos;
            transform.eulerAngles = _tagGunStartRot;
        }

        public void Update()
        {
            if((ForceGameStart || _nvrInteractable.AttachedHand != null) && !IsPickedUpFirstTime)
            {
                ForceGameStart = false;
                IsPickedUpFirstTime = true;
                FindObjectOfType<GameManager>().StartGame();
            }

            if (_nvrInteractable.AttachedHand != null && _nvrInteractable.AttachedHand.HoldButtonPressed == true && _nvrInteractable.AttachedHand.UseButtonDown) {
				PrimeTagGun ();
			}

            UpdateDipslay();

		}

        private void UpdateDipslay()
        {
            TagGunDisplay.text = NumStickers.ToString();
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
				StickToObject(col.gameObject);
            }

            if (NumStickers <= 0)
            {
                HasStickers = false;
            }

            if (!HasStickers)
            {
                FindObjectOfType<GameManager>().EndGame();
            }
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
				StickToObject(col.gameObject);
            }

            if (NumStickers <= 0)
            {
                HasStickers = false;
            }

		    if (!HasStickers)
		    {
                FindObjectOfType<GameManager>().EndGame();
            }
        }

        public void StickToObject(GameObject box)
        {
            BoxInfo boxInfo = box.GetComponent<BoxInfo>();
            Vector3 stickerPos = boxInfo.StickerPoint.transform.position;

            // Attach sticker
            var newSticker = (GameObject)Instantiate(Sticker, stickerPos, Quaternion.identity);
            newSticker.transform.Rotate(box.transform.localEulerAngles);
            newSticker.transform.parent = box.transform;

            // Add score
            _gM.AddScore(boxInfo.TotalBoxValue);

            // Play audioclip
            _sC.PlaySoundAtSourceOnce(SoundSource.TagGun, Sounds.PlaceSticker);
        }
    }
}
