using UnityEngine;
using System.Collections;

public class Heart : Collectable 
{
  public int healAmount=25;
  public override void Use()
  {
    myBrain.currentStats.health= Mathf.Clamp(myBrain.currentStats.health+healAmount,0,myBrain.currentStats.maxHealth);

  }

}
