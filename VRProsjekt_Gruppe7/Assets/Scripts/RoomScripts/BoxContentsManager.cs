using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class BoxContentsManager : MonoBehaviour {

    public GameObject[] Contents;
    public int Max = 4;
    private Vector3 _spawnOffset;

    void Awake()
    {
        _spawnOffset = Vector3.zero;
    }

    public void FillBoxes(List<GameObject> allBoxes, Vector3 boxDimensions)
    {
        _spawnOffset = new Vector3(0, boxDimensions.y / 2, 0);

        for(int i = 0; i < allBoxes.Count; i++)
        {
            FillBox(allBoxes[i]);
        }
    }

    private void FillBox(GameObject curBox)
    {
        GameObject[] contents = new GameObject[Max];

        for(int i = 0; i < Max; i++)
        {
            GameObject gO = (GameObject) Instantiate(
                Contents[Random.Range(0, Contents.Length)], 
                curBox.transform.position  + _spawnOffset, 
                curBox.transform.rotation);
            contents[i] = gO;
            contents[i].AddComponent<ItemInfo>().SetValue(Random.Range(1, 100));

        }
        curBox.GetComponent<BoxInfo>().AddBoxContents(contents);
    }
}
