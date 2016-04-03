using UnityEngine;
using System.Collections;

public class AIState_IslandAttack : AIState_Base 
 {
  bool engaging = true;
  Range myAttackRange;
  AttackBase[] equippedAttacks = new AttackBase[2];
  int useAttack = 0;
  bool switchAttackLock = false;
  float reactionTime;
  public float closeRangeAttackDistance = 2.5f;
  public float midRangeAttackDistance = 6f;
  public float longRangeAttackDistance = 10f;
  float currentRangeDistance;
  float minSwitchAttackTimer = 2f;
  float maxSiwtchAttackTimer = 10f;
  Transform target;
 

  public AIState_IslandAttack(KnobberAI_Island myAIController):base(myAIController)
  {
    equippedAttacks = aiController.equippedAttacks;
    reactionTime = aiController.reactionTime;
    
    
  }

  public enum AttackState
{
    Hunting,//getting in range to attack
    Fleeing,//flees
    Attacking//using attacks
}
  protected override void EnterState ()
  {
  if (aiController.debug)
  {
    Debug.Log("entering attack state");
  }
  target = aiController.targetTransform;
  SwitchAttack();
}

  public override void Update(float dt)
  {
    //attack is split into 2 parts.
    //part 1-> engage enemy. get within bounds of the enemy to attack
    //part 2-> battle. the actual fighting
    
    if (engaging)
    {
      EngageTarget();
    }
    else
    {
      Battle();
      
    }
    
  }
  
  public void StartEngageTarget()
  {
    myAttackRange = equippedAttacks [useAttack].myRange;
    
    if (myAttackRange == Range.CLOSE)
    {
      currentRangeDistance = closeRangeAttackDistance;
    }
    else if (myAttackRange == Range.MID)
    {
      currentRangeDistance = midRangeAttackDistance;
    }
    else
    {
      currentRangeDistance = longRangeAttackDistance;
    }
    //  Debug.Log("start engage target " + myAttackRange.ToString() + " at " + currentRangeDistance);
    engaging = true;
    
  }
  
  public void UseAttack()
  {
    equippedAttacks [useAttack].UseAttack(aiController.myDirection);
  }
  
  protected void SwitchAttack( )
  {
    // Debug.Log("switching attack");
    switchAttackLock = false;
    useAttack++;
    useAttack %= 2;
    StartEngageTarget();
  }
  
  protected void DelayedRepeatEngage()
  {
    engaging = true;
  }
  
  bool IsInRange( )
  {
    Vector2 direction = target.position - myTransform.position;//find direction of target

    aiController.myDirection = direction.normalized;



    if (direction.magnitude == 0)
    {
      Debug.Log("error at " );
      GOD.myGOD.TraceStack();
      Debug.Log(" target= " + aiController.targetTransform.name+ " target position= " +  aiController.targetTransform.position +
                " my position= "+ myTransform.position);
    }
    
    if (direction.magnitude <= currentRangeDistance)//if you are already close enough to target, then your done
    {
      return true;
    }
    else
    {
      return false;
    }
  }
  
  protected void EngageTarget()
  {
    
    if (IsInRange())
    {
      
      engaging = false;
    }
    else
    {
      Vector2 direction= target.position-myTransform.position;
      aiController.addForce += direction.normalized * aiController.myStats.speed * aiController.speedVariance;
    }
    
    
  }
  
  protected void Battle()
  {
    if (IsInRange())//if in range
    {
      UseAttack();//attack
      
      float attackSwitchTimer = Random.Range(minSwitchAttackTimer, maxSiwtchAttackTimer); //possibly switch attacks up
      if (!switchAttackLock)
      {
        GOD.myGOD.StartTimer(SwitchAttack, attackSwitchTimer);
        switchAttackLock = true;
      }
      
    }
    else
    {
      StartEngageTarget();
    }
    
    
  }//end battle
  }//end aistate_island attack
 