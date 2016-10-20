using System;
using UnityEngine;
using UnityEngineInternal;

namespace Assets.Scripts
{
	public class TagGunBehaviour : MonoBehaviour
	{
		public int NumStickers = 7;

		public bool IsTagGunEquipped;
		public bool IsPrimed;
		public bool HasStickers = true;

		private TagGunPlaceSticker _placeSticker;

		public void Start ()
		{
			_placeSticker = GetComponent<TagGunPlaceSticker>();
			Debug.Log("The Tag Gun Initialized");
			HasStickers = true;
		}

		public void Update ()
		{
			// Constraint, don't let the player drop the tag gun through the floor.
			if (transform.position.y < transform.localScale.y/2.0f && GetComponent<Rigidbody>().isKinematic)
			{
				transform.position = new Vector3(transform.position.x, transform.localScale.y/2f, transform.position.z);
			}
		}

		public void PrimeTagGun()
		{
			if (!HasStickers) return;
			IsPrimed = true;
			var audioSource = GetComponent<AudioSource>();
			audioSource.Play();
		}


		// Unsure if OnTriggerEnter or OnCollisionEnter is best.
		/*
		public void OnTriggerEnter(Collider col)
		{
			if (IsPrimed && col.tag == "Container")
			{
				NumStickers--;
				IsPrimed = false;
				//_placeSticker.StickToObject( collision );
			}
			if (NumStickers <= 0)
			{
				HasStickers = false;
			}
			Debug.Log( "On Trigger Enter happend" );
		}
		*/

		public void OnCollisionEnter( Collision collision)
		{
			if (IsPrimed && collision.gameObject.tag == "Container")
			{
				NumStickers--;
				IsPrimed = false;
				_placeSticker.StickToObject( collision );
			}
			if (NumStickers <= 0)
			{
				HasStickers = false;
			}
			Debug.Log("Collision happend");
		}
	}
}
