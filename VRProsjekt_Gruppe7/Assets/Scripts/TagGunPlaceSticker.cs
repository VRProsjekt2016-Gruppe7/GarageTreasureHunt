using UnityEngine;
using System.Collections;

public class TagGunPlaceSticker : MonoBehaviour {

    public GameObject Sticker;

    private bool StickerAssigned;

	// Use this for initialization
	void Start () {
	    if(Sticker == null)
        {
            StickerAssigned = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!StickerAssigned) return;

	}

    public void StickToObject(Collider target)
    {
        Instantiate(Sticker, target.transform);
    }
    
}
