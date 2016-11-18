using UnityEngine;

public class OpenBox : MonoBehaviour
{
    [SerializeField]
    Transform _hand;

    public BoxLidStatus LidStatus;
    private BoxInfo _boxInfo;

    private float _closeLidSpeed = 5f;

    void Awake()
    {
        _boxInfo = GetComponentInParent<BoxInfo>();
        LidStatus = BoxLidStatus.Closed;
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
            LidStatus = BoxLidStatus.Open;
            _boxInfo.LidStatus(true);
        }
    }

    public void Open(Transform hand)
    {
        _hand = hand;
    }

    public void Close()
    {
        _hand = null;
    }

    private void HandleClosingLid()
    {
        if (transform.localEulerAngles.x >= 270 && transform.localEulerAngles.x < 360)
        {
            transform.localEulerAngles += new Vector3(_closeLidSpeed * (10), 0, 0) * Time.deltaTime;
        }
        else if (transform.localEulerAngles.x > 0 && transform.localEulerAngles.x < 270)
        {
            transform.localEulerAngles = Vector3.zero;
            LidStatus = BoxLidStatus.Closed;
            _boxInfo.LidStatus(false);
        }
    }

    private void HandleOpenLid()
    {
        float boxRot = transform.parent.localEulerAngles.z;
        float offset = 0.06f;
        Vector3 offsetPos = new Vector3(0, 0, 0);

        if ((boxRot >= 315 && boxRot <= 360) || (boxRot >= 0 && boxRot < 45))
        {
            // Box facing up
            offsetPos.y += offset;
        }
        else if (boxRot >= 45 && boxRot < 135)
        {
            // Box rotated right
            offsetPos.x += offset;
        }
        else if (boxRot >= 135 && boxRot < 225)
        {
            // Box rotated up-side-down
            offsetPos.y -= offset;
        }
        else if (boxRot >= 225 && boxRot < 315)
        {
            // Box rotated left
            offsetPos.x -= offset;
        }

        transform.LookAt(_hand.position + offsetPos);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, 0);
    }
}
