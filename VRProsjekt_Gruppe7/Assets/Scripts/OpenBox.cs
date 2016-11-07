using UnityEngine;

public class OpenBox : MonoBehaviour
{
    [SerializeField]
    Transform _hand;

    private float _closeLidSpeed = 5f;

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
        if (transform.localEulerAngles.x >= 270 && transform.localEulerAngles.x < 360)
        {
            transform.localEulerAngles += new Vector3(_closeLidSpeed * (10), 0, 0) * Time.deltaTime;
        } 
        else if (transform.localEulerAngles.x > 0 && transform.localEulerAngles.x < 270)
        {
            transform.localEulerAngles = Vector3.zero;
        }
    }

    private void HandleOpenLid()
    {
        if (_hand.position.y - 0.05f < transform.position.y)
            return;

        if ((transform.localEulerAngles.x > 270 && transform.localEulerAngles.x < 360) || transform.localEulerAngles.x == 0)

        {
            transform.LookAt(_hand.position - new Vector3(0, 0.05f, 0));
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, 0);
        }
    }
}
