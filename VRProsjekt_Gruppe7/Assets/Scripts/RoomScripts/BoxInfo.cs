using UnityEngine;
using System.Collections.Generic;

public class BoxInfo : MonoBehaviour {

    public List<GameObject> BoxContents;
    public int TotalBoxValue = 0;
    public bool HasSticker = false;

    public void AddBoxContents(GameObject[] contents)
    {
        BoxContents = new List<GameObject>();

        foreach (GameObject gO in contents)
        {
            BoxContents.Add(gO);
        }

        SetBoxValues();
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
    }
}
