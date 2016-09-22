using UnityEngine;

namespace Assets.Scripts
{
	public class TagGunBehaviour : MonoBehaviour {

		public int NumStickers { get; set; }	

		private void Start ()
		{
			Debug.Log("The Tag Gun Initialized");
			NumStickers = 10;
		}

		private void Update ()
		{
			// Constraint, don't let the player drop the tag gun through the floor.
			if (transform.position.y < transform.localScale.y/2.0f && GetComponent<Rigidbody>().isKinematic)
			{
				transform.position = new Vector3(transform.position.x, transform.localScale.y/2f, transform.position.z);
			}
		}
	}
}
