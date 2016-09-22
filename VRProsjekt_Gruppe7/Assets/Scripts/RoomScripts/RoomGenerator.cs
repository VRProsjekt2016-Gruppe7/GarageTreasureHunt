using UnityEngine;
using System.Collections;
using System;

public class RoomGenerator : MonoBehaviour {

    // Left -> Front -> Right
    private Vector3[] shelvesPositions = { new Vector3(-0.9f, 0, 0), new Vector3(0, 0, 0.9f), new Vector3(0.9f, 0, 0) };
    public GameObject shelfPrefab;
    public GameObject[] shelves;

    void Awake()
    {
        shelves = new GameObject[shelvesPositions.Length];
    }

    void Start()
    {
        GenerateRoom();
    }

    public void GenerateRoom()
    {
        SpawnShelves();
        GetComponent<BoxesManager>().GenerateBoxes(shelves);
    }

    public void SpawnShelves () {
	    for(int i = 0; i < shelvesPositions.Length; i++)
        {
            RoomSide side = GetRoomSide(i);

            shelves[i] = (GameObject)Instantiate(shelfPrefab, transform.position, transform.rotation);
            shelves[i].GetComponent<ShelfManager>().PlaceObject(shelvesPositions[i], 2f, side);
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
