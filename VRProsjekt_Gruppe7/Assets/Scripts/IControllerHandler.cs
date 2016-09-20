using UnityEngine;
using System.Collections;


public interface IControllerHandler {

    void AssignHand(Hand hand);
    void AssignObject(GameObject gO);
    void RemoveObject();
    void SetupController(Hand hand, GameObject gO);
}
