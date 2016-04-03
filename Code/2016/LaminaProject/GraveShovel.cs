using UnityEngine;
using System.Collections;

public class GraveShovel :  PickUp 
{
  public DigInstance myDigInstance;


  override public void Use()
  {
    if(!canUse){return;}
    base.Use();

    //3. create dig spot
    //find spawn location
    Vector2 spawnLocation= myTransform.position;
    
    //use the instance
    myDigInstance.Use (spawnLocation);
    
    

  }

}
