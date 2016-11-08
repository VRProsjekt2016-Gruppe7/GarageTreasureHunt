using UnityEngine;
using System.Collections;


public interface IControllerHandler {

    void AssignHand (Hand hand);
    void AssignObjectLeft (GameObject gO);
	void AssignObjectRight (GameObject gO);
    void RemoveObjectLeft();
	void RemoveObjectRight();
	void SetupController(Hand hand, GameObject gO, GameObject gO2);
}
