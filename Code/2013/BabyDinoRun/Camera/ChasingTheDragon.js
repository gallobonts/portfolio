#pragma strict
/*
	causes the camera to follow the dragon
*/


//reference to the dragon
var DragonRef:GameObject;
//the y position of the camera
var f_YPosition:float;
//tweaked variable to keep the camera from going too fast
var CameraSlow:float;

function Start()
{
	//find the dragon in the scene to be used later
	DragonRef= GameObject.Find("dragoncube");
	//tweaked value
	CameraSlow=60;
}

function Update () 
{
	//finds the difference between the dragon and camera's position and lerps the camera to the dragon smoothly
	f_YPosition=(DragonRef.transform.position.y-transform.position.y)/CameraSlow;
	transform.position.y+= f_YPosition;
}


