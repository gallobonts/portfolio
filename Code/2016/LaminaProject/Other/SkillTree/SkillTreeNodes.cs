using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//decides what type of node we are looking at
public enum nodeType
{
	Attack,Skill
}

//contains all possible attacks
public enum attackNode 
{
	Scratch,FireBall
}
//lists all the skills
public enum skillNode
{
	Health,Speed
}

public class SkillTreeNodes : MonoBehaviour 
{
	public nodeType myNodeType;
	public attackNode myAttackNode;
	public skillNode mySkillNode;

}
