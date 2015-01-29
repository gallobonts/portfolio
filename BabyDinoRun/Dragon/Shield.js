#pragma strict

/*
blocks the player from monster attacks for a short duration
*/

var DragonRef:GameObject;
var isShield:boolean;

//max time the shield has until it goes away
var ShieldDuration:float;
//how much time left the shield has
var ShieldDurationTimer:float;

function Start()
{
	//set the shield to off at the start
	isShield=false;
	var DragonScript=DragonRef.GetComponent(Controller);

	//determine the shield's duration from the dragonscript
	var f_Multiplier:float=DragonScript.i_ShieldLevel*.2;
	ShieldDuration=1+f_Multiplier;
	ShieldDurationTimer=ShieldDuration;
	//turns shield off
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

//save cpu by turning on and off the shield component rather than re-creating the shield each time
public function Activate()
{
	this.gameObject.SetActive(true);
	isShield=true;
	var DragonScript=DragonRef.GetComponent(Controller);
	DragonScript.b_isShield=true;

}

//save cpu by turning on and off the shield component rather than re-creating the shield each time
private function Deactivate()
{
	isShield=false;
	ShieldDurationTimer=ShieldDuration;
	var DragonScript=DragonRef.GetComponent(Controller);
	DragonScript.b_isShield=false;
	this.gameObject.SetActive(false);
	
}

