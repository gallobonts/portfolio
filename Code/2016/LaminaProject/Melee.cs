using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Melee : AttackInstance 
{
  public float knockBackAmount;
  public List<GameObject> thingsAlreadyHit= new List<GameObject>();
  
  
  void OnTriggerEnter2D(Collider2D other)
  {
    if(other.gameObject==owner) {return;}
    for(int i=0; i<thingsAlreadyHit.Count;i++)
    {
      if(other.gameObject==thingsAlreadyHit[i]){return;}
    }
    
    KnockBack(other.gameObject);
    
    
  }
  
  void KnockBack(GameObject knockedObject)
  {
    thingsAlreadyHit.Add(knockedObject);
    Vector2 knockBackLocation= knockedObject.transform.position;
    knockBackLocation+=direction*knockBackAmount;
    
    knockedObject.transform.position=knockBackLocation;
    
    
    
  }
  
}
