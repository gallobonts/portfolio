using UnityEngine;
using System.Collections;

public class AIState_IslandWander : AIState_Base 
{
  public AIState_IslandWander(KnobberAI_Island myAIController):base(myAIController){}

  Vector2 target;

  //collision stuff
  public float collisionDetectionDistance = 5f;
  float colisionDetectionTimer;
  public float collisionDetectionFrequency= .5f;



  public float wanderOffset=15f;//distance of projected circle from the ai
  public float wanderCircleSize=5f;//size of projected circle  
  public float wanderClipSize=5f;//how close to you get to the target before you update wander
  
  float idletimer;
  public float minIdleTimer=5;
  public float maxIdleTimer=10;


  protected override void EnterState ()
  {
    if(aiController.debug)
    {Debug.Log("entering Wander state");}
 
    idletimer = Random.Range(minIdleTimer,maxIdleTimer);
    idletimer = collisionDetectionFrequency;

    FindNewDirection();//set a new direction to wander in
    UpdateWander();//update your wander target and such

  }

  public override void Update (float dt)
  {
    if (aiController.debug)
    {
      GOD.myGOD.DebugStar(target);
     // Debug.DrawLine(myTransform.position, target);
    
    }

    Vector2 direction = target - (Vector2)myTransform.position;

    
    if(direction.magnitude <= wanderClipSize)//if you are already close enough to target, then your done
    {
      UpdateWander();
    }
    direction.Normalize();
    aiController.myDirection = direction;


    aiController.addForce +=  direction*aiController.speedVariance*aiController.myStats.speed;

    colisionDetectionTimer -= dt;
    if (colisionDetectionTimer < 0)
  {
      CheckCrash();
  }
    idletimer -= dt;
    if(idletimer<0)
    {
      SwitchState(aiController.myAIState_IslandIdle);
    }
  }
  
  void FindNewDirection()
  {
    aiController.myDirection = Random.insideUnitCircle; 
  }
  
  protected void UpdateWander()
  {
 
    Vector2 newTarget;
    newTarget= myTransform.position;
    newTarget+= (aiController.myDirection*wanderOffset);
    newTarget+=Random.insideUnitCircle*wanderCircleSize;
   

    target=newTarget;
    CheckCrash();
  
  }

  void CheckCrash()
  {
    colisionDetectionTimer = collisionDetectionFrequency;

    if (aiController.debug)
    {
      Vector2 endline= (Vector2)myTransform.position + aiController.myDirection*collisionDetectionDistance;
      Debug.DrawLine(myTransform.position, endline, Color.blue,collisionDetectionFrequency);
    }
    
    if (Physics2D.Raycast(myTransform.position, aiController.myDirection, collisionDetectionDistance))//repeat if raycast hit an someone
    {
      if(aiController.debug)
      {Debug.Log("gonna crash!");}
      
      FindNewDirection();
      UpdateWander();
    }

  }
}



