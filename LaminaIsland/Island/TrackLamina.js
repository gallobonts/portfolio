#pragma strict

var laminas:GameObject[];//holds all of laminas in the level
function Start ()
{
	laminas = GameObject.FindGameObjectsWithTag("lamina");
}

function Update ()
{
	
}

function getLamina():GameObject[] 
{
	return laminas;
}