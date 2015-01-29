/*
	switches the camera between the different lamina
*/
#pragma strict

var target:Transform; 
var damping:float = 6;
var smooth:boolean = true;
var GetSwap:boolean = false ;
var SwapNum:int=-1;
var length:int;

function Start () 
{
if (rigidbody)
	rigidbody.freezeRotation = true;
}

function Swap(n:int )
{
var LookObjects=GameObject.FindGameObjectsWithTag("lamina");
length= LookObjects.length;
if(n >=0 && n< length)
{
	if(SwapNum==-1)
	{
	SwapNum=n;
	target=LookObjects[SwapNum].transform;
	target.GetComponent(AI).Controller=1;

	}
	else
	{
	SwapNum=n;
	target.GetComponent(AI).Controller=0;
	target=LookObjects[SwapNum].transform;
	if(target.GetComponent(AI).PlayerOwned==true)
	{target.GetComponent(AI).Controller=1;}
	}
	
}

}


function LateUpdate () 
{
	
if(target)
{
var position= target.position;
position+=Vector3(0,10,-10);
		
	if(smooth)
	{
		var rotation= Quaternion.LookRotation(target.position-transform.position);

		//looks sloppy when done to the position, but rotates the camera slightly with the lamina
		transform.rotation=Quaternion.Slerp(transform.rotation,rotation,Time.deltaTime*damping);
		//transform.position=Vector3.Slerp(transform.position,position,Time.deltaTime*damping);
		transform.position=position;
	}
	else
	{
	transform.LookAt(target);
	transform.position=position;	
	}
}
	GetSwap=false;
}
