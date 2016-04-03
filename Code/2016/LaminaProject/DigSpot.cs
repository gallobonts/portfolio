using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DigSpot : InteractionBase 
{
	public List<GameObject> PossibleRewards;
	public float minEruptDistance=500000;
	public float maxEruptDistance=5;

	public override void Use() 
	{

      EruptReward();
			Die ();
	}

	void EruptReward()
	{

		//instantiate the reward
		int rand = Random.Range(0,PossibleRewards.Count);
		GameObject reward= (GameObject)GameObject.Instantiate (PossibleRewards [rand],myTransform.position,myTransform.rotation);

		//erupt the reward
		Vector2 randDirection= Random.insideUnitCircle;
		float randEruptionForce = Random.Range (minEruptDistance, maxEruptDistance);

		Vector2 addForce = randDirection * randEruptionForce;
		reward.GetComponent<Rigidbody2D> ().AddForce (addForce);


	}

	void Die()
	{
		//we will do a more elegant solution later involving the level manager doing the array-destroy method
		Destroy (this.gameObject);
	}
}
