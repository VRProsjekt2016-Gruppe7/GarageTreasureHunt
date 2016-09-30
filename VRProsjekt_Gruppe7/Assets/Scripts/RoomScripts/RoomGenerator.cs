using UnityEngine;
using System.Collections;
using System;

public class RoomGenerator : MonoBehaviour {

    // Left -> Front -> Right
    public GameObject ShelfPrefab;

    private Vector3[] _shelvesPositions = { new Vector3(-0.9f, 0, 0), new Vector3(0, 0, 0.9f), new Vector3(0.9f, 0, 0) };
    private GameObject[] _shelves;

    void Awake()
    {
        _shelves = new GameObject[_shelvesPositions.Length];
    }

    void Start()
    {
        GenerateRoom();
    }

    public void GenerateRoom()
    {
        SpawnShelves();
        GetComponent<BoxContentsManager>().FillBoxes(
            GetComponent<BoxesManager>().GenerateBoxes(_shelves),
            GetComponent<BoxesManager>().GetBoxDimesions());
    }

    private void SpawnShelves ()
    {
	    for(int i = 0; i < _shelvesPositions.Length; i++)
        {
            //Debug.Log("Spawning shelf nr: " + i);
            RoomSide side = GetRoomSide(i);

            _shelves[i] = (GameObject)Instantiate(ShelfPrefab, transform.position, transform.rotation);
            _shelves[i].GetComponent<ShelfManager>().PlaceObject(_shelvesPositions[i], 2f, side);
        }
	}

    private RoomSide GetRoomSide(int i)
    {
        if(i == 0)
                return RoomSide.Left;
        else if (i == 2)
            return RoomSide.Right;
        else
            return RoomSide.Front;
    }
}
