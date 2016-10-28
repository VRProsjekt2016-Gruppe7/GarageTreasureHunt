using UnityEngine;
using System.Collections;

public class LidScript : MonoBehaviour {

    private float maxOpenValue = 2f;
    private float curOpenValue = 0f;

    private bool _openLid = false;
    private bool _closingLid = false;

    public void OpenLid()
    {
        if (_closingLid)
            return;

        _openLid = true;
    }

    public void CloseLid()
    {
        _closingLid = true;
    }

    void Update () {

        if (_openLid && (curOpenValue < maxOpenValue))
        {
            curOpenValue += 1f * Time.deltaTime;
            transform.Rotate(Vector3.right, -22.5f * Time.deltaTime);
        }
        else if (_openLid && (curOpenValue >= maxOpenValue))
        {
            _openLid = false;
        }
        else if (!_openLid && _closingLid && (curOpenValue > 0))
        {
            _closingLid = true;
            curOpenValue -= 1f * Time.deltaTime;
            transform.Rotate(Vector3.right, +22.5f * Time.deltaTime);
        }
        else if (!_openLid && _closingLid && (curOpenValue <= 0))
        {
            _closingLid = false;
        }
    }
}
