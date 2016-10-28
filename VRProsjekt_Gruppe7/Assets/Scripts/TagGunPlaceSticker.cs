using UnityEngine;

namespace Assets.Scripts
{
    public class TagGunPlaceSticker : MonoBehaviour
    {
        public GameObject Sticker;
        public bool TagGunPickedUpFirstTime = false;

        private GameManager _gM;
        private SoundController _sC;


        public void Start()
        {
            _sC = FindObjectOfType<SoundController>();
            _gM = FindObjectOfType<GameManager>();
        }

        public void StickToObject(Collision target)
        {
            //            if(!TagGunPickedUpFirstTime)

            // Attach sticker
            var newSticker = (GameObject)Instantiate(Sticker, target.transform.position, new Quaternion(0, target.transform.rotation.y, 0, 1));
            newSticker.transform.position = target.contacts[0].point;
            newSticker.transform.parent = target.transform;

            // Play audioclip
            _sC.PlaySoundAtSourceOnce(SoundSource.TagGun, Sounds.PlaceSticker);

            _gM.AddScore(target.transform.GetComponent<BoxInfo>().TotalBoxValue);
        }

    }
}
