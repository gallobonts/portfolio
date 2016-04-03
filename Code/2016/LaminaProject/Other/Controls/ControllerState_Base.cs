using UnityEngine;
using System.Collections;

public class ControllerState_Base 
  //this & the knober ai state system should be saved for future projects
  {

    protected Transform myTransform;
  protected BoxCollider2D myBoxCollider2D;
  protected Rigidbody2D myRigidbody2D;
    
    virtual public void Update(float dt){}
    virtual public void FixedUpate(float dt){}
    
    virtual protected void EnterState(){}
    virtual protected void ExitState(){}
    
    /*

    
  //island intializtion
  public ControllerState_Base(HumanController myHumanController)
  {
    myHumanController = myHumanController;
    myTransform = myHumanController.transform;
    myBoxCollider2D = myHumanController.myBoxCollider2D;
    myRigidbody2D = myHumanController.myRigidbody2D;
  }
*/
    
  }

