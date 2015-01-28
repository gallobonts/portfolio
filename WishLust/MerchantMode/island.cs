using UnityEngine;
using System.Collections;
public class island : PlayerInteraction 
{
	public string levelName;

  	
	override public void  Interact()
	{
		Application.LoadLevel(levelName);
	}

}
