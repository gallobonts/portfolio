/*
	reward that raises health
*/
#pragma strict

public class HealthUp extends Rewards
{
public function Start()
{
	initialize();
}

public function Update()
{
	//run reward's tick function
	Run();
}

public function PowerUp(script:Controller)
{
	script.f_Health+=1;
	if(script.f_Health>script.f_MaxHealth)
	{script.f_Health=script.f_MaxHealth;}
}

}