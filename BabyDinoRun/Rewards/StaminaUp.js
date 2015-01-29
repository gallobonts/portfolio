/*
	reward that boosts stamina
*/
#pragma strict

public class StaminaUp extends Rewards
{
public function Start()
{
	Initialize();
}

public function Update()
{	
	//run basic reward tick
	Run();
}

public function PowerUp(script:Controller)
{
	script.f_Stamina+=5;
	if(script.f_Stamina>script.f_MaxStamina)
	{script.f_Stamina=script.f_MaxStamina;}
}

}