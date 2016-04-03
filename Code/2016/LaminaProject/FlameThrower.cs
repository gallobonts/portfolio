using UnityEngine;
using System.Collections;

public class FlameThrower : PickUp 
{
  public AttackInstance flame;
  public float flameDistance=.25f;

	override public void Use()
  { 
    if(!canUse){return;}
    base.Use();

    Vector2 position = myTransform.position;
    position += new Vector2(myTransform.localScale.x * flameDistance, 0);
    flame.myTransform.position = position;
    flame.InvokeSuicide();
    //flame.Use(direction, position);
	}

}
