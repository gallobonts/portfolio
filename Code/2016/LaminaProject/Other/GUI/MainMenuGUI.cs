using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuGUI : GUIMenuBase 
{

	public GameObject choosePlayerPanel;

	public Button newGameButton;

	override sealed public void ResetPanels()
	{
		mainMenuPanel.SetActive(false);
	}
	
	override sealed public void ChangeFlow(string newFlow) 
	{
		base.ChangeFlow(newFlow);

		switch(newFlow)
		{
		case "MainMenu":
		{
			mainMenuPanel.SetActive(true);
			SetFocus(newGameButton);

			break;
		}
	
		case "Back":
		{
			Back();
			break;
		}

		}//end switch


	}


	public void StartNewGame()
	{
    GOD.myGOD.isNewGame = true;

    GOD_Memory.instance.EraseGame();
		GOD.myGOD.LoadLevel("new_island");

	}

	public void LoadGame()
	{
		GOD.myGOD.LoadLevel("new_island");
	}


}
