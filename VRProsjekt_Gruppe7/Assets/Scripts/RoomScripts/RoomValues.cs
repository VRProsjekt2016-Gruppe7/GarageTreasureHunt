using UnityEngine;
using System.Collections;

public class RoomValues : MonoBehaviour
{
    public static readonly float BoxDepthPos = 0.185f;
    public static readonly float LowestFloorPosY = 0.30011f;
    public static readonly float DistanceToNextFloor = 0.5f;
    public static readonly float[] BoxSidesPos = { -0.73f, -0.23f, 0.23f, 0.73f };
    public static readonly Vector3[] ShelvesPositions = { new Vector3(-0.9f, 0, 0.1f), new Vector3(0.9f, 0, 0.1f) };
}
