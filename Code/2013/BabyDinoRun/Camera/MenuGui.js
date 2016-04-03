#pragma strict
/*
	Displays all the menu GUI elements
*/

//Overall GUI attributes
var button_width:float=125;
var button_height:float=50;
var rect_area:Rect= Rect(0,0,button_width,button_height);

//used to determine where in the menu you are
var i_MenuGUI:int;
private static var MAIN:int=0;
private static var INSTRUCTIONS:int=1;
private static var HIGHSCORE:int=2;

function Start()
{
	i_MenuGUI=MAIN;
}

function Update()
{
	//changes between the menu and the game 
	var GodRef : GodSingleton = GodSingleton.GetInstance();
	if(i_MenuGUI==MAIN)
	{
		if(Input.GetButton("Fire"))
		{
			GodRef.ResetGame();
			if(GodRef.i_DragonLevel==0)
				{GodRef.ResetCharacter();}
			Application.LoadLevel("GameFlow");
		}
	}

}

function OnGUI()
{
	var GodRef : GodSingleton = GodSingleton.GetInstance();
	switch(i_MenuGUI)
	{
	/*****************************************
	Main
	*******************************************/
	case MAIN:
		button_width=125;
		button_height=100;
		rect_area= Rect(0,0,button_width,button_height);

		GUILayout.BeginArea(rect_area);
		GUILayout.BeginVertical("box");

		if( GUILayout.Button("New Game"))
		{
			GodRef.ResetGame();
			GodRef.ResetCharacter();
			Application.LoadLevel("GameFlow");
		}
			if( GUILayout.Button("Continue"))
		{
			GodRef.ResetGame();
			if(GodRef.i_DragonLevel==0)
			{GodRef.ResetCharacter();}
			Application.LoadLevel("GameFlow");
			
		}
			
		if( GUILayout.Button("Instructions"))
		
			{i_MenuGUI=INSTRUCTIONS;}
		
		if( GUILayout.Button("View High Score"))
			{i_MenuGUI=HIGHSCORE;}
		GUILayout.EndVertical();
		GUILayout.EndArea();
		break;
		
	/*****************************************
	Instructions
	*******************************************/
	case INSTRUCTIONS:
		button_width=125;
		button_height=50;
		rect_area= Rect(0,0,button_width,button_height);

		var s_Instructions:String="Arrow keys to move, get as far up as possible, space bar to shoot & ctrl for shield, continue keeps your dragon stats while new game does not";
		
		GUILayout.BeginArea(rect_area);
		GUILayout.BeginVertical("box");

		if( GUILayout.Button("Back To Menu"))
		{i_MenuGUI=MAIN;}
		GUILayout.EndVertical();
		GUILayout.EndArea();

		GUI.Label(new Rect(Screen.width/2 -100,100,200,50), s_Instructions);
			break;
			
	/*****************************************
	High Score
	*******************************************/		
	case HIGHSCORE:
		button_width=125;
		button_height=50;
		rect_area= Rect(0,0,button_width,button_height);
		
		var f_HighScores:float[]= new float[10];
		//retrieve current scores
		for(var i:int=0;i<10;i++)
		{
			f_HighScores[i]=PlayerPrefs.GetInt(("HighScore")+i.ToString(),0);
		}
		
		var s_HighScore:String="Highscore # ";
		var s_HighScore2:String=" is ";
		
		GUILayout.BeginArea(rect_area);
		GUILayout.BeginVertical("box");

		if( GUILayout.Button("Back To Menu"))
		{i_MenuGUI=MAIN;}
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
		var f_YPosition:float=0;
		
		for(i=0;i<10;i++)
		{
		GUI.Label(new Rect(Screen.width/2 -200,f_YPosition,200,50), (s_HighScore+ (i+1).ToString()+s_HighScore2+f_HighScores[i].ToString()));
		f_YPosition+=50;
		}
		break;
		
	}//end switch

}
