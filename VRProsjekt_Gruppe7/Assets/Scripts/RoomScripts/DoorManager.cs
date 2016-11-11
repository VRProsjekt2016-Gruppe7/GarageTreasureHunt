using UnityEngine;
using System.Collections;

public enum DoorStatus
{
    Open,
    Closed
}

public class DoorManager : MonoBehaviour
{

    public bool Open = false;
    public DoorStatus CurrentDoorStatus = DoorStatus.Closed;

    private float _moveSpeed = 1.4f;

    private readonly float _yMin = Mathf.Abs(0.0f);
    private readonly float _yMax = Mathf.Abs(2.15f);
    private readonly float _posX = Mathf.Abs(0.0f);
    private readonly float _posZ = -1.227f;

    void Update () {
	    if ((Open && transform.position.y < _yMax) 
            || (!Open && transform.position.y > _yMin))
	    {
	        MoveDoorAxisY();
	    }

        VerifyPos();
        VerifyStatus();
    }

    private void VerifyStatus()
    {
        if (transform.position.y == _yMax && CurrentDoorStatus != DoorStatus.Open)
        {
            CurrentDoorStatus = DoorStatus.Open;
        }
        else if (transform.position.y == _yMin && CurrentDoorStatus != DoorStatus.Closed)
        {
            CurrentDoorStatus = DoorStatus.Closed;
        }
    }

    private void VerifyPos()
    {
        if (transform.position.y > _yMax && CurrentDoorStatus != DoorStatus.Open)
        {
            transform.position = new Vector3(_posX, _yMax, _posZ);
        }
        else if (transform.position.y < _yMin && CurrentDoorStatus != DoorStatus.Closed)
        {
            transform.position = new Vector3(_posX, _yMin, _posZ);
        }
    }

    private void MoveDoorAxisY()
    {
        Vector3 dir = Open ? Vector3.up : -Vector3.up;

        transform.position += dir*_moveSpeed*Time.deltaTime;

    }
}
