using UnityEngine;
using System.Collections;

public class questgiver : PlayerInteraction {
	public Dialouge myDialouge;
	int questStage=0;
	KeyItems mykeyItem= KeyItems.poop;
	string stage1Dialouge= "Hurray, thank you. Take this health stone in return";
	string stage2Dialogue= "now, don't go getting yourself killed";

	public override void Interact ()
	{
		if(myDialouge.frameCount!=Time.frameCount)
		{
			Controls script= (Controls) playerTarget.GetComponent("Controls");
			switch(questStage)
			{
			case 0:
				questStage++;
				break;
			case 1:
				if(script.myKeyItems.Contains(mykeyItem))
				{
					questStage++;
					myDialouge.text[0]=stage1Dialouge;
					script.maxHealth++;

				}
				break;
			case 2:
				questStage++;
				myDialouge.text[0]= stage2Dialogue;
				break;

			}
			myDialouge.playDialogue=true;

		}
	}
	
}
