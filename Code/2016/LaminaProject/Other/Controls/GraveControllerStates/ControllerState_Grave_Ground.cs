using UnityEngine;
using System.Collections;

public class ControllerState_Grave_Ground : ControllerState_Grave 
{
  public float rewindTimerMax = 4.0f;
  float rewindTimer = 0.0f;
 



  public ControllerState_Grave_Ground(HumanController_Grave myHumanController):base(myHumanController){}

  override protected void EnterState()
  {
 // Debug.Log("enter ground state");
  }

  public override void Update(float dt)
  {

    bool grounded = myHumanController.IsGrounded();
    if(!grounded)
      {
        myHumanController.myControllerState.SwitchState(myHumanController.myControllerState_GraveAir);
      return;
      }
 
    rewindTimer -= dt;
    
    if (rewindTimer <= 0)
    {
        myHumanController.oldPosition = myTransform.position;
      rewindTimer = rewindTimerMax;
    }
    
    if (myHumanController.myControls.jump.WasPressed)//if you jump, enter air state
    {
        Jump(); 
        myHumanController.myControllerState.SwitchState(myHumanController.myControllerState_GraveAir);
        return;
    }

      myHumanController.HandleAttacks();
      myHumanController.HandleInteractions();
    
    //if holding an object, check and see if the player wants to throw it, else check and see if the player wants to pick one up
      if (myBrain.holdingObject)
    {
        myHumanController.HandleThrow();
      
    }
    else
    {
        myHumanController.HandlePickup();
    }
    
    if (myHumanController.inDigArea && myHumanController.myGraveLaminaBrain.hasTechnique [(int)Technique.DIG])
    {
      if (myHumanController.myControls.technique.WasPressed)
      {
          myHumanController.myDigTechnique.Use();
        myHumanController.inDigArea = false;
      }
    }//in dig area
    
    UpdateMovement();

  }// update

 

  void UpdateMovement()
  {
   //horizontal stuff
    myHumanController.addForce += new Vector2(1, 0) * myHumanController.myControls.move.X * myBrain.currentStats.speed * myHumanController.speedVariance;

    myBrain.myPlayerLaminaManager.FollowChild();
    
    

  }

  void Jump( )
  {
    myHumanController.addForce += new Vector2(0, 100) * myHumanController.jumpHeight;
  
    myHumanController.ApplyMovement();
  }//end jump
}//class
