using UnityEngine;
using System.Collections;

public class TagGunPlaceSticker : MonoBehaviour {

    public GameObject Sticker;

    private bool StickerAssigned;

	public AudioClip StickSound;

	// Use this for initialization
	public void Start () {
	    if(Sticker == null)
        {
            StickerAssigned = false;
        }
	}
	
	// Update is called once per frame
	public void Update () {
        if (!StickerAssigned) return;

	}

    public void StickToObject(Collision target)
    {
		// Attach sticker
        var newSticker = (GameObject)Instantiate(Sticker, target.transform);
	    newSticker.transform.position = target.contacts[0].point;

		// Play audioclip
	    var audioSouce = GetComponent<AudioSource>();
		audioSouce.PlayOneShot(StickSound);
    }
    
}
