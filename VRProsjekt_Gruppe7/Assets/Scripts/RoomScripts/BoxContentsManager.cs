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

    private Dictionary<string, int> _itemValues;
    private Dictionary<string, int> _itemSizeValues;

    private readonly int _maxBoxSpace = 100;
    private readonly string _itemsSettingsEditorPath = "Assets/Resources/ItemsDB.txt";

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
        List<GameObject> contentsList = new List<GameObject>();
        int boxSpaceLeft = _maxBoxSpace;
        int maxRetries = 10;


        while(boxSpaceLeft > 0 || maxRetries > 0)
        { 
            int objIndex = Random.Range(0, Contents.Length);

            if (!ValidItem(objIndex, boxSpaceLeft))
            {
                maxRetries--;
                continue;
            }

            GameObject gO = (GameObject)Instantiate(
                Contents[objIndex],
                curBox.transform.position + _spawnOffset[Random.Range(0, _spawnOffset.Length)],
                Quaternion.identity);

            string key = gO.GetComponent<ItemInfo>().ItemName;
            int value;
            int sizeValue;

            _itemValues.TryGetValue(key, out value);
            _itemSizeValues.TryGetValue(key, out sizeValue);

            gO.GetComponent<ItemInfo>().SetValues(curBox, value, sizeValue);
            gO.transform.parent = curBox.transform;

            boxSpaceLeft -= sizeValue;
            contentsList.Add(gO);
            _spawnedContents.Add(gO);
        }

        curBox.GetComponent<BoxInfo>().AddBoxContents(contentsList.ToArray());
    }

    private bool ValidItem(int objIndex, int boxSpaceLeft)
    {
        string itemName = Contents[objIndex].GetComponent<ItemInfo>().ItemName;

        if (!_itemSizeValues.ContainsKey(itemName))
            return false;

        int sizeValue = -1;
        _itemSizeValues.TryGetValue(itemName, out sizeValue);

        return (sizeValue >= 0 && sizeValue <= boxSpaceLeft);
    }
    
    private void SetCorretItemValues()
    {
        foreach (GameObject gO in Contents)
        {
            ItemInfo itemInfo = gO.GetComponent<ItemInfo>();

            if (itemInfo && _itemValues.ContainsKey(itemInfo.ItemName))
                itemInfo.Value = _itemValues[itemInfo.ItemName];
            else
                Debug.LogError("No item with that name found in database!");
        }
    }

    private void InitItemsFromFile()
    {
        _itemValues = new Dictionary<string, int>();
        _itemSizeValues = new Dictionary<string, int>();

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
                _itemSizeValues.Add(words[0], int.Parse(words[2]));
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
                    _itemSizeValues.Add(words[0], int.Parse(words[2]));
                }
            }
            while (currentLine != null);

            streamReader.Close();
        }
    }
}
