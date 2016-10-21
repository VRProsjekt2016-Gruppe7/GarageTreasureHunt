using UnityEngine;
using System.Collections;
using System;

public enum RoomSide
{
    Left,
    Front,
    Right
}

public class ShelfManager : MonoBehaviour {

    public GameObject[] Legs;

    private readonly int _nrOfShelfFloors = 3;
    private readonly float _spaceBetweenShelves = 0.5f;
    private readonly float _lowestFloorPosY = 0.11f;

    private readonly float _legsDiameter = 0.03f;
    private readonly float _legsHeight = 0.85f;

    private readonly float _legsOffsetX = 0.45f;
    private readonly float _legsOffsetY = 0.85f;
    private readonly float _legsOffsetZ = 0.115f;

    public void PlaceObject(Vector3 pos, float width, RoomSide side)
    {
        transform.position = pos;
        transform.localScale = new Vector3(width, transform.localScale.y, transform.localScale.z);

        AdjustSizes(width);
        RotateShelf(side);

    }

    private void RotateShelf(RoomSide side)
    {
        if(side == RoomSide.Front)
            return;

        float rotY = (side == RoomSide.Left) ? -90f : 90f;
        transform.Rotate(0f, rotY, 0f);
    }

    private void AdjustSizes(float shelfWidth)
    {
        foreach(GameObject leg in Legs)
        {
            float newDia = _legsDiameter / shelfWidth;

            leg.transform.localScale = new Vector3(newDia, transform.localScale.y, newDia);
        }
    }

    public int GetNrOfShelfFloors()
    {
        return _nrOfShelfFloors;
    }

    public float GetSpaceBetweenShelfFloors()
    {
        return _spaceBetweenShelves;
    }

    public float GetLowestShelfFloorY()
    {
        return _lowestFloorPosY;
    }
}
