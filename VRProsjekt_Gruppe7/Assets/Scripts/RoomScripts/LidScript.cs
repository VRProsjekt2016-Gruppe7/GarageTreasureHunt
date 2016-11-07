using UnityEngine;
using System.Collections;
using System.Linq.Expressions;

public class LidScript : MonoBehaviour {

    private float maxOpenValue = 2f;
    private float curOpenValue = 0f;

    private bool _openLid = false;
    private bool _closingLid = false;

    // Open lid V2 test
    public bool OpeningLid = false;
    public GameObject Hand;
    public Vector3 HandPos = Vector3.zero;
    public Vector3 StartRotation;
    public Vector3 CurrentRotation;
    private float startX;

    // start 270 

    void Start()
    {
        StartRotation = transform.localEulerAngles;
        startX = transform.localEulerAngles.x;
        print(startX);
    }

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

    public void OpenLidV2(GameObject hand)
    {
//        HandPos = handPosition;
        Hand = hand;
        OpeningLid = true;
    }

    void Update () {

        Vector3 startAngle = new Vector3(270, 0, 180);

        float angle = Vector2.Angle(new Vector2(transform.position.z, transform.position.y),
            new Vector2(Hand.transform.position.z, Hand.transform.position.y));

        float newAngle = (270 - (Get0To90Range(angle) + 180));

        if (newAngle < 0 || newAngle > 90)
            return; 

        transform.eulerAngles = startAngle + new Vector3(newAngle, 0 , 0);

        // EulerAngles starter på Vec3(270, 0, 180);
        // Skal gå opp til Vec3(359, 0, 180)


    }

    private float Get0To90Range(float angle)
    {
        float oldRange = 60 - 15;
        float newRange = 90 - 0;
        float newValue = (((angle - 15)*newRange)/oldRange) + 0;
        return newValue;
    }
}
