

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
public function Attack()
{

transform.position = Vector3.MoveTowards(transform.position, o_Player.position, Time.deltaTime*f_Speed);

}

}// class end

/*
var dir:Vector3 = (o_Player.position-this.transform.position);
transform.Translate(dir.normalized*f_Speed);
//transform.Translate(Vector3(1,1,1));
circling monster's attack
*/

















