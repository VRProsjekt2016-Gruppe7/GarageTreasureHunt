using UnityEngine;

public class RoomGenerator : MonoBehaviour
{

    // Left -> Front -> Right
    public GameObject ShelfPrefab;

    private GameObject[] _shelves;

    void Awake()
    {
        _shelves = new GameObject[RoomValues.ShelvesPositions.Length];
    }
    /*
    void Start()
    {
        GenerateRoom();
    }
    */
    public void GenerateRoom()
    {
        GetComponent<BoxesManager>().Init();
        GetComponent<BoxContentsManager>().Init();

        SpawnShelves();
        GetComponent<BoxesManager>().GenerateBoxes(_shelves);
        /*
        GetComponent<BoxContentsManager>().FillBoxes(
            GetComponent<BoxesManager>().GenerateBoxes(_shelves),
            GetComponent<BoxesManager>().GetBoxDimesions());
            */
    }

    private void SpawnShelves()
    {
        for (int i = 0; i < RoomValues.ShelvesPositions.Length; i++)
        {
            _shelves[i] = (GameObject)Instantiate(ShelfPrefab, transform.position, transform.rotation);
            _shelves[i].GetComponent<ShelfManager>().PlaceShelf(RoomValues.ShelvesPositions[i], GetRoomSide(i));
        }
    }

    private RoomSide GetRoomSide(int i)
    {
        return (i == 0) ? RoomSide.Left : RoomSide.Right;
    }
}
