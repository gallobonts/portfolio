#pragma strict

var DragonRef:GameObject;
var f_NormalDam:float;
var f_SizeConst:float;

function Start () 
{
f_NormalDam=10;
f_SizeConst=.45;
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
	if(other.CompareTag("monster"))
	{
	var DragonScript=DragonRef.GetComponent(Controller);
	var MonsterScript=other.gameObject.GetComponent(Monster);
	var f_Damage:float= DragonScript.i_Strength* f_NormalDam*Time.deltaTime;
	MonsterScript.TakeDamage(f_Damage);

	}
	
}