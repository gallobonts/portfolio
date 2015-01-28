#pragma strict

var localPosition:Vector3=Vector3(0,0,0);

function Start () 
{

}

function Update () 
{
this.transform.position= this.transform.parent.position+localPosition;
}
	