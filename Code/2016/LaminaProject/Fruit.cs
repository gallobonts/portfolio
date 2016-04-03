using UnityEngine;
using System.Collections;

public class Fruit : PickUp 
{
  public Priorities myPriorities;

  override public void Use()
  {
    myBrain.myPriorities += myPriorities;
    PutMeDown();
    GOD.myGOD.currentLevelManager.myMissionManager.CompleteObjective("Eat Fruit");
    Destroy(myGameObject);
  }

}
