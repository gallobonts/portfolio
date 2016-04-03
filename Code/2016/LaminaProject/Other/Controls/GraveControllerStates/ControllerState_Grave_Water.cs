using UnityEngine;
using System.Collections;

public class ControllerState_Grave_Water : ControllerState_Grave
{
bool topOfWater = false;

public ControllerState_Grave_Water(HumanController_Grave myHumanController):base(myHumanController)
{
}

protected override void EnterState( )
{
  if (myBrain.hasTechnique [(int)Technique.SWIM])
  {
     
    myHumanController.addForce.y = myRigidbody2D.velocity.y;
    myHumanController.addForce.y -= myHumanController.waterJumpHeight / 1.5f;//plunge yourself in

    myHumanController.ApplyMovement();
    topOfWater = false;
  }
  else
  {
    myHumanController.TakeDam(9999);//kill meh!
  }
}//enter state

public override void Update( float dt )
{
  if (topOfWater)
  {
    myRigidbody2D.gravityScale = 0.0f;//stay where you are
      
    if (myHumanController.myControls.jump.WasPressed)//if you jump, enter air state
    {
      Jump(); 
    }
    if (IsWaterAbove(myHumanController.groundRadius))//bottom or middle of water
    {
      topOfWater = false;
    }
      
  }
  else
  {
    myRigidbody2D.gravityScale = -1.0f;//float
      
      
    if (!IsWaterAbove(myHumanController.groundRadius / 3f))//top of water
    {
      topOfWater = true;//switch to top of water
      myHumanController.Invoke("PauseVerticalVelocity", .5f);
    }//stop floating
  }
  UpdateMovement();
}

void UpdateMovement( )
{
  //horizontal stuff
  myHumanController.addForce += new Vector2(1, 0) * myHumanController.myControls.move.X * myBrain.currentStats.speed * myHumanController.speedVariance;
   
  if (myBrain.hasTechnique [(int)Technique.DIVE])
  {
    myHumanController.addForce += new Vector2(0, 1) * myHumanController.myControls.move.Y * myBrain.currentStats.speed * (myHumanController.speedVariance * 2);
    if (topOfWater && myHumanController.addForce.y > 0)
    {
      myHumanController.addForce.y = 0;
    }
  }//if have dive

    
  myBrain.myPlayerLaminaManager.FollowChild();
    
    
    
}

void Jump( )
{
  myHumanController.addForce.y += myHumanController.waterJumpHeight;
    myHumanController.ApplyMovement();
}
 
public bool IsWaterAbove( float checkDistance )
{
  Collider2D[] checks = new Collider2D[1];
  int count = Physics2D.OverlapCircleNonAlloc(myHumanController.waterCheck.position, checkDistance, checks, myHumanController.waterLayerMask);

  if (count > 0)
  {
    return true;
  }
  return false;
   
}


}
