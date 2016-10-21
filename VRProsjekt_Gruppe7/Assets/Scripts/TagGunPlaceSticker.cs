using UnityEngine;

namespace Assets.Scripts
{
	public class TagGunPlaceSticker : MonoBehaviour
	{

		public GameObject Sticker;
		public AudioClip StickSound;

		private bool _stickerAssigned;
		private GameManager _gM;
		public bool TagGunPickedUpFirstTime = false;


		public void Awake( )
		{
			_gM = FindObjectOfType<GameManager>();
		}

		// Use this for initialization
		public void Start( )
		{
			if (Sticker == null)
			{
				_stickerAssigned = false;
			}
			if (TagGunPickedUpFirstTime)
				_gM.StartGame();
		}

		// Update is called once per frame
		public void Update( )
		{
		}

		public void StickToObject( Collision target )
		{
			// Attach sticker
			var newSticker = (GameObject)Instantiate( Sticker, target.transform );
			newSticker.transform.position = target.contacts[0].point;

			// Play audioclip
			var audioSouce = GetComponent<AudioSource>();
			audioSouce.PlayOneShot( StickSound );

			//_gM.AddScore( target.transform.parent.GetComponent<BoxInfo>().TotalBoxValue );
		}

	}
}
