using UnityEngine;
using System.Collections;

public class ControllerState_Grave_Air : ControllerState_Grave
{
  bool usedDoubleJump = true;

  public ControllerState_Grave_Air(HumanController_Grave myHumanController):base(myHumanController){}

  override protected void EnterState()
  {
   // Debug.Log("enter air state");
    usedDoubleJump = false;
    myRigidbody2D.gravityScale = gravityScale;

  }
 
  public override void Update(float dt)
  {
    bool grounded = myHumanController.IsGrounded();
    if(grounded)
    {
      myHumanController.myControllerState.SwitchState(myHumanController.myControllerState_GraveGround);
      return;
    }   
      
      //double jump
    if (myHumanController.myControls.jump.WasPressed && (myHumanController.myGraveLaminaBrain.hasTechnique [(int)Technique.DOUBLEJUMP] && !usedDoubleJump))
      {
        Jump();
        usedDoubleJump = true;
        
      }
      //if holding an object, check and see if the player wants to throw it, else check and see if the player wants to pick one up
      if (myBrain.holdingObject)
      {
      myHumanController.HandleThrow();
        
      }
      else
      {
      myHumanController.HandlePickup();
      }
      
    myHumanController.HandleInteractions();

    UpdateMovement();
    }//end update

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

}
