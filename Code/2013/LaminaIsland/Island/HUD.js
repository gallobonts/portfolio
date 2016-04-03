/*
	main hud
*/


enum PlayState{ATTACK,INTERACT,MENU};

var playstate:PlayState;
var length:int;
var cam;

function Start()
{
playState=PlayState.MENU;
cam=Camera.mainCamera;
var Lamina=GameObject.FindGameObjectsWithTag("lamina");
length=Lamina.length;

for(var i:int=0; i<length; i++)
{
	if(Lamina[i].GetComponent(AI).PlayerOwned)
	{
		cam.GetComponent(CamControl).Swap(i);
		i=length;
	}
}

}

function OnGUI()
{
switch(playstate)
{
case PlayState.MENU:
var Lamina=GameObject.FindGameObjectsWithTag("lamina");


var top_area:Rect= Rect(0,0,Screen.width, 50);
var left_area:Rect= Rect(0,50,100,Screen.height);
var right_area:Rect= Rect(Screen.width-100,50,100,Screen.height);//Rect(Screen.width-100,50,0,Screen.height);


GUILayout.BeginArea(top_area);//layout for to menu
GUILayout.BeginHorizontal("box");

//handles the hud's buttons
if(GUILayout.Button("Pause"))
{
}
if(GUILayout.Button("Store"))
{
	Application.LoadLevel("Store");
}
if(GUILayout.Button("MiniGames"))
{
	Application.LoadLevel("Mini_Games");
}
GUILayout.Label("Money: "+ 5);
GUILayout.EndHorizontal();
GUILayout.EndArea();




GUILayout.BeginArea(left_area);//layout for player laminas
GUILayout.BeginVertical("box");
GUILayout.Label("Player Owned Lamina");
for(var i:int=0; i<length; i++)
{
	if(Lamina[i].GetComponent(AI).PlayerOwned)
	{
		//if you click one of your own lamina, camera changes, and control changes
		if(GUILayout.Button(Lamina[i].name))
		{cam.GetComponent(CamControl).Swap(i);}
	}
}

GUILayout.EndVertical();
GUILayout.EndArea();

GUILayout.BeginArea(right_area);//layout for guest laminas
GUILayout.BeginVertical("box");
GUILayout.Label("Guest      Lamina");
for(i=0; i<length; i++)
{
	if(!Lamina[i].GetComponent(AI).PlayerOwned)
	{
		//if you click on a guest lamina, camera change and control nulls
		if(GUILayout.Button(Lamina[i].name))
		{cam.GetComponent(CamControl).Swap(i);}
	}
}
GUILayout.EndVertical();
GUILayout.EndArea();
break;

case PlayState.ATTACK:
break;

case PlayState.INTERACT:
break;

}//end switch
}//end gui
