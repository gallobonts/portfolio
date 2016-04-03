using UnityEngine;
using System.Collections;

public class adventureBoat :PlayerInteraction 
{
	private string levelName="merchantMode";


	override public void  Interact()
	{
		//save all data
		GameObject [] players= GameObject.FindGameObjectsWithTag("Player");
		for(int i=0;i<players.Length;i++)
		{
			Controls script= (Controls)players[i].GetComponent("Controls");
			script.EnterMerchantMode();
		}

		Application.LoadLevel(levelName);
	}

}
