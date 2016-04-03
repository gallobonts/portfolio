using UnityEngine;
using System.Collections;

[System.Serializable]
public class SkillSets
{
	public SkillTreeNodes Level1Skill;
	public SkillTreeNodes[] Level2Skill= new SkillTreeNodes[2];
	public SkillTreeNodes[] Level3Skill= new SkillTreeNodes[4];
	public SkillTreeNodes[] Level4Skill= new SkillTreeNodes[8];

}

public class SkillTreeBase : MonoBehaviour 
{
	public SkillSets[] mySkillSets= new SkillSets[25];

}
