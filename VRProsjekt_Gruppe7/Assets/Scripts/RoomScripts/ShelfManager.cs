using UnityEngine;
using System.Collections;
using System;

public enum RoomSide
{
    Left,
    Right,
    Front
}

public class ShelfManager : MonoBehaviour
{

    public GameObject[] Legs;

    private readonly int _nrOfShelfFloors = 4;

    public void PlaceShelf(Vector3 pos, RoomSide side)
    {
        transform.position = pos;
        RotateShelf(side);
    }

    private void RotateShelf(RoomSide side)
    {
        if (side == RoomSide.Front)
            return;

        float rotY = (side == RoomSide.Left) ? -90f : 90f;
        transform.Rotate(270f, rotY + 180f, 0f);
    }

    public int GetNrOfShelfFloors()
    {
        return _nrOfShelfFloors;
    }
}
