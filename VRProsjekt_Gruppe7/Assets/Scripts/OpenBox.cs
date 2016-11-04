using UnityEngine;

public class OpenBox : MonoBehaviour
{
    [SerializeField]
    Transform _hand;

    Vector3 _startAngles;

    [SerializeField]
    float _targetAngle = 0f;

    private float _closeLidSpeed = 30f;
    private float _closingSmoothing = 7.5f;
    Vector3 _lidClosedDirection;

    void Start()
    {
        this._startAngles = this.transform.localEulerAngles;
        _lidClosedDirection = -this.transform.up;
    }

    public void Open(Transform hand)
    {
        _hand = hand;
    }

    public void Close()
    {
        _hand = null;
    }

    void Update()
    {
        if (_hand == null)
        {
            HandleClosingLid();
        }
        else
        {
            HandleOpenLid();
        }
        
    }

    private void HandleClosingLid()
    {
        float diff = transform.localEulerAngles.x - this._startAngles.x;

        if (transform.localEulerAngles.x > this._startAngles.x)
        {
            transform.localEulerAngles -= new Vector3(_closeLidSpeed * (diff / _closingSmoothing), 0, 0) * Time.deltaTime;
        }
    }

    private void HandleOpenLid()
    {
        Vector3 diff = _hand.position - this.transform.position;

        diff = this.transform.parent.worldToLocalMatrix * diff;

        diff.x = 0f;
        float angle = -Vector3.Angle(this.transform.parent.worldToLocalMatrix * _lidClosedDirection, diff.normalized);

        //        Debug.Log(diff);

        if (diff.y < 0f)
        {
            if (diff.z <= 0f)
            {
                angle = 0f;
            }
            else
            {
                angle *= -1f;
                angle = Mathf.Max(90f, angle);
            }
        }

        //        Debug.LogFormat("Angle: {0}", angle);
        angle = SmoothLid(angle);

        this.transform.localEulerAngles = this._startAngles + new Vector3(angle, 0f, 0f);
    }

    private float SmoothLid(float angle)
    {
        float tempAngle = angle;

        if (tempAngle < -8f)
            tempAngle += 8f;
        else if (tempAngle >= -8)
            tempAngle = 0f;
        return tempAngle;

    }
}
