using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class MainMenuLevelManager : LevelManager
{
//Menu stuff
public	MainMenuGUI mainMenuGUI;
	

	override public void  InitializeLevel(LevelManagerInitialization initialize)
{
	
}//end initalize level	


override public void RegisterController(InputDevice inputDevice,Controls newControls)
{
  //if starting from menu
	if(!firstPlayerSet)//register player 1 in menu
	{
		//prevent re-registering p1
		firstPlayerSet=true;
    
		//setup input & controls
		firstPlayerControls=newControls; 
		firstPlayerInputDevice=inputDevice;
    
    

		//remove the 'press start' hud item
		registerFirstPlayer.SetActive(false);
	
    //set up the main menu & it's input device
		mainMenuGUI.ChangeFlow("MainMenu");
		mainMenuGUI.SetDevice(1,inputDevice,newControls);
		return;
	}

}//register controller


}
