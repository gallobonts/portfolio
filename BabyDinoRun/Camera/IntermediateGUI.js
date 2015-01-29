#pragma strict

/*
 	GUI used to  switch between menu and game
*/

//menu attributes
var button_width:float=125;
var button_height:float=50;
var rect_area:Rect= Rect(465,315,120,55);

//determines how you lost
enum IntermediateState{HEALTHLOSE,STAMINALOSE};
var currentState:IntermediateState;


function Start()
{
	//retrieve current state from Singleton class
	var GodRef : GodSingleton = GodSingleton.GetInstance();
	currentState=GodRef.e_intermediate;
}

function Update()
{
	//switches to main menu for you
	if(Input.GetButton("Fire"))
	{
		Application.LoadLevel("MainMenu");
	}


}

function OnGUI()
{
	//displays a label dictating to the player why they died
	switch(currentState)
	{

	case IntermediateState.HEALTHLOSE:

		GUI.Label(new Rect(0,0,100,50), "You have died from running out of health.");
		break;
	case IntermediateState.STAMINALOSE:
		GUI.Label(new Rect(0,0,100,50), "You have died from running out of stamina.");
		break;

	}

		GUI.Label(new Rect(Screen.width/2 -100,0,100,50), "Press the SpaceBar to go back to the Main Menu");
}