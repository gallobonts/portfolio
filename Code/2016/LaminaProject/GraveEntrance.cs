using UnityEngine;
using System.Collections;

public class GraveEntrance : DigSpot 
{
	public string LoadLevel;

	public override void Use() 
	{
    GOD.myGOD.LoadLevel(LoadLevel);
	}

	


}
