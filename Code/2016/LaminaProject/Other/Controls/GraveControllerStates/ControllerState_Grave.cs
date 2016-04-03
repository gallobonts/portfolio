using UnityEngine;
using System.Collections;

public class ControllerState_Grave : ControllerState_Base {

  protected HumanController_Grave myHumanController;

  protected LaminaBrain_Grave myBrain;

  protected float gravityScale;

  //grave intializtion

  public ControllerState_Grave(HumanController_Grave newHumanController)
  {
    myHumanController = newHumanController;
    myTransform = myHumanController.transform;
    myBoxCollider2D = myHumanController.myBoxCollider2D;
    myRigidbody2D = myHumanController.myRigidbody2D;
    gravityScale = myHumanController.myRigidbody2D.gravityScale;

    myBrain = myHumanController.myGraveLaminaBrain;
     }

  public void SwitchState(ControllerState_Grave newControllerState)
  {
    ExitState();//calls the exit function in current state
    myHumanController.myControllerState=newControllerState;//switches state
    newControllerState.EnterState ();//calls enter function in new state
  }
}
