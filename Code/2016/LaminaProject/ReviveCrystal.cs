using UnityEngine;
using System.Collections;

public class ReviveCrystal : MonoBehaviour 
{
  
  void OnTriggerEnter2D( Collider2D col )
  {
    if(col.tag=="Player")
    {
      if(GOD.myGOD.ReviveLamina())
      {Destroy(this.gameObject);}
    }
  }
	
}
