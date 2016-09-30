using UnityEngine;
using System.Collections;

public class BoxInfo : MonoBehaviour {

    public GameObject[] _boxContents;

    public void AddBoxContents(GameObject[] contents)
    {
        _boxContents = new GameObject[contents.Length];
        _boxContents = contents;
    }
}
