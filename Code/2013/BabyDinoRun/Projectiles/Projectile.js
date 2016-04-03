/*
	Base class for all projectiles
*/

public class Projectile extends MonoBehaviour
{
var f_Speed:float;
var f_Damage:float;
var v3_Direction:Vector3;

public function Initialize(f_newSpeed:float,f_newDamage:float,v3_newDirection:Vector3)
{
	//sets basic projectile atrtributes
	f_Speed=f_newSpeed;
	var GodRef : GodSingleton = GodSingleton.GetInstance();
	var f_DifficultyBoost=1+ .2*GodRef.i_GameLevel;
	f_Damage=f_newDamage*f_DifficultyBoost;

	//determine which direction to translate towards
	v3_Direction=(this.transform.position-v3_newDirection).normalized*f_Speed;

}

//basic tick function
public function Run()
{
	if(renderer.isVisible)
	{
		transform.Translate(v3_Direction);
	}
	else
	{Destroy(this.gameObject);}

}

public function DealDamage():float
{
	return f_Damage;
}

function OnTriggerEnter(other: Collider)
{
	//allow fire to destroy the projectile
	if(other.CompareTag("fire"))
	{Destroy(this.gameObject);}
}
}//end class




