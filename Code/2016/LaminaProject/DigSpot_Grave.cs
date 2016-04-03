using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DigSpot_Grave : MonoBehaviour 
{
  public List<GameObject> PossibleRewards;
  public float minEruptDistance=5;
  public float maxEruptDistance=10;

  Transform myTransform;

  void Awake()
  {
    myTransform= this.transform;
  }
  
  public void Use() 
  {
   if (PossibleRewards.Count != 0)
  {
    EruptReward();
  }
    Die ();
  }
  
  void EruptReward()
  {
    
    //instantiate the reward
    int rand = Random.Range(0,PossibleRewards.Count);
    GameObject reward= (GameObject)GameObject.Instantiate (PossibleRewards [rand],myTransform.position,myTransform.rotation);
    
    //erupt the reward
    Vector2 randDirection = new Vector2(0, 1);
    float randEruptionForce = Random.Range (minEruptDistance, maxEruptDistance);
    
    Vector2 addForce = randDirection * randEruptionForce;
    reward.GetComponent<Rigidbody2D> ().AddForce (addForce);
    
    
  }
  
  void Die()
  {
    //we will do a more elegant solution later involving the level manager doing the array-destroy method
    Destroy (this.gameObject);
  }
  void OnTriggerEnter2D(Collider2D col)
  {
    if (col.tag == "Player")
  {
      HumanController_Grave script= col.GetComponent<HumanController_Grave>();
      if(script)
      {
        script.ChangeDigState(true);
      }
  }
  
  }//on trigger enter
	
  void OnTriggerExit2D(Collider2D col)
  {
    if (col.tag == "Player")
    {
      HumanController_Grave script= col.GetComponent<HumanController_Grave>();
      if(script)
      {
        script.ChangeDigState(false);
      }
    }
    
  }//on 
}
