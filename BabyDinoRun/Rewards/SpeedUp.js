/*
	reward that increases the player's speed
*/

#pragma strict

public class SpeedUp extends Rewards
{
public function Start()
{
	Initialize();
}

public function Update()
{
	//run reward tick
	Run();
}

public function PowerUp(script:Controller)
{
	script.f_HighSpeed*=1.05;
	script.f_LowSpeed=script.f_HighSpeed/2;
}

}