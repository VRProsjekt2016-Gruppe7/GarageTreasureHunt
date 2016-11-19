using UnityEngine;

namespace Assets.Scripts
{
    public class TagGunPlaceSticker : MonoBehaviour
    {
        public GameObject Sticker;

        private GameManager _gM;
        private SoundController _sC;


        public void Start()
        {
            _sC = FindObjectOfType<SoundController>();
            _gM = FindObjectOfType<GameManager>();
        }

//		public void StickToObject( Collision target )
        public void StickToObject(GameObject box)
        {
            BoxInfo boxInfo = box.GetComponent<BoxInfo>();
            Vector3 stickerPos = boxInfo.StickerPoint.transform.position;

			// Attach sticker
            var newSticker = (GameObject)Instantiate( Sticker, stickerPos, new Quaternion(0, box.transform.rotation.y,0,1));
		    newSticker.transform.parent = box.transform;

            // Add score
			_gM.AddScore(boxInfo.TotalBoxValue );

            // Play audioclip
            _sC.PlaySoundAtSourceOnce(SoundSource.TagGun, Sounds.PlaceSticker);
        }
    }
}
