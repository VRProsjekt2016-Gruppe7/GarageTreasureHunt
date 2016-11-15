using UnityEngine;

public class RoomGenerator : MonoBehaviour
{

    // Left -> Front -> Right
    public GameObject ShelfPrefab;

    private GameObject[] _shelves;

    public void ResetAndSpawnShelves()
    {
        Reset();
        SpawnShelves();
    }

    public void Reset()
    {
        _shelves = new GameObject[RoomValues.ShelvesPositions.Length];
        GetComponent<BoxesManager>().ResetBoxes();
        GetComponent<BoxContentsManager>().Init();
    }

    public void SpawnBoxesAndContents()
    {
        GetComponent<BoxContentsManager>().FillBoxes(GetComponent<BoxesManager>().GenerateBoxes(_shelves));
    }

    public void EndGame()
    {
        
    }

    public void SpawnShelves()
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

    public void CleanRoom()
    {
        GetComponent<BoxContentsManager>().ClearContents();
        GetComponent<BoxesManager>().ResetBoxes();
    }
}
