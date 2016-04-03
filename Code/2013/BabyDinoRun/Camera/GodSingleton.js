#pragma strict
/*
	used to track the entire game. Also used as an intermediate between scripts.
*/

class GodSingleton
{
private static var Instance : GodSingleton = null;


//upgrade-able
var i_DragonLevel:int;
var f_Health:float;
var f_Stamina:float;
var f_Speed:float;
var i_Strength:int;
var i_Style:int;
var i_Luck:int;
var i_ShieldLevel:int;

//boostable
var f_Experience:float;
var f_ExperienceNeeded:float;
var i_Score:int;


//level determinant
var f_Distance:float;
var i_MonsterCount:int;
var i_GameLevel:int;

//enumerated states
var e_intermediate:IntermediateState;

public static function GetInstance() : GodSingleton
{
	if(Instance == null)
  		{Instance = new GodSingleton();}  
   	return Instance;
}
public function LevelUp():boolean
{
if(f_Experience>=f_ExperienceNeeded)
{
	f_Experience-=f_ExperienceNeeded;
	f_ExperienceNeeded*=1.5;
	i_DragonLevel++;
	return true;
}
else
{return false;}

}

public function NextLevel()
{
	i_GameLevel++;
	f_Distance*=1.5;
	i_MonsterCount*=1.5;
	Application.LoadLevel("GameFlow");
}

public function ResetGame()
{
	f_Distance=350;
	i_MonsterCount=10;
	i_GameLevel=1;
	i_Score=0;
}

public function ResetCharacter()
{
	f_Stamina=25;
	f_Health=3;
	f_Speed=25;
	i_Strength=1;
	i_Strength=1;
	i_Style=1;
	i_Luck=1;
	i_ShieldLevel=1;
	
	i_DragonLevel=1;
	f_Experience=0;
	f_ExperienceNeeded=10;
}

    
            
private function GodSingleton() {}
}//end of class 