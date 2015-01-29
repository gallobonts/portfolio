/*
	Arrow projectile
*/
public class Arrow extends Projectile
{
var v3_Target:Vector3;
public function Start()
{
	v3_Target=GameObject.Find("dragoncube").transform.position;

	super.Initialize(.2,//speed
					1,//damage
					v3_Target);//direction

}

public function Update()
{
	//calls projectile's tick function
	Run();
}

}// class end

















