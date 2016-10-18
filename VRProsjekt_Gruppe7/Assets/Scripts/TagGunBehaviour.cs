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

		public void Start ()
		{
			Debug.Log("The Tag Gun Initialized");

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
			IsPrimed = true;
		}

		public void OnTriggerEnter(Collider col)
		{
			if (IsPrimed && col.tag == "Container")
			{
				NumStickers--;
				IsPrimed = false;
			}
			if (NumStickers >= 0)
			{
				HasStickers = false;
			}
			// TODO place sticker on container
		}
	}
}
