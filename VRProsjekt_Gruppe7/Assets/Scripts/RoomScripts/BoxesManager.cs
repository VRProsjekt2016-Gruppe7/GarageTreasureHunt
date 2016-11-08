using UnityEngine;
using System.Collections.Generic;

public class BoxesManager : MonoBehaviour
{

    public GameObject BoxPrefab;
    private List<GameObject> _allBoxes;

    public void Init()
    {
        _allBoxes = new List<GameObject>();
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
        for (int i = 0; i < shelves.Length; i++)
        {
            if (shelves[i] == null)
                continue;

            PlaceBoxesOnShelf(shelves[i]);
        }
    }

    private void PlaceBoxesOnShelf(GameObject shelf)
    {
        int shelfFloors = shelf.GetComponent<ShelfManager>().GetNrOfShelfFloors();

        for (int floor = 0; floor < shelfFloors; floor++)
        {
            float curPosY = RoomValues.LowestFloorPosY + (floor * RoomValues.DistanceToNextFloor);
            PlaceBoxOnShelfFloor(shelf, curPosY, RoomValues.BoxSidesPos.Length);
        }
    }

    private void PlaceBoxOnShelfFloor(GameObject shelf, float curPosY, int boxesPrFloor)
    {
        Quaternion shelfRot = shelf.transform.rotation;

        for (int i = 0; i < boxesPrFloor; i++)
        {
            Vector3 pos = GetBoxPos(shelf, curPosY, i);

            GameObject newBox = (GameObject)Instantiate(BoxPrefab, pos, transform.rotation);

            newBox.transform.localEulerAngles = new Vector3(0, ((shelf.transform.rotation.x >= 0.5f) ? 90 : 270), 0);

            _allBoxes.Add(newBox);
        }
    }

    private Vector3 GetBoxPos(GameObject shelf, float curPosY, int i)
    {
        Vector3 shelfPos = shelf.transform.position;

        float x = shelfPos.x;
        float y = curPosY;
        float z = RoomValues.BoxSidesPos[i] + shelfPos.z;

        return new Vector3(x, y, z);
    }
}
