using UnityEngine;

public enum Hand
{
    Left,
    Right
}

public abstract class HandController : MonoBehaviour, IControllerHandler {

    public Hand CurrentHand;
    public GameObject ConnectedObjectLeft;
	public GameObject ConnectedObjectRight;


	public void SetupController(Hand hand, GameObject gO, GameObject gO2)
    {
        AssignHand(hand);
		AssignObjectLeft(gO);
		AssignObjectRight (gO2);
    }

    public void AssignHand(Hand hand)
    {
        CurrentHand = hand;
    }

    public void AssignObjectLeft(GameObject gO)
    {
		ConnectedObjectLeft = gO;
    }

	public void AssignObjectRight(GameObject gO)
	{
		ConnectedObjectRight = gO;
	}

    public void RemoveObjectRight()
    {
		ConnectedObjectRight = null;
    }

	public void RemoveObjectLeft()
	{
		ConnectedObjectLeft = null;	
	}

}
