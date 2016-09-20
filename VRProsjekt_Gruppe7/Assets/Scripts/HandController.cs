using UnityEngine;
using System.Collections;

public enum Hand
{
    Left,
    Right
}

public abstract class HandController : MonoBehaviour, IControllerHandler {

    public Hand CurrentHand;
    public GameObject ConnectedObject;


    public void SetupController(Hand hand, GameObject gO)
    {
        AssignHand(hand);
        AssignObject(gO);
    }
    public void AssignHand(Hand hand)
    {
        CurrentHand = hand;
    }

    public void AssignObject(GameObject gO)
    {
        ConnectedObject = gO;
    }

    public void RemoveObject()
    {
        ConnectedObject = null;
    }

}
