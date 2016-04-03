using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

//Based off the PlayerManager class in the InControl- MultiplePLayer with bindings example
public class GOD_ControllerManager : MonoBehaviour 
{
	Controls keyboardListener;
	Controls joystickListener;
	static bool lookForPlayers=true;

void OnEnable()
	{
		keyboardListener = Controls.CreateWithKeyboardBindings();
		joystickListener = Controls.CreateWithJoystickBindings();
	}

void Update()
{
	if(lookForPlayers)
	{WaitForPlayerToRegister();}
	else{WaitForDisconnection();}

}

static void StopLookingForPlayers()
{
	lookForPlayers=false;
}
void WaitForPlayerToRegister()
{
		if(JoinGameWasPressed(joystickListener))
		{
			InputDevice inputDevice = InputManager.ActiveDevice;
			RegisterPlayer(inputDevice,joystickListener);
			joystickListener = Controls.CreateWithJoystickBindings();
		}
		else if(JoinGameWasPressed(keyboardListener))
		{
			InputDevice inputDevice = InputManager.ActiveDevice;
			RegisterPlayer(inputDevice,keyboardListener);
			keyboardListener = Controls.CreateWithKeyboardBindings();
		}
}

void RegisterPlayer( InputDevice inputDevice, Controls newControls )
{
	GOD.myGOD.RegisterController(inputDevice,newControls);
}

void WaitForDisconnection()
{

}
bool JoinGameWasPressed(Controls controller)
{
	return controller.start.WasPressed;
}
void OnDisable()
{
	joystickListener.Destroy();
	keyboardListener.Destroy();
}
}