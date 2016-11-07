using UnityEngine;
using System.Collections.Generic;

public class BoxesManager : MonoBehaviour {

    public GameObject BoxPrefab;

    private List<GameObject> _allBoxes;
    private readonly float _boxWidth = 0.3f;
    private readonly float _boxHeight = 0.3f;
    private readonly float _boxDepth = 0.3f;
    private readonly float _offSetWidthPrBox = 0.08f;
    private readonly float _shelfSideOffset = 0.22f;
    private float _reqSpacePrBox;
    private Vector3 _boxDimensions;

    public void Init()
    {
        _allBoxes = new List<GameObject>();
        _reqSpacePrBox = _boxWidth + _offSetWidthPrBox;
        _boxDimensions = new Vector3(_boxWidth, _boxHeight, _boxDepth);
    }

    public List<GameObject> GenerateBoxes(GameObject[] shelves)
    {
        if (shelves.Length == 0)
        {
            Debug.Log("Aborting: No shelves to place boxes on!");
            return null;
        }

        PlaceBoxesOnShelves(shelves);
        return _allBoxes;
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

        int boxesPrFloor = (shelfWidth > _reqSpacePrBox) ? AmountOfBoxesEachFloor(shelfWidth) : 0 ;

        for (int floor = 0; floor < shelfFloors; floor++)
        {
            float curPosY =  startPosY + (floor * spaceBetweenShelfFloors);
            PlaceBoxOnShelfFloor(shelf, curPosY, boxesPrFloor, shelfWidth);
        }
    }

    private void PlaceBoxOnShelfFloor(GameObject shelf, float curPosY, int boxesPrFloor, float shelfWidth)
    {
        Quaternion shelfRot = shelf.transform.rotation;

        for (int i = 1; i <= boxesPrFloor; i++)
        {
            Vector3 pos = GetBoxPos(shelf, curPosY, boxesPrFloor, shelfWidth, i);//new Vector3(x, curPosY, 0);

            GameObject newBox = (GameObject)Instantiate(BoxPrefab, pos, transform.rotation);

            newBox.transform.localRotation = shelfRot;

            _allBoxes.Add(newBox);
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

        while(spaceLeft > _reqSpacePrBox)
        {
            count++;
            spaceLeft -= _reqSpacePrBox;
        }

        return count;
    }

    public Vector3 GetBoxDimesions()
    {
        return _boxDimensions;
    }
}
