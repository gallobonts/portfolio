#pragma strict

var DragonRef:GameObject;
var isShield:boolean;
var ShieldDuration:float;
var ShieldDurationTimer:float;

function Start()
{
isShield=false;
var DragonScript=DragonRef.GetComponent(Controller);
var f_Multiplier:float=DragonScript.i_ShieldLevel*.2;
ShieldDuration=1+f_Multiplier;
ShieldDurationTimer=ShieldDuration;
Deactivate();

}
function Update()
{

if(isShield)//deactivate shield timer
{
	ShieldDurationTimer-=Time.deltaTime;
	if(ShieldDurationTimer<=0)
	{Deactivate();}
}

}

public function Activate()
{
	this.gameObject.SetActive(true);
	isShield=true;
	var DragonScript=DragonRef.GetComponent(Controller);
	DragonScript.b_isShield=true;

}

private function Deactivate()
{
	isShield=false;
	ShieldDurationTimer=ShieldDuration;
	var DragonScript=DragonRef.GetComponent(Controller);
	DragonScript.b_isShield=false;
	this.gameObject.SetActive(false);
	
}

