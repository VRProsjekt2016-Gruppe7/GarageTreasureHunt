using UnityEngine;
using System.Collections.Generic;

public class BoxInfo : MonoBehaviour
{
    public List<GameObject> BoxContents;
    public int TotalBoxValue = 0;
    public bool HasSticker = false;
    public GameObject StickerPoint;

    private bool _contentsVisible = true;

    public void AddBoxContents(GameObject[] contents)
    {
        BoxContents = new List<GameObject>();

        foreach (GameObject gO in contents)
        {
            BoxContents.Add(gO);
        }

        SetBoxValues();
        LidStatus(false);
    }

    public void LidStatus(bool open)
    {
        if (_contentsVisible == open)
            return;

        _contentsVisible = open;
        SetObjectVisibility(_contentsVisible);
    }

    private void SetObjectVisibility(bool visible)
    {

        foreach (GameObject gO in BoxContents)
        {
            gO.SetActive(visible);
        }
    }

    private void SetBoxValues()
    {
        foreach (var content in BoxContents)
        {
            TotalBoxValue += content.GetComponent<ItemInfo>().GetValue();
        }
    }

    public void RemoveMe(GameObject gO)
    {
        if (!BoxContents.Contains(gO) || !gO.GetComponent<ItemInfo>())
            return;

        int value = gO.GetComponent<ItemInfo>().Value;

        if (TotalBoxValue - value > 0)
        {
            TotalBoxValue -= value;
        }
        else
        {
            TotalBoxValue = 0;
        }

        BoxContents.Remove(gO);
        gO.transform.parent = null;
    }
}
