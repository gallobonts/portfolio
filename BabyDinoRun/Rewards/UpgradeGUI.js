/*
	gui used to let the player upgrade their character
*/

//#pragma strict

//gui box attributes
var button_width:float=125;
var button_height:float=50;
var rect_area:Rect= Rect(Screen.width/2,315,120,55);

//states of the menu
enum UpgradeState{SCORE,UPGRADE,NEXTLEVEL};
var e_upgrade:UpgradeState;

var f_Timer:float;
//2 random numbers representing which abilities you can upgrade
var r:int;
var r2:int;


function Start()
{
	e_upgrade=UpgradeState.SCORE;
	f_Timer=3;
	
	
	//used for the abilitys that you get to choose
	var i_AbilityCount:int=6;
	
	r=Random.Range(0,i_AbilityCount);
	r2=Random.Range(0,i_AbilityCount);
	if(r==r2)
	{r2=(r2+1)%i_AbilityCount;}//prevent them from being the same ability
}

function Update()
{
	var GodRef : GodSingleton = GodSingleton.GetInstance();

	//switch between menu states
	if(e_upgrade==UpgradeState.SCORE)
	{
		f_Timer-=Time.deltaTime;
		if(f_Timer<0||Input.GetButton("Fire"))
		{
			if(GodRef.LevelUp())
			{e_upgrade=UpgradeState.UPGRADE;}
			else
			{e_upgrade=UpgradeState.NEXTLEVEL;}
		}
		
	}
	if(e_upgrade==UpgradeState.NEXTLEVEL && Input.GetButton("Fire"))
	{
	GodRef.NextLevel();
	}


}
function OnGUI()
{
	var GodRef : GodSingleton = GodSingleton.GetInstance();

	var StatPage:String[]=new String[9];
	StatPage[0]="Dragon Level " +GodRef.i_DragonLevel.ToString();
	StatPage[1]="Experience til next level " +GodRef.f_ExperienceNeeded.ToString();
	StatPage[2]="Health " +GodRef.f_Health.ToString();
	StatPage[3]="Stamina " +GodRef.f_Stamina.ToString();
	StatPage[4]="Speed " +GodRef.f_Speed.ToString();
	StatPage[5]="Strength " +GodRef.i_Strength.ToString();
	StatPage[6]="Style " +GodRef.i_Style.ToString();
	StatPage[7]="Luck " +GodRef.i_Luck.ToString();
	StatPage[8]="Shield Level " +GodRef.i_ShieldLevel.ToString();

	var i_LoopLimit=StatPage.length;
	for(var i:int;i<i_LoopLimit;i++)
	{
		GUI.Label(new Rect(Screen.width-100,0+(i*50),100,50+(i*50)), StatPage[i]);
	}
		
	switch(e_upgrade)
	{
	case UpgradeState.SCORE:
		GUI.Label(new Rect(0,0,100,50), "You have beat level " +GodRef.i_GameLevel.ToString());
		var s_Score:String="Your Current Score is ";
		GUI.Label(new Rect(Screen.width/2 -100,100,200,50), s_Score + GodRef.i_Score.ToString());
			break;

	//ugrade state of the upgrade gui
	case UpgradeState.UPGRADE:
		
		GUI.Label(new Rect(0,100,200,50), "You leveled up. Choose an upgrade!");

		GUILayout.BeginArea(rect_area);
		GUILayout.BeginVertical("box");
		
		///determines which 2 upgrades base on the 2 random numbers r, to show the player
		switch(r)
		{
		case 0:
			if( GUILayout.Button("Upgrade Stamina"))
			{
				GodRef.f_Stamina+=5;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;
		case 1:
			if( GUILayout.Button("Upgrade Health"))
			{
				GodRef.f_Health+=1;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;
		case 2:
			if( GUILayout.Button("Upgrade Strength"))
			{
				GodRef.f_Health+=1;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;
		case 3:
			if( GUILayout.Button("Upgrade Style"))
			{
				GodRef.i_Style+=1;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;
		case 4:
			if( GUILayout.Button("Upgrade Speed"))
			{
				GodRef.f_Speed+=2;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;
		case 5:
			if( GUILayout.Button("Upgrade Luck"))
			{
				GodRef.i_Luck+=1;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;
		case 6:
			if( GUILayout.Button("Upgrade Shield"))
			{
				GodRef.i_ShieldLevel+=1;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;
		 }//end switch r

		 switch(r2)
		{
			case 0:
			if( GUILayout.Button("Upgrade Stamina"))
			{
				GodRef.f_Stamina+=5;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;
		case 1:
			if( GUILayout.Button("Upgrade Health"))
			{
				GodRef.f_Health+=1;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;
		case 2:
			if( GUILayout.Button("Upgrade Strength"))
			{
				GodRef.f_Health+=1;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;
		case 3:
			if( GUILayout.Button("Upgrade Style"))
			{
				GodRef.i_Style+=1;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;
		case 4:
			if( GUILayout.Button("Upgrade Speed"))
			{
				GodRef.f_Speed+=2;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;
		case 5:
			if( GUILayout.Button("Upgrade Luck"))
			{
				GodRef.i_Luck+=1;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;	
		case 6:
			if( GUILayout.Button("Upgrade Shield"))
			{
				GodRef.i_ShieldLevel+=1;
				e_upgrade=UpgradeState.NEXTLEVEL;
			}
			break;
			
		 }//end switch r2
		
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
		break;
		
	//after the player has chosen an upgrade, this is the screen they are given
	case UpgradeState.NEXTLEVEL:
		
		GUILayout.BeginArea(Rect(Screen.width/2-100,0,100,50));
		GUILayout.BeginVertical("box");
		if( GUILayout.Button("Continue"))
		{
			GodRef.NextLevel();
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
		break;

	}//end switch
}