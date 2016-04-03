using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KnobberAI_Island : KnobberAI_Base 
{
 
  public List<GameObject> PossibleRewards;
  public float minEruptDistance=5;
  public float maxEruptDistance=10;

  
  
  
  
  override protected void InitializeAIStates()
  {
    myAIState_IslandIdle = new AIState_IslandIdle (this);
    myAIState_IslandWander = new AIState_IslandWander (this);
    myAIState_IslandAttack = new AIState_IslandAttack(this);
    
    myAIState = myAIState_IslandIdle;
  }

	

  void OnCollisionEnter2D(Collision2D other)
  {
    if(other.transform.tag=="Attack")
    {
      AttackInstance attckinst= other.gameObject.GetComponent<AttackInstance>();
      if(attckinst.owner==myGameObject)
      {
        return;
      }
      lastThingToHitMe=attckinst.owner;
      TakeDam(attckinst.damage);
      
      
    }
  }
  void TakeDam(float dam)
  {
    myStats.health -= dam;
    
    targetTransform= lastThingToHitMe.transform;
    myAIState.SwitchState(myAIState_IslandAttack);
    
  }
  
  
  override  protected void EruptReward()
  {
    
    //instantiate the reward
    int rand = Random.Range(0,PossibleRewards.Count);
    GameObject reward= (GameObject)GameObject.Instantiate (PossibleRewards [rand],myTransform.position,myTransform.rotation);
    
    //erupt the reward...not working correctly
    Vector2 randxDirection = new Vector2(-1, 1);
    Vector2 randDirection = new Vector2(Random.Range(randxDirection.x, randxDirection.y), 1);
    float randEruptionForce = Random.Range (minEruptDistance, maxEruptDistance);
    
    
    Vector2 addForce = randDirection * randEruptionForce;
    reward.GetComponent<Rigidbody2D> ().AddForce (addForce);
    
    Destroy(myGameObject);
  }
  


	

	/*

	void OnCollisionEnter2D(Collision2D col)
	{
		AttackInstance recievedAttack=col.gameObject.GetComponent("AttackInstance")as AttackInstance;
		if(recievedAttack==null)
		{return;}
		if(recievedAttack.myTeam!=myTeam)
		{
			myStats.health-= recievedAttack.damage;
			lastThingToHitMe=recievedAttack.owner;
			targetObject=lastThingToHitMe;
			
		}
		
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		AttackInstance recievedAttack=col.gameObject.GetComponent("AttackInstance")as AttackInstance;
		if(recievedAttack==null)
		{return;}
		if(recievedAttack.myTeam!=myTeam)
		{
			myStats.health-= recievedAttack.damage;
			lastThingToHitMe=recievedAttack.owner.transform.root.gameObject;
			targetObject=lastThingToHitMe;
	//		StartCoroutine(DelayedAIStateChange(reactionTime,AIState.ATTACK));
			
		}
	}
*/
	
}
