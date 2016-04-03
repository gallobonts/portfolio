using UnityEngine;
using System.Collections;

public class skullCurrency : Collectable 
{
  public int currency=5;
  public override void Use()
  {
    GOD.myGOD.currentSkulls += currency;
  }

}
