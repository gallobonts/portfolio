using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericTree : InteractionBase 
{
  public float burnTime=3f;
  public int fruitAmount=3;

  public List<GameObject> PossibleRewards;
  public float minEruptDistance=10;
  public float maxEruptDistance=25;

  public Sprite burnTree;

	public override void Use(Brain_Base myBrain)
	{
    if (fruitAmount > 0)
  {
      fruitAmount--;
      EruptReward();
      GOD.myGOD.currentLevelManager.myMissionManager.CompleteObjective("Shake Tree");
  }

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

  void OnCollisionEnter2D(Collision2D col)
  {
    
    if (col.gameObject.tag == "Attack")
    {
      AttackInstance script = col.gameObject.GetComponent<AttackInstance>();
      if(script.myElementalType== ElementalType.fire)
      {
        Burn();
      }
    }
  }
  void Burn()
  {
    myGameObject.GetComponent<SpriteRenderer>().sprite = burnTree;

    Invoke("Die", burnTime);
  }

  void Die()
  {
    Destroy (this.gameObject);

  }

}
