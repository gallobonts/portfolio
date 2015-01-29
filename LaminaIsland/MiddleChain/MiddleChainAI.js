/*
	middlechain= mini game
	ai for monsters in mini game
*/
#pragma strict

var state:State;
//var size:int;
var xCameraBounds:Vector2=Vector2(70,109);//not the best solution by far...currently just a ghetto fix until a better solution is found
var zCameraBounds:Vector2=Vector2(-42,-64);//still unsure how to find these values...attacking the unity's forums to the extreme
var xzCameraBounds:Rect=Rect(xCameraBounds.x,zCameraBounds.y,xCameraBounds.y-xCameraBounds.x,zCameraBounds.x-zCameraBounds.y);

var size:int= Random.value*100;
enum ScreenSide{Left,Right,Top,Bottom};
var test:ScreenSide;
var target:Transform;
var direction:Vector3;
var speed:float=.1;
var readyToChange:boolean=false;
var timer:float=5;
var deathTimer:float=10;
var id:int=Random.value*1000;

function Start () 
{
state= State.BIRTH;
}


function Update ()
{
var testDeath:Vector2=Vector2(this.transform.position.x,this.transform.position.z);
//print("xzCameraBounds= " +xzCameraBounds+" testDeath= "+testDeath);
//print(xzCameraBounds.Contains(testDeath));
if(!xzCameraBounds.Contains(testDeath))
{

if(deathTimer>0)
{deathTimer-= Time.deltaTime;
	if(deathTimer<0)
	{state=State.DEATH;timer=8;}}


}

if(timer>0)
{timer-= Time.deltaTime;
	if(timer<0)
	{readyToChange=true;}}
	
switch(state)
//switch that decides which behavior to activate....will eventually only provide the ones that it can directly access, (not seek or flee, and it will be able to change states on it's own
{

	case State.BIRTH:
	Birth();
	break;
	case State.DEATH:
	Death();
	break;
	case State.SEEKING:
	Seek();
	break;
	case State.FLEEING:
	Flee();
	break;
	case State.WANDERING:
	Wander();
	break;
}
}


function OnCollisionEnter(collision:Collision)
{
if(collision.rigidbody.tag=="enemy")
{
	//var script=collision.other.GetComponent(MiddleChainAI);
	var script=collision.rigidbody.GetComponent(MiddleChainAI);
	if(size>script.size)
	{
		script.state=State.DEATH;
	}
	else
	{
		state=State.DEATH;
	}

}//enemy && enemy collision

else if(collision.rigidbody.tag=="player")
{
	var player_script=collision.rigidbody.GetComponent(MiddleChainAI);
	
	if(size>player_script.size)
	{
		player_script.state=State.DEATH;
	}
	else
	{
		state=State.DEATH;
	}
}

}//end oncollisionenter


function Flee()
{
direction= target.transform.position-this.transform.position;
direction.Normalize();
direction.y=0;
this.transform.Translate(direction*speed*-1);

}

function Birth()
{


}

function Birth_End()
{


var side:ScreenSide;
var findSide:float=Random.value;
var position:Vector3;

if(findSide>.25)
{
	if(findSide>.50)
	{
		if(findSide>.75)
		{side=ScreenSide.Bottom;}//bigger than .75
		else{side=ScreenSide.Top;}//in between .5 and .75
	}
	else{side=ScreenSide.Right;}//in between .25 and .5
}
else{side=ScreenSide.Left;}//smaller than .25

test=side;
switch(side)
{
case ScreenSide.Left:
	position.x=xCameraBounds.x;
	position.z=Random.Range(zCameraBounds.x,zCameraBounds.y);
	break;
case ScreenSide.Right:
	position.x=xCameraBounds.y;
	position.z=Random.Range(zCameraBounds.x,zCameraBounds.y);
	break;
case ScreenSide.Top:
	position.x=Random.Range(xCameraBounds.x,xCameraBounds.y);
	position.z=zCameraBounds.x;
	break;
case ScreenSide.Bottom:
	position.x=Random.Range(xCameraBounds.x,xCameraBounds.y);
	position.z=zCameraBounds.y;	
	break;
}
position.y=0;

this.transform.position=position;
Wander_begin(side);
}


function Death()
{
}

function Seek()
{
direction= target.transform.position-this.transform.position;
direction.Normalize();
direction.y=0;
this.transform.Translate(direction*speed);
}

function Wander_begin()
{
direction= Random.insideUnitSphere;
direction.y=0;
state=State.WANDERING;
}

function Wander_begin(side:ScreenSide)
{
switch(side)
{
case ScreenSide.Left:
	direction.x=xCameraBounds.y-this.transform.position.x;
	direction.z=0;
	break;
case ScreenSide.Right:
	direction.x=xCameraBounds.x-this.transform.position.x;
	direction.z=0;
	break;
case ScreenSide.Top:
	direction.x=0;
	direction.z=zCameraBounds.x;
	break;
case ScreenSide.Bottom:
	direction.x=0;
	direction.z=-zCameraBounds.y;	
	break;
}
direction.y=0;
state=State.WANDERING;
}
function Wander()
{
direction.Normalize();
direction.y=0;
this.transform.Translate(direction*speed,Space.World);

if(timer>0)
{timer-= Time.deltaTime;
	if(timer<0)
	{readyToChange=true;}
}
}




