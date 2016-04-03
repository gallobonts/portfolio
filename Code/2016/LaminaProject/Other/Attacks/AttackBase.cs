using UnityEngine;
using System.Collections;
using System;

public enum Team
{
	PLAYER,AI,ENEMY
}

public enum Range
{
	CLOSE,
	MID,
	LONG
}

public enum ElementalType
{
	none,
	fire
}

public abstract class AttackBase : MonoBehaviour, IComparable
{
	public ElementalType myElementalType;
	public attackNode myAttack;
	public GameObject source;


	//can attack information
	public float delayInSeconds;
	public float deathCountDown;
	public bool canAttack;

	public Team myTeam;

	public Range myRange;
	public int level;
	public float damage;

	abstract public void UseAttack(Vector2 direction);
	abstract public void LevelUp();

	public void SetUp(GameObject newSource,Team newTeam)
	{source = newSource;myTeam=newTeam;}

	//used to allow attackBase to be found in a list
	public int CompareTo(object obj) 
	{
		return this.myAttack.CompareTo((obj as AttackBase).myAttack);

	}
}
