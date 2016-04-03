using UnityEngine;
using System.Collections;

//base ai for 
public class KnobberAI_Base : AI_Base 
{
  //all ai state possibilities

  //grave states
  [HideInInspector]
  public AIState_GravePatrol myAIState_GravePatrol;
  [HideInInspector]
  public AIState_GraveIdle myAIState_GraveIdle;
  [HideInInspector]
  public AIState_GraveAttack myAIState_GraveAttack;


  //island states
  //all ai state possibilities
  [HideInInspector]
  public AIState_IslandIdle myAIState_IslandIdle;
  [HideInInspector]
  public AIState_IslandWander myAIState_IslandWander;
  [HideInInspector]
  public AIState_IslandAttack myAIState_IslandAttack;


	public Stats myStats;
	public AttackBase[] equippedAttacks= new AttackBase[2];

  public float reactionTime=.4f;//frequency at checking if you are still hitting the player

	public int exp=2;

  public virtual void Flip(){}

  new public void Start()
  {
    base.Start ();
    
    //InitializeAI states
    InitializeAIStates ();
    
    //add temp attacks
    SetUpTempAttacks ();
    
    
  }

  virtual protected void InitializeAIStates(){}
  void SetUpTempAttacks()
  {
    GameObject temp=null;
    
    if(equippedAttacks[0]==null)
    {
      //load the pefab,instantiate the prefab, then set it's parent
      temp=Resources.Load(attackNode.FireBall.ToString()+"_Attack")as GameObject;
      temp=GameObject.Instantiate(temp)as GameObject;
      temp.transform.parent= transform.FindChild("Attacks");
      AttackBase tempAttack =temp.GetComponent("AttackBase") as AttackBase;
      tempAttack.delayInSeconds*=1.25f;
      
      equippedAttacks[0]=temp.GetComponent("AttackBase")as AttackBase;
      equippedAttacks[0].SetUp(myGameObject,myTeam);
    }
    
    if(equippedAttacks[1]==null)
    {
      //load the pefab,instantiate the prefab, then set it's parent
      temp=Resources.Load(attackNode.Scratch.ToString()+"_Attack")as GameObject;
      temp=GameObject.Instantiate(temp)as GameObject;
      temp.transform.parent= transform.FindChild("Attacks");
      AttackBase tempAttack =temp.GetComponent("AttackBase") as AttackBase;
      tempAttack.delayInSeconds*=1.25f;
      
      equippedAttacks[1]=temp.GetComponent("AttackBase")as AttackBase;
      equippedAttacks[1].SetUp(this.gameObject,myTeam);
      
    }
  }

	void Update()
	{
		
		if(GOD.myGOD.isGamePaused)
		{return;}
		
		if(myStats.health<=0)
		{Die();}
	}

	void Die()
	{
    if (debug)
  {
    Debug.Log("last thing to hit me = " + lastThingToHitMe.name);
  }
    LaminaBrain_Island killerBrain=lastThingToHitMe.GetComponent("LaminaBrain_Island")as LaminaBrain_Island;
		if(killerBrain!=null)
		{
			killerBrain.GainExperience(exp);
		}
		
    
    if(debug)
    {Debug.Log ("I, "+myGameObject.name+", have been killed by "+killerBrain.myGameObject.name);}

    EruptReward();
	}
	
  protected virtual void EruptReward()
  {

  }














	public override void ApplyMovement ()
	{
		myRigidBody2D.velocity=addForce*myStats.speed;
	}



}
