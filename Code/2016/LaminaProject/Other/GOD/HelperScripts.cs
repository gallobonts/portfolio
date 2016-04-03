using UnityEngine;
using System.Collections;


[System.Serializable]
public class Stats
{
	public float maxHealth;
	public float health;
	public int strength;
	public int speed;

	
	public static Stats operator +(Stats left, Stats right)
	{
		Stats addition= new Stats();
		addition.maxHealth=left.maxHealth+right.maxHealth;
		addition.health=Mathf.Clamp(left.health+right.health,0,left.maxHealth);  
		addition.strength=left.strength+right.strength;
		addition.speed=left.speed+right.speed;
	   
		return addition;
	}

	public static Stats operator -(Stats left, Stats right)
	{
		Stats addition= new Stats();
		addition.maxHealth=left.maxHealth-right.maxHealth;
		addition.health=Mathf.Clamp(left.health-right.health,0,left.maxHealth);  
		addition.strength=left.strength-right.strength;
		addition.speed=left.speed-right.speed;
		
		return addition;
	}

	public void SetEqual(Stats newStats)
	{
		maxHealth=newStats.maxHealth;
		health = newStats.health;
		strength = newStats.strength;
		speed = newStats.speed;
	}

}

[System.Serializable]
public class LevelInfo
{
	public int level=1;
	public int experience=0;
	public int experienceTilNxtLvl=3;
  public int skillpoints=0;

}

[System.Serializable]
public class StatusEffects//0->3 ranges
{
	public int sleepDerpivation=0;
	public int starvation=0;
	public int dimWit=0; //
  public int genius = 0;
	public int stirCrazy=0; //territory effect
	public int homeSick=0;
}

public enum DecayType
{
	ACTIVE, PASSIVE
}
[System.Serializable]
public class DecayRates
{
	public float sleepRate=.5f;
	public float hungerRate=.5f;
	public float exerciseRate=.5f;
	public float educationRate=.5f;
	public float wanderLustRate=.5f;
}

[System.Serializable]
public class Priorities
{
	public float sleep=0;//affects healing
	public float hunger=0;//acts like poison
	static public float education=0;//exp gain
	public float wanderLust=0;


	public DecayRates activeInTerritoryDecayRates;
	public DecayRates activeOutTerritoryDecayRates;
	public DecayRates passiveInTerritoryDecayRates;
	public DecayRates passiveOutTerritoryDecayRates;

	public static Priorities operator +(Priorities left, Priorities right)
	{
		Priorities addition= left;
		addition.sleep=Mathf.Clamp(left.sleep+right.sleep,0,100);
		addition.hunger=Mathf.Clamp(left.hunger+right.hunger,0,100); 
		addition.wanderLust=Mathf.Clamp(left.wanderLust+right.wanderLust,0,100); 

		return addition;
	}

	public void ResetToMax()
	{
		sleep=100;
		hunger=100;
		education=100;
		wanderLust=100;
	}

	public void Depreciate(float dt,DecayType myDecayType,bool inTerritory)
	{
		if(myDecayType== DecayType.ACTIVE)
		{
			if(inTerritory)
			{
				sleep-=dt*activeInTerritoryDecayRates.sleepRate;
				hunger-=dt*activeInTerritoryDecayRates.hungerRate;
				education-=dt*activeInTerritoryDecayRates.educationRate;
				wanderLust+=dt*activeInTerritoryDecayRates.wanderLustRate;
			}
			else
			{
				sleep-=dt*activeOutTerritoryDecayRates.sleepRate;
				hunger-=dt*activeOutTerritoryDecayRates.hungerRate;
				education-=dt*activeOutTerritoryDecayRates.educationRate;
				wanderLust-=dt*activeOutTerritoryDecayRates.wanderLustRate;
			}

		}
		else 
		{
			if(inTerritory)
			{
				sleep-=dt*passiveInTerritoryDecayRates.sleepRate;
				hunger-=dt*passiveInTerritoryDecayRates.hungerRate;
				education-=dt*passiveInTerritoryDecayRates.educationRate;
				wanderLust+=dt*passiveInTerritoryDecayRates.wanderLustRate;
			}
			else
			{
				sleep-=dt*passiveOutTerritoryDecayRates.sleepRate;
				hunger-=dt*passiveOutTerritoryDecayRates.hungerRate;
				education-=dt*passiveOutTerritoryDecayRates.educationRate;
				wanderLust-=dt*passiveOutTerritoryDecayRates.wanderLustRate;
			}
		}


		sleep=Mathf.Clamp(sleep,0,100);
		hunger=Mathf.Clamp(hunger,0,100); 
		education=Mathf.Clamp(education,0,125); 
		wanderLust=Mathf.Clamp(wanderLust,0,100); 
	}
}



[System.Serializable]
public class InteractionOptions
{
	public string functionName;
	public string displayName;
}


public struct PotentialCollision
{
	public Collider2D col;
	public Vector2 relativePos;
	public float distance;
	public Vector2 relativeVel;
	public float relativeSpeed;
}


/*
 * code to be saved for future projects!
 * 
 * 1. the fsm that both the ai & controllers are using but in a frame work way
 * 2. the debug star function (currently held by god) 
 * 3. the trace stack function (currently held by god)
 * 4. the whole singleton skeleton
 * 5. the start timer-> timer co routine function (currently held by god)
 * 6. isInlayerMask function (currently held by god)
 * 
 */
