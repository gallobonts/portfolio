
/*
	cloase range monster
*/
public class Knight extends Monster
{


public function Start()
{
	Initialize();

	
}

public function Initialize()
{

	super.Initialize(1//health
					,10//speed
					,1//damage
					,5//score
					,2//experience
					,50);//drop chance

	
}

public function Update()
{
Run();

}

//overides parent's attack function
public function Attack()
{

transform.position = Vector3.MoveTowards(transform.position, o_Player.position, Time.deltaTime*f_Speed);

}

}// class end
















