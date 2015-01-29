/*
	Base class for rewards dropped by monsters
*/
#pragma strict

public class Rewards extends MonoBehaviour
{

//time until the reward destroys itself
var f_Timer:float;

public function Initialize()
{
	f_Timer=2;
}

public function Run()
{
	f_Timer-=Time.deltaTime;
	if(f_Timer<0)
	{
		Destroy(this.gameObject);
	}

}

public function PowerUp(script:Controller)
{
	Debug.Log("Powerup original");
}

};



