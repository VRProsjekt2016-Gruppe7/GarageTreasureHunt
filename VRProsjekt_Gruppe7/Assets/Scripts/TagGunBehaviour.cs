using UnityEngine;

namespace Assets.Scripts
{
    public class TagGunBehaviour : MonoBehaviour
    {
        public int NumStickers = 7;

        public bool IsTagGunEquipped;
        public bool IsPrimed;
        public bool HasStickers = true;
        private TagGunPlaceSticker _placeSticker;
        private SoundController _sC;

        public TextMesh GripToStartText;
        public Transform _cameraTransform = null;

	    void Start()
	    {
	        _sC = FindObjectOfType<SoundController>();

        }

        public void Init(int nrOfSticker)
        {
            _placeSticker = GetComponent<TagGunPlaceSticker>();
            HasStickers = true;
        }

        public void Update()
        {
            // Constraint, don't let the player drop the tag gun through the floor.
            if (transform.position.y < transform.localScale.y / 2.0f && GetComponent<Rigidbody>().isKinematic)
            {
                transform.position = new Vector3(transform.position.x, transform.localScale.y / 2f, transform.position.z);
            }

            // Update rotation of the text mesh
            GripToStartText.transform.rotation = _cameraTransform.rotation; 
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
                _placeSticker.StickToObject(col);
            }

            if (NumStickers <= 0)
            {
                HasStickers = false;
            }
            Debug.Log("Collision happend with " + col.transform.tag);
        }
    }
}
