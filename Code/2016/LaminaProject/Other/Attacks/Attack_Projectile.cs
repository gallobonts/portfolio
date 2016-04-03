using UnityEngine;
using System.Collections;

public class Attack_Projectile : AttackBase 
{
	public GameObject attackInstance;
	//public GameObject[] myAttackInstances;
	public AttackInstance[] myAttackInstances;
	public float spawnDistance;
	int spawnCount=0;
	int useInstanceNum=0;

	void Start()
	{
		spawnCount= Mathf.CeilToInt(deathCountDown/delayInSeconds);
		//myAttackInstances= new GameObject[spawnCount];
		myAttackInstances= new AttackInstance[spawnCount];
		for(int i=0;i<spawnCount;i++)
		{
			GameObject newInstance= (GameObject)Instantiate(attackInstance);
		
			myAttackInstances[i]=newInstance.GetComponent("AttackInstance") as AttackInstance;
			myAttackInstances[i].myTeam=myTeam;
			myAttackInstances[i].damage=damage;
			myAttackInstances[i].owner=source;
			myAttackInstances[i].myElementalType=myElementalType;
			myAttackInstances[i].deathCountDown=deathCountDown;
			myAttackInstances[i].transform.parent= this.transform;

		}

	}
	override public void UseAttack(Vector2 direction)
	{
		if(!canAttack){return;}

		//find spawn location
		Vector2 spawnLocation= source.transform.position;

  
		spawnLocation+= direction*spawnDistance;

		//use the instance
		myAttackInstances [useInstanceNum].Use (direction, spawnLocation);
		//figure out next object tobe used
		useInstanceNum++;
		useInstanceNum%=spawnCount;

		//handle attack delay
		canAttack=false;
		Invoke("CanAttackCountDown",delayInSeconds);


	}

	override public void LevelUp()
	{

	}

	void CanAttackCountDown()
	{
		canAttack=true;
	}

	

}
