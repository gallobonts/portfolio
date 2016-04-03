using UnityEngine;
using System.Collections;

public class AIState_Base 
{

  protected KnobberAI_Base aiController;

	protected Transform myTransform;
	protected BoxCollider2D myBoxCollider;
	protected Rigidbody2D myRigidBody2D;

	virtual public void Update(float dt){}

	virtual protected void EnterState(){}
	virtual protected void ExitState(){}
 

  //grave intializtion
  public AIState_Base(KnobberAI_Grave myAIController)
  {
    aiController = myAIController;
    myTransform = myAIController.transform;
    myBoxCollider = myAIController.myBoxCollider;
    myRigidBody2D = myAIController.myRigidBody2D; 
  }

  //island intializtion
  public AIState_Base(KnobberAI_Island myAIController)
  {
    aiController = myAIController;
    myTransform = myAIController.transform;
    myBoxCollider = myAIController.myBoxCollider;
    myRigidBody2D = myAIController.myRigidBody2D; 
  }
  public void SwitchState(AIState_Base newAIstate)
  {
    ExitState();//calls the exit function in current state
    aiController.myAIState=newAIstate;//switches state
    newAIstate.EnterState ();//calls enter function in new state
  }
}




  

 

