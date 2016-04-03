using UnityEngine;
using System.Collections;

public class AIState_IslandIdle : AIState_Base 
{
  
  public AIState_IslandIdle(KnobberAI_Island myAIController):base(myAIController){}
  
  float timer;
  public float minTimer=2;
  public float maxTimer=5;


  
  protected override void EnterState ()
  {
    if(aiController.debug)
    {Debug.Log("entering idle state");}
    
    timer = Random.Range(minTimer,maxTimer);
  }
  
  public override void Update (float dt)
  {
    timer -= dt;
    if(timer<0)
    {
      SwitchState(aiController.myAIState_IslandWander);
      
    }
    
  }

}

