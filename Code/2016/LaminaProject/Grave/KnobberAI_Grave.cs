using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class KnobberAI_Grave : KnobberAI_Base 
{

  public List<GameObject> PossibleRewards;
  public float minEruptDistance=5;
  public float maxEruptDistance=10;

	

  override protected void InitializeAIStates()
	{
		myAIState_GravePatrol = new AIState_GravePatrol (this);
		myAIState_GraveIdle = new AIState_GraveIdle (this);
    myAIState_GraveAttack = new AIState_GraveAttack(this);

		myAIState = myAIState_GraveIdle;
	}
	
	public override void ApplyMovement ()
	{
		addForce.y += myRigidBody2D.velocity.y;//maintain y velocity
		addForce.x *= myStats.speed*speedVariance;
		myRigidBody2D.velocity = addForce;
	}
	
	public override void Flip()
	{
		myDirection.x *= -1;
		
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
    myAIState.SwitchState(myAIState_GraveAttack);
 
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

 


}
