using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using InControl;


	
[Serializable]
public class LaminaBrain_Island : Brain_Base 
{
	public bool inControl=false;

    
	//refrences
	public HumanController_Island myHumanControler;//reference to human controller
	public LaminaAI myAIController;//reference to ai controller
	public SkillTreeBase mySkillTree;//skill tree associated with this lamina

	//controller info
	public bool debug;

	

	public bool inTerritory;
	

	override protected void Start()
	{
		base.Start();
	
		myPriorities.ResetToMax();
	

	
	}


  void OnEnable()
  {
    GOD_Memory.SaveNormal += Save;
    GOD_Memory.LoadNormal += Load;
    //we reset it on level change
  }


  void Save()
  {
   if (myGameObject == null)
  {
    Debug.Log("I don't know why " + gameObject.name + " has a null reference to self...but it needs to be fixed");
    myGameObject = this.gameObject;
  }

    string savePath = GOD_Memory.rootFolder + "laminaBrain";

    GOD_Memory.instance.CheckPath(savePath);

    savePath+="?tag=" + myGameObject.name;


    //save base stats
    ES2.Save<float>(baseStats.maxHealth, savePath    + "maxhealth");
    ES2.Save<float>(baseStats.health, savePath +"health");
    ES2.Save<int>(baseStats.strength, savePath +"strength");
    ES2.Save<int>(baseStats.speed, savePath +"speed");

    //save level info
    ES2.Save<int>(myLevelInfo.level, savePath +"level");
    ES2.Save<int>(myLevelInfo.experience, savePath +"experience");
    ES2.Save<int>(myLevelInfo.experienceTilNxtLvl, savePath +"experienceTilNxtLvl");




  }

  void Load()
  {
    if(GOD.myGOD.isNewGame){return;}//if it's a new game, leave the defaults


    string loadPath = GOD_Memory.rootFolder + "laminaBrain"+"?tag=" + myGameObject.name;
    if (!ES2.Exists(loadPath)){return;}


    baseStats.maxHealth = ES2.Load<float>(loadPath + "maxhealth");
    //save base stats
    baseStats.maxHealth = ES2.Load<float>(loadPath + "maxhealth");
    baseStats.health = ES2.Load<float>(loadPath + "health");
    baseStats.strength = ES2.Load<int>(loadPath + "strength");
    baseStats.speed = ES2.Load<int>(loadPath +"speed");
    
    //save level info

    myLevelInfo.level =  ES2.Load<int>(loadPath  +"level");
    myLevelInfo.experience =  ES2.Load<int>(loadPath  +"experience");
    myLevelInfo.experienceTilNxtLvl =  ES2.Load<int>(loadPath +"experienceTilNxtLvl");
  

  }

	void Die()
	{
		Debug.Log ("I, "+this.gameObject.name+", have died");

	}

	override public void SetActive(InputDevice inputDevice,Controls newControls,int newPlayerNum)
	{
 			myControls=newControls;
			myControls.Device=inputDevice;
			inControl=true;
			myHumanControler.enabled=true;
      playernum = newPlayerNum;
			myHumanControler.SetController(myControls,newPlayerNum);
      myAIController.enabled = false;
		
	}

public void SetInactive()
{
  myControls = null;
  inControl=false;
  myHumanControler.enabled=false;
  myHumanControler.SetController(myControls,0);
  
  myAIController.enabled = true;

}
	public void GainExperience(int exp)
	{
  	myLevelInfo.experience+= (int) (exp* expMultiplier);

		if(myLevelInfo.experience>=myLevelInfo.experienceTilNxtLvl)
		{
			LevelUp();
		}
	}

	void Update()
	{
		if(GOD.myGOD.isGamePaused)
		{return;}

		if(currentStats.health<=0)
		{Die();}
		else{CheckPriorities();}

		if(myStatusEffects.starvation>0)
		{Starve();}


	}

	void FixedUpdate()
	{
		if(GOD.myGOD.isGamePaused)
		{return;}

		Vector2 newDirection=this.GetComponent<Rigidbody2D>().velocity.normalized;
		if(newDirection!= new Vector2(0,0))
		{direction=newDirection;}

	}

  public Controls GetController( )
  {
   return myHumanControler.GetController();
  }
 

	public GUI_info GetGUIInfo()
  {
    GUI_info myInfo=new GUI_info();
    myInfo.name = myName;
    myInfo.myLevelInfo = myLevelInfo;
    myInfo.myPriorities = myPriorities;
    myInfo.myStats = currentStats;
    myInfo.myStatusEffects = myStatusEffects;
    return myInfo;
  }
	void CheckPriorities()
	{
		DecayType myDecayType=DecayType.PASSIVE;
		if(inControl){myDecayType=DecayType.ACTIVE;}


		myPriorities.Depreciate(Time.deltaTime,myDecayType,inTerritory);

    //high
    if (Priorities.education > 100)
  {
    expMultiplier = 2.0f;
    myStatusEffects.dimWit = 0;
    myStatusEffects.genius = 1;
  }
  else if (myStatusEffects.genius==1)
  {
      myStatusEffects.genius=0;
      expMultiplier=1.0f;
  }

		//low education -> dullness
		if(Priorities.education>stageOnePriorityPenalty)
		{
			if(myStatusEffects.dimWit!=0)
			{
        expMultiplier+= myStatusEffects.dimWit*.25f;
				myStatusEffects.dimWit=0;
			}
		}
		else if(Priorities.education>stageTwoPriorityPenalty)
		{
			if(myStatusEffects.dimWit!=1)
			{
				expMultiplier+= myStatusEffects.dimWit*.25f;
				myStatusEffects.dimWit=1;
				expMultiplier-= myStatusEffects.dimWit*.25f;

			}
		}
    else if(Priorities.education>stageThreePriorityPenalty)
		{
			if(myStatusEffects.dimWit!=2)
			{
				expMultiplier+= myStatusEffects.dimWit*.25f;
				myStatusEffects.dimWit=2;
				expMultiplier-= myStatusEffects.dimWit*.25f;

			}
		}
		else if(myStatusEffects.dimWit!=3 )
		{	
			expMultiplier+= myStatusEffects.dimWit*.25f;
			myStatusEffects.dimWit=3;
			expMultiplier-= myStatusEffects.dimWit*.25f;
		}

		
		//low hunger -> starvation
		if(myPriorities.hunger>stageOnePriorityPenalty)
		{myStatusEffects.starvation=0;}
		else if(myPriorities.hunger>stageTwoPriorityPenalty)
		{myStatusEffects.starvation=1;}
		else if(myPriorities.hunger>stageThreePriorityPenalty)
		{myStatusEffects.starvation=2;}
		else
		{myStatusEffects.starvation=3;}


		//low sleep -> sleep deprevation
		if(myPriorities.sleep>stageOnePriorityPenalty)
		{
			if(myStatusEffects.sleepDerpivation!=0)
			{
				healingMultiplier+=  .25f + myStatusEffects.sleepDerpivation*.25f;
				myStatusEffects.sleepDerpivation=0;
			}
		}
		else if(myPriorities.sleep>stageTwoPriorityPenalty)
		{
			if(myStatusEffects.sleepDerpivation!=1)
			{
				if(myStatusEffects.sleepDerpivation==0)
				{healingMultiplier-=.25f;}

				healingMultiplier+=  myStatusEffects.sleepDerpivation*.25f;
				myStatusEffects.sleepDerpivation=1;
				healingMultiplier-=  myStatusEffects.sleepDerpivation*.25f;

			}
		}
		else if(myPriorities.sleep>stageThreePriorityPenalty)
		{
			if(myStatusEffects.sleepDerpivation!=2)
			{
				healingMultiplier+= myStatusEffects.sleepDerpivation*.25f;
				myStatusEffects.sleepDerpivation=2;
				healingMultiplier-= myStatusEffects.sleepDerpivation*.25f;

			}
		}
		else if(myStatusEffects.sleepDerpivation!=3 )
		{	
			healingMultiplier+=  myStatusEffects.sleepDerpivation*.25f;
			myStatusEffects.sleepDerpivation=3;
			healingMultiplier-= myStatusEffects.sleepDerpivation*.25f;
		}

    //if wanderlust->high
    if (myPriorities.wanderLust >= 95f)
  {
    myStatusEffects.homeSick = 0;
    myStatusEffects.stirCrazy = 1;
  }
  else
  {
      myStatusEffects.stirCrazy=0;
  }

		 	//low wanderLust -> homeSick
		if(myPriorities.wanderLust>stageOnePriorityPenalty)
		{
			if(myStatusEffects.homeSick!=0)
			{
				myStatusEffects.homeSick=0;
				lockAllAttacks(false);
			}
		}
		else if(myPriorities.wanderLust>stageTwoPriorityPenalty)
		{
			if(myStatusEffects.homeSick==0)//set up stage 1 lock
			{
				int randomLock=UnityEngine.Random.Range(0,3);
				attackLock[randomLock]=true;
				previousLocks[0]=randomLock;
				myStatusEffects.homeSick=1;
			}
			else if(myStatusEffects.homeSick==2)
			{
				attackLock[previousLocks[1]]=false;//unlock stage 2's lock
				myStatusEffects.homeSick=1;

			}
		}
		else if(myPriorities.wanderLust>stageThreePriorityPenalty)
		{
			if(myStatusEffects.homeSick==1)//set up stage 2 lock
			{
				int randomLock=UnityEngine.Random.Range(0,3);

				if(randomLock==previousLocks[0])//prevent re lock
				{randomLock++; randomLock%=4;}

				attackLock[randomLock]=true;
				previousLocks[1]=randomLock;
				myStatusEffects.homeSick=2;
			}
			else if(myStatusEffects.homeSick==3)
			{
				attackLock[previousLocks[2]]=false;//unlock stage 3's lock
				myStatusEffects.homeSick=2;
				
			}
		}
		else if(myStatusEffects.homeSick!=3 )
		{
			int randomLock=UnityEngine.Random.Range(0,3);

			while(randomLock==previousLocks[0] || randomLock==previousLocks[1])//prevent re lock
			{randomLock++; randomLock%=4;}

			attackLock[randomLock]=true;
			previousLocks[2]=randomLock;
			myStatusEffects.homeSick=3;
		}


		//preserve current health  between tranfusion, since deltaStats shouldn't have anything to do with it
		float currentHealth=currentStats.health;
		currentStats.SetEqual(baseStats+deltaStats);
		currentStats.health=currentHealth;
	}
	void Starve()
	{

		if(hungerPangLull)
		 {
			switch(myStatusEffects.starvation)
			{
			case 1:
				Invoke("HungerPang",stageOneStarveTime);
				break;
			case 2:
				Invoke("HungerPang",stageTwoStarveTime);
				break;
			case 3:
				Invoke("HungerPang",stageThreeStarveTime);
				break;
			}
			hungerPangLull=false;
		 }
		 
	}
	//
	void HungerPang()
	{
		switch(myStatusEffects.starvation)
		{
		case 1:
			currentStats.health= Mathf.Clamp( currentStats.health-(stageOneStarveEffect*currentStats.maxHealth),1f,currentStats.maxHealth);
			break;
		case 2:
			currentStats.health= Mathf.Clamp( currentStats.health-(stageTwoStarveEffect*currentStats.maxHealth),1f,currentStats.maxHealth);
			break;
		case 3:
			currentStats.health= Mathf.Clamp( currentStats.health-(stageThreeStarveEffect*currentStats.maxHealth),1f,currentStats.maxHealth);
			break;
		}
	
		hungerPangLull=true;
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag=="AreaOfInfluence")
			{
				inTerritory=true;
			}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.tag=="AreaOfInfluence")
		{
			inTerritory=false;
		}

	}


	public void Heal(float val)
	{
		//multiply it by multiplier & clamp it to 0 & max health
		currentStats.health= Mathf.Clamp(currentStats.health+(val*healingMultiplier),0,currentStats.maxHealth);
	}


	void OnCollisionEnter2D(Collision2D col)
	{
		AttackInstance recievedAttack=col.gameObject.GetComponent("AttackInstance")as AttackInstance;
		if(recievedAttack==null)
		{return;}
	
		if(recievedAttack.myTeam!=myTeam)
		{
			currentStats.health-= recievedAttack.damage;
		}

//		lastThingToHitMe=recievedAttack.owner;
	}

	public void LevelUp()
	{
  myLevelInfo.experience -= myLevelInfo.experienceTilNxtLvl;
  double newExperience = myLevelInfo.experienceTilNxtLvl * 1.5;
  myLevelInfo.experienceTilNxtLvl = (int)newExperience;
  myLevelInfo.level++;
    myLevelInfo.skillpoints++;
}
  /*
		int nodeLevel = (myLevelInfo.level-(skillpoints+1))%4;
		int setLevel= (myLevelInfo.level-1(skillpoints+1)) /4;
		//find starting node
		skillPair= lastLevelChoice*2;

		//Debug.Log ("level = " +level + " nodelevel "+ nodeLevel +" setLevel "+setLevel + " skillpair " +skillPair);

		switch (nodeLevel)
		{
		case 0:
			myNodes[0]=mySkillTree.mySkillSets[setLevel].Level1Skill;
			myNodes[1]=null;
			lastLevelChoice=0;
			skillPair=0;
			break;
		case 1:
			myNodes[0]=mySkillTree.mySkillSets[setLevel].Level2Skill[skillPair];
			myNodes[1]=mySkillTree.mySkillSets[setLevel].Level2Skill[skillPair+1];
			break;
			
		case 2:
			myNodes[0]=mySkillTree.mySkillSets[setLevel].Level3Skill[skillPair];
			myNodes[1]=mySkillTree.mySkillSets[setLevel].Level3Skill[skillPair+1];
			break;
			
		case 3:
			myNodes[0]=mySkillTree.mySkillSets[setLevel].Level4Skill[skillPair];
			myNodes[1]=mySkillTree.mySkillSets[setLevel].Level4Skill[skillPair+1];
			break;

		}


	}//end level up

*/

	public void ChooseLevelUp(int choice)
	{
    /*
		if(myNodes[choice]==null)
		{return;}
		//choice should = 0 || 1
		lastLevelChoice=skillPair+choice;

		SkillTreeNodes levelUpNode= myNodes[choice];

		switch(myNodes[choice].myNodeType)
		{
		case nodeType.Attack:
/*
			//find out if youalready have the attack
			int attackFound = -1;
			for(int i=0; i<collectedAttacks.Count;i++)
			{
				if(collectedAttacks[i].myAttack==levelUpNode.myAttackNode)
				{
//					Debug.Log("collected attack= "+collectedAttacks[i].myAttack +" myattacknode = "+myNodes[choice].myAttackNode+ " i = "+i);
					attackFound=i;
				}
			}

			//if you have it, then level it up
			if(attackFound>=0)//if attack found
			{
				collectedAttacks[attackFound].LevelUp();//level up both list & equipped ability
			}

			//else, learn the attack
			else
			{
				GameObject temp=null;
				AttackBase tempAttackBase;
				string path= levelUpNode.myAttackNode.ToString()+"_Attack";
				//load the pefab,instantiate the prefab, then set it's parent
				temp=Resources.Load(path)as GameObject;
				tempAttackBase= temp.GetComponent("AttackBase")as AttackBase;
				

				temp=GameObject.Instantiate(temp)as GameObject;
				temp.transform.parent= transform.FindChild("Attacks");
				collectedAttacks.Add(tempAttackBase);

			}
			break;
*/
	
	
	}//end choose level up

/*
	public void ChangeControl(int newPlayerNum,Controls newControls)
	{
	
		//change the control to the player
		if(newPlayerNum>0)
		{
			myHumanControler.enabled=true;
			myHumanControler.SetController(newPlayerNum,newControls);
			myAIController.enabled=false;
			myTeam=Team.PLAYER;
			inControl=true;
		}
		//change control to ai
		else
		{
			myHumanControler.enabled=false;
			myAIController.enabled=true;
			myTeam=Team.AI;
			inControl=false;

		}
		equippedAttacks[0].myTeam=myTeam;
		equippedAttacks[1].myTeam=myTeam;

	}
	*/
}
