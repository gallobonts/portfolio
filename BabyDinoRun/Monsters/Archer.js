var ArrowPrefab:GameObject;

/*
  Monster that shoots arrows
*/

public class Archer extends Monster
{
//max delay
var fireDelay;
//delay remaining
var fireTimer;
//determines if the archer can shoot an arrow
var canFire;
var myArrowPrefab;


public function Start()
{
	Initialize();

	
}

public function Initialize()
{
	super.Initialize(1//health
					,0//speed
					,1//damage
					,3//score
					,1//experience
					,50);//drop chance
						
	fireDelay=2.5f;
	fireTimer=0;
	canFire=false;

}

public function Update()
{

	Run();
}


public function Attack()
{
	//if can attack, then attack, else update timer to see if you can attack
	if(canFire)
	{
		
		fireTimer=fireDelay;
		canFire=false;
		Instantiate(ArrowPrefab,this.transform.position,this.transform.rotation);
	}
	else
	{
		fireTimer-=Time.deltaTime;
		if(fireTimer<=0.0f)
		{canFire=true;}
	}

	}

}// class end



















