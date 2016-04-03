#pragma strict
/*
	produces the visual effects of the fire as well as handles collisions
*/

var DragonRef:GameObject;
var f_NormalDam:float;
var f_SizeConst:float;

function Start () 
{
	//damage done to monsters
	f_NormalDam=10;
	//size of fire
	f_SizeConst=.45; 
	//get a hold of the controller to determine how much to scale up the fire
	var DragonScript=DragonRef.GetComponent(Controller);
	var newSize:float=DragonScript.i_Style;
	transform.localScale += Vector3(newSize,newSize,0);
	transform.position.y+=newSize*f_SizeConst;
}

function Update () 
{

}


function OnTriggerStay(other: Collider)
{
	//damages the monsters
	if(other.CompareTag("monster"))
	{
	var DragonScript=DragonRef.GetComponent(Controller);
	var MonsterScript=other.gameObject.GetComponent(Monster);
	var f_Damage:float= DragonScript.i_Strength* f_NormalDam*Time.deltaTime;
	MonsterScript.TakeDamage(f_Damage);

	}
	
}