#pragma strict


var DragonRef:GameObject;
var f_YPosition:float;
var CameraSlow:float;

function Start()
{
DragonRef= GameObject.Find("dragoncube");
CameraSlow=60;
}

function Update () 
{
f_YPosition=(DragonRef.transform.position.y-transform.position.y)/CameraSlow;
transform.position.y+= f_YPosition;
}


