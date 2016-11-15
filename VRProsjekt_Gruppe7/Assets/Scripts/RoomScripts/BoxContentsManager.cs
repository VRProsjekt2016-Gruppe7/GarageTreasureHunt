using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.UI;

public class BoxContentsManager : MonoBehaviour
{

    public Text ErrorMessages;

    public GameObject[] Contents;
    public TextAsset ConfigFile;

    private int MaxItemsInBox = 1;

    private Dictionary<string, int> _itemValues;
    private Dictionary<string, int> _validItemsLeft;

    private readonly string _itemsSettingsEditorPath = "Assets/Resources/ItemsDB.txt";
//    private readonly string _itemsSettingsBuildPath = "ItemsDB.txt";
    private readonly Vector3[] _spawnOffset =
    {
        new Vector3(-0.05f, 0.08f, 0.05f),
        new Vector3(0.05f, 0.08f, 0.05f),
        Vector3.zero,
        new Vector3(-0.05f, 0.08f, -0.05f),
        new Vector3(0.05f, 0.08f, -0.05f)
    };

    private List<GameObject> _spawnedContents;

    public void Init()
    {
        InitItemsFromFile();
        SetCorretItemValues();
    }

    public void ClearContents()
    {
        if (_spawnedContents == null || _spawnedContents.Count == 0)
            return;

        foreach (var gO in _spawnedContents)
        {
            Destroy(gO);
        }
    }

    public void FillBoxes(List<GameObject> allBoxes)
    {
        _spawnedContents = new List<GameObject>();

        for (int i = 0; i < allBoxes.Count; i++)
        {
            FillBox(allBoxes[i]);
        }
    }

    private void FillBox(GameObject curBox)
    {
        if (MaxItemsInBox > 4)
            MaxItemsInBox = 4;

        GameObject[] contents = new GameObject[MaxItemsInBox];

        for (int i = 0; i < MaxItemsInBox; i++)
        {
            int objIndex = GetValidObject();

            if (objIndex == -1)
            {
                print("No more items! Get more prefabs!!!");
                break;
            }


            GameObject gO = (GameObject)Instantiate(
                Contents[objIndex],
                curBox.transform.position + _spawnOffset[i],
                Quaternion.identity);

            contents[i] = gO;

            contents[i].GetComponent<ItemInfo>().SetValue(curBox, contents[i].GetComponent<ItemInfo>().Value);
            contents[i].transform.parent = curBox.transform;
            _spawnedContents.Add(gO);
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
        _itemValues = new Dictionary<string, int>();
        _validItemsLeft = new Dictionary<string, int>();

        try
        {
            if (Application.isEditor)
            {
                EditorLoad();   
            }
            else
            {
                BuildLoad();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            //ErrorMessages.text = e.Message;
        }
    }

    private void BuildLoad()
    {
        StringReader read = new StringReader(ConfigFile.text);
        string currentLine;

        do
        {
            currentLine = read.ReadLine();
            if (currentLine != null)
            {
                string[] words = currentLine.Split(',');

                _itemValues.Add(words[0], int.Parse(words[1]));
                _validItemsLeft.Add(words[0], int.Parse(words[2]));
                //ErrorMessages.text = words[0] + ", " + words[1] + ", " + words[2];
            }
        }
        while (currentLine != null);
    }

    private void EditorLoad()
    {
        StreamReader streamReader = new StreamReader(_itemsSettingsEditorPath, Encoding.Default);

        using (streamReader)
        {
            string currentLine;

            do
            {
                currentLine = streamReader.ReadLine();
                if (currentLine != null)
                {
                    string[] words = currentLine.Split(',');

                    _itemValues.Add(words[0], int.Parse(words[1]));
                    _validItemsLeft.Add(words[0], int.Parse(words[2]));
                }
            }
            while (currentLine != null);

            streamReader.Close();
        }
    }
}
