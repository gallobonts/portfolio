using UnityEngine;
using System.Collections;

public class hut : InteractionBase {

  public Priorities myPriorities;

	override  public void Use(Brain_Base brain)
	{
    brain.myPriorities += myPriorities;
  }

}
