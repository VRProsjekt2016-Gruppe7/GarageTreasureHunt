using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BoxesManager : MonoBehaviour {

    public GameObject boxPrefab;
    List<GameObject> AllBoxes;

    private readonly float _boxWidth = 0.3f;
    private readonly float _boxHeight = 0.3f;
    private readonly float _boxDepth = 0.3f;
    private readonly float _offSetWidthPrBox = 0.08f;
    private readonly float _shelfSideOffset = 0.22f;
    private float reqSpacePrBox;

    void Awake()
    {
        AllBoxes = new List<GameObject>();
        reqSpacePrBox = _boxWidth + _offSetWidthPrBox;
    }

    public void GenerateBoxes(GameObject[] shelves)
    {
        if (shelves.Length == 0)
        {
            Debug.Log("Aborting: No shelves to place boxes on!");
            return;
        }

        PlaceBoxesOnShelves(shelves);
    }

    private void PlaceBoxesOnShelves(GameObject[] shelves)
    {
        for(int i = 0; i < shelves.Length; i++)
        {
            if (shelves[i] == null)
                continue;

            PlaceBoxesOnShelf(shelves[i]);
        }
    }

    private void PlaceBoxesOnShelf(GameObject shelf)
    {
        int shelfFloors = shelf.GetComponent<ShelfManager>().GetNrOfShelfFloors();

        float startPosY = shelf.GetComponent<ShelfManager>().GetLowestShelfFloorY();
        float spaceBetweenShelfFloors = shelf.GetComponent<ShelfManager>().GetSpaceBetweenShelfFloors();
        float shelfWidth = shelf.transform.localScale.x - (_shelfSideOffset * 2);

        int boxesPrFloor = (shelfWidth > reqSpacePrBox) ? AmountOfBoxesEachFloor(shelfWidth) : 0 ;

        for (int floor = 0; floor < shelfFloors; floor++)
        {
            for (int boxNr = 0; boxNr < boxesPrFloor; boxNr++)
            {
                float curPosY =  startPosY + (floor * spaceBetweenShelfFloors);
                PlaceBoxOnShelf(shelf, curPosY, boxesPrFloor, shelfWidth);
            }
        }
    }

    private void PlaceBoxOnShelf(GameObject shelf, float curPosY, int boxesPrFloor, float shelfWidth)
    {
        float shelfRotY = shelf.transform.localRotation.y;

        for (int i = 1; i <= boxesPrFloor; i++)
        {
            Vector3 pos = GetBoxPos(shelf, curPosY, boxesPrFloor, shelfWidth, i);//new Vector3(x, curPosY, 0);

            GameObject newBox = (GameObject)Instantiate(boxPrefab, pos, transform.rotation);

            newBox.transform.Rotate(0,shelfRotY,0);

            AllBoxes.Add(newBox);
        }
    }

    private Vector3 GetBoxPos(GameObject shelf, float curPosY, float boxesPrFloor, float shelfWidth, int i)
    {
        float offsetPos = shelfWidth / boxesPrFloor;
        float x = shelf.transform.position.x;
        float y = curPosY;
        float z = 0;

        if(shelf.transform.localRotation.y == 0) // Shelf is on left
        {
            x = -_shelfSideOffset + (shelf.transform.position.x - (shelfWidth / 2)) + (i * offsetPos);
            z = shelf.transform.position.z;
        }
        else // Shelf is on a side
        {
            z = (-_shelfSideOffset) + (shelf.transform.position.z - (shelfWidth / 2)) + (i * offsetPos);
        }

        return new Vector3(x, y, z);
    }

    private int AmountOfBoxesEachFloor(float shelfWidth)
    {
        float spaceLeft = shelfWidth;
        int count = 0;

        while(spaceLeft > reqSpacePrBox)
        {
            count++;
            spaceLeft -= reqSpacePrBox;
        }

        return count;
    }
}
