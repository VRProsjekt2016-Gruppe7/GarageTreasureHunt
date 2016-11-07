using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class BoxContentsManager : MonoBehaviour {

    public GameObject[] Contents;
    private int MaxItemsInBox = 1;

    private Dictionary<string, int> _itemValues = new Dictionary<string, int>();
    private Dictionary<string, int> _validItemsLeft = new Dictionary<string, int>();

    private readonly string _itemsSettingsPath = "Assets/Config/ItemsDB.vri";
    private readonly Vector3[] _spawnOffset =
    {
        new Vector3(-0.05f, 0.13f, 0.05f),
        new Vector3(0.05f, 0.13f, 0.05f),
        Vector3.zero,
        new Vector3(-0.05f, 0.13f, -0.05f),
        new Vector3(0.05f, 0.13f, -0.05f)
    };

    public void Init()
    {
        InitItemsFromFile();
        SetCorretItemValues();
    }

    public void FillBoxes(List<GameObject> allBoxes)
    {
        for(int i = 0; i < allBoxes.Count; i++)
        {
            FillBox(allBoxes[i]);
        }
    }

    private void FillBox(GameObject curBox)
    {
        if (MaxItemsInBox > 4)
            MaxItemsInBox = 4;

        GameObject[] contents = new GameObject[MaxItemsInBox];

        for(int i = 0; i < MaxItemsInBox; i++)
        {
            int objIndex = GetValidObject();

            if (objIndex == -1)
            {
                print("No more items! Get more prefabs!!!");
                break;
            }


            GameObject gO = (GameObject) Instantiate(
                Contents[objIndex], 
                curBox.transform.position  + _spawnOffset[i], 
                Quaternion.identity);

            contents[i] = gO;

            contents[i].GetComponent<ItemInfo>().SetValue(curBox, contents[i].GetComponent<ItemInfo>().Value);
            contents[i].transform.parent = curBox.transform;

        }

        curBox.GetComponent<BoxInfo>().AddBoxContents(contents);
    }

    private int GetValidObject()
    {
        int pos = -1;

        if (_validItemsLeft.Count == 0)
            return pos;

        string validItem = "";

        do
        {
            pos = Random.Range(0, Contents.Length);
            string curItemName = Contents[pos].GetComponent<ItemInfo>().ItemName;

            if (_validItemsLeft.ContainsKey(curItemName))
            {
                validItem = curItemName;
                _validItemsLeft[validItem]--;

                if (_validItemsLeft[validItem] == 0)
                {
                    _validItemsLeft.Remove(validItem);
                }
            }
        }
        while (validItem == "");

        return pos;

    }

    private void SetCorretItemValues()
    {
        foreach (GameObject gO in Contents)
        {
            ItemInfo itemInfo = gO.GetComponent<ItemInfo>();

            if (itemInfo && _itemValues.ContainsKey(itemInfo.ItemName))
                itemInfo.Value = _itemValues[itemInfo.ItemName];
            else
                Debug.LogError("Herrooooo. No item by that name found in database!");
        }
    }


    private void InitItemsFromFile()
    {
        try {
            StreamReader streamReader = new StreamReader(_itemsSettingsPath, Encoding.Default);

            using (streamReader) {
                string currentLine;

                do {
                    currentLine = streamReader.ReadLine();
                    if (currentLine != null) {
                        string[] words = currentLine.Split(',');

                        _itemValues.Add(words[0], int.Parse(words[1]));
                        _validItemsLeft.Add(words[0], int.Parse(words[2]));
                    }
                }
                while (currentLine != null);

                streamReader.Close();
            }
        }
        catch (Exception e) {
            Debug.LogError(e.Message);
        }

        #region Debug print collected data from file
        /*
        foreach (KeyValuePair<string, int> itemWithValue in _itemValues)
        {
            print(itemWithValue);
            print(itemWithValue.Key + " has " + _validItemsLeft[itemWithValue.Key].ToString() + " items left.");
        }
        */
        #endregion
    }
}
