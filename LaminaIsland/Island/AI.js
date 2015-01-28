#pragma strict

import System.Collections.Generic;//system namespace, contains the list class


var laminaWidth=3;//currently just a random value to depict width of lamina...when a 3d model is present, the 3d model should provide a more appropriate width

var maxSpeed:float=10;//max linear speed
var maxAcceleration : float=5;//max linear acceleration
var maxRotation:float=1.5;//max angular speed
var maxAngularAcceleration:float=.1;//max angular acceleration
var myTarget : Transform;//target...when updated, this will be figured out via state changes instead of hard coded.
var currentState:State=State.WANDERING;//starting state
var arrived : boolean=false;//test to determine if you have arrived/escaped so that the state knows to change rather than keep trying to arrive
var facing : boolean=false;//sane as ^ but for rotations
var bInteractionComplete:boolean=false;
var myNumber:int;//lamina number used for debugging
var avoidNumber:int;// lamina number currently avoiding

//used in wander
private var timeToChange:float=3.8;//delay til change target
private var timeSoFar:float=0;//time elasped since last delay
private var changeTarget:boolean=true;//determines if it should change target
private var searchPosition:Vector3;//target's position that is sent to the seek function


//blended weights
private var totalAcceleration:Vector3=Vector3(0,0,0);//acceleration used to determine how the object moves
private var avoidCollisionAcceleration:Vector3=Vector3(0,0,0);//acceleration of the avoid collision behavior prior to multiplying it by it's weight
private var moveAcceleration:Vector3=Vector3(0,0,0);//acceleration of the movement behaviors prior to multiplying it by it's weight
private var asm:AIStateMachine;//state machine, responsible for changing states

private var timer:float;//used for various state changing purposes

var priorities:Priorities= new Priorities();//holds all the values that determine what goal the lamina is to seek
var Happiness:float;

var PlayerOwned:boolean;
var Controller:int=0;
var CurrentVelocity:Vector3;

function Start ()
{
	asm= AIStateMachine(this.gameObject,State.ARRIVING);//state machine, starts it at arriving
	asm.AddState(State.WANDERING);//add the various states
	asm.AddState(State.ARRIVING);
	asm.AddState(State.ESCAPING);
	asm.AddState(State.IDLE);
	asm.AddState(State.INTERACT);
	asm.AddState(State.CONTROL);
	asm.ChangeState(State.IDLE);

	myTarget=transform.FindChild("StartTarget");
	
}

function Update ()
{
	if(this.transform.position.y>=GameObject.FindWithTag("ground").transform.position.y+.5)
	//currently gravity is broken...when it collides with ground, gravity prevents movement, this takes that into account and turns gravity on/off depending on if it's touching ground
	{this.rigidbody.useGravity=true;}//not a fix, but a temp patch
	else
	{this.rigidbody.useGravity=false;}
	
	resetWeights();//sets all the accelerations to 0...should be renamed resetAccelerations eventually
	
	Happiness=priorities.FindHappiness();
	
	if(currentState==State.CONTROL)
	{if(Controller==0)
		{asm.ChangeState(State.WANDERING);}
	}
	else
	{if(Controller!=0)
		{asm.ChangeState(State.CONTROL);}
	}
	if(Controller!=0)
	{currentState=State.CONTROL;}
	
	switch(currentState)
	//switch that decides which behavior to activate....will eventually only provide the ones that it can directly access, (not seek or flee, and it will be able to change states on it's own
	{
		case State.CONTROL:
		CONTROL();
		break;
		case State.WANDERING:
		WANDERING();
		break;
		case State.IDLE:
		IDLE();
		break;
		case State.ARRIVING:
		if(arrived==false)
		{arrived=ARRIVING(myTarget.transform.position);}//stop the arrive method
		else
		{
		ARRIVING_end();
		}
		break; 
		case State.ALIGNING:
		if(arrived==false)
		{arrived=ALIGNING(myTarget.transform.forward);}
		break;
		case State.SEEKING:
		SEEKING(myTarget.transform.position);
		break;
		case State.FLEEING:
		FLEEING(myTarget.transform.position);
		break;
		case State.ESCAPING:
		if(arrived==false)
		{arrived=ESCAPING(myTarget.transform.position);}
		break;
		case State.INTERACT:
		INTERACT();
		break;
	}//end switch
	
	
	AvoidCollision();
	FaceWhereYourGoing();
	ApplyForces();//uses the accelerations, multiplies it by the weights and calculates the total acceleration
	priorities.Decrement(Time.deltaTime);
}

function CONTROL_begin()
{
CurrentVelocity=Vector3(0,0,0);
}
function CONTROL()
{
//var Vchange:boolean=false;
//var Hchange:boolean=false;
var Velocity:float=5;
var MaxVelocity:float=15;
var NewVelocity:Vector3;
var Friction:float=1;
var JumpVelocity=0;
var VerticalValue:float=Input.GetAxis("Vertical1")*Velocity;
//if(VerticalValue!=0)
//{Vchange=true;}
var HorizontalValue:float=Input.GetAxis("Horizontal1")*Velocity;
//if(HorizontalValue!=0)
//{Hchange=true;}
NewVelocity=Vector3(HorizontalValue,JumpVelocity,VerticalValue);
CurrentVelocity+=NewVelocity;

HorizontalValue=CurrentVelocity.x;
if(HorizontalValue<0)
{
/*	if(!Hchange)
	{HorizontalValue+=Friction*10;}
	else
	{HorizontalValue+=Friction;}
*/	HorizontalValue+=Friction;
	HorizontalValue= Mathf.Clamp(HorizontalValue,-MaxVelocity,0);
}
else
{
/*	if(!Hchange)
	{HorizontalValue-=Friction*10;}
	else
	{HorizontalValue-=Friction;}
*/	HorizontalValue-=Friction;
	HorizontalValue= Mathf.Clamp(HorizontalValue,0,MaxVelocity);
}

VerticalValue=CurrentVelocity.z;
if(VerticalValue<0)
{
/*	if(!Vchange)
	{VerticalValue+=Friction*2;}
	else
	{VerticalValue+=Friction;}
*/	VerticalValue+=Friction;
	VerticalValue= Mathf.Clamp(VerticalValue,-MaxVelocity,0);
}
else
{
/*	if(!Vchange)
	{VerticalValue-=Friction*2;}
	else
	{VerticalValue-=Friction;}
*/	
	VerticalValue-=Friction;
	VerticalValue= Mathf.Clamp(VerticalValue,0,MaxVelocity);
}

CurrentVelocity=Vector3(HorizontalValue,JumpVelocity,VerticalValue);

moveAcceleration=CurrentVelocity;    
}

function CONTROL_end()
{
}



function resetWeights()
//sets all the accelerations to 0...should be renamed resetAccelerations eventually
{
	totalAcceleration=Vector3(0,0,0);
	avoidCollisionAcceleration=Vector3(0,0,0);
	moveAcceleration=Vector3(0,0,0);
}

function ApplyForces()
//uses the accelerations, multiplies it by the weights and calculates the total acceleration
{
	var elipse=.1;
	var moveWeight:float=.3;//might need to be updated...currently much lower than avoid collision weight to give it precedance
	var AvoidCollisionWeight:float=.6;
	if(Controller!=0)
	{AvoidCollisionWeight=0;}
	totalAcceleration= moveWeight*moveAcceleration + AvoidCollisionWeight*avoidCollisionAcceleration;
	totalAcceleration.y=0;
	if(moveAcceleration.magnitude<elipse)
	{
	StopMoving();
	return;
	}
	
	if(maxAcceleration>totalAcceleration.magnitude)
	{
		totalAcceleration=totalAcceleration.normalized*maxAcceleration;
	}
	this.rigidbody.AddForce(totalAcceleration,ForceMode.Acceleration);//apply acceleration
}

function INTERACT_begin()
{

arrived=false;
bInteractionComplete=false;
}
function INTERACT()
{

if(!arrived)//first part is to get to the target
{
arrived=ARRIVING(myTarget.transform.position);
}
else//once there we call up it's interact script and follow whatever code it has in mind Essentially making 'smart objects'
{
	if(!bInteractionComplete)
	{
	var script=myTarget.GetComponent(targetScript);

	bInteractionComplete=script.Interact();
	}
	else
	{	
	asm.ChangeState(State.WANDERING);
//	currentState=State.WANDERING;
	}//end else interaction complete
}//end else arrived


}
function INTERACT_end()
{

arrived=false;
bInteractionComplete=false;
}

function IDLE_begin()
{
timer= Random.Range(3,6);


}

function IDLE()
{

timer-=Time.deltaTime;
if(timer<0)
{asm.ChangeState(State.WANDERING);
}
else if(Happiness<75)
{

var targets=GameObject.FindGameObjectsWithTag("target");

var checkDistance=30;
var targetList = new List.<GameObject>();
var targetExists:boolean=false;

for(var i:int=0;i<targets.length;i++)//find if object is in the checkcircle
{

	var testDistance= (this.transform.position-targets[i].transform.position).magnitude;
	if(checkDistance>=testDistance)
	{
		targetList.Add(targets[i]);//add the target to the list
		targetExists=true;
	}//end if
}//end for loop


if(targetExists)//there are targets in range
{
	var tempHappiness:float=Happiness;
	var officialTarget:Transform=myTarget;
	targetExists=false;
	for(target in targetList)//go through list of possible targets
	{
	
		var testHappiness=FindEntropy(target);//find happiness level from following that target

		if(testHappiness>tempHappiness)//if it's the heighest happiness level 
		{
			tempHappiness=testHappiness;
			officialTarget=target.transform;
			targetExists=true;
		}
	
	}
}
if(targetExists)//there are available targets
{
	myTarget=officialTarget;
	officialTarget.GetComponent(targetScript).SetUser(this.transform);
	asm.ChangeState(State.INTERACT);
}//end if target exists
}//end else wander=false

}//end function

function FindEntropy(target:GameObject):float
{
	var script=target.GetComponent(targetScript);
	return script.FindHappiness(this.transform.position,priorities);

}

function IDLE_end()
{

}


function SEEKING_begin()
{
}

function SEEKING(targetPosition:Vector3)
/*
Implementation notes:
Not to be used directly, should only be used IN other behaviors since there is no exit case
*/
{
	var direction:Vector3;//the variable for the direction the ai is trying to get to
	var myPosition:Vector3=this.transform.position;//the variable for position of game object
	var otherPosition : Vector3=targetPosition;//the variable for the targets position
	var applyForce:Vector3;
	direction = otherPosition-myPosition;//gets the difference between position
	direction.Normalize();//turns the difference into a direction
	applyForce= direction*maxAcceleration;//create acceleration
	moveAcceleration=applyForce;
	
}

function SEEKING_end()
{
}

function FLEEING_begin()
{
}

function FLEEING(targetPosition : Vector3)
/*
Implementation notes:
Not to be used directly, should only be used IN other behaviors since there is no exit case
*/

{
	var direction:Vector3;//the variable for the direction the ai is trying to get to
	var myPosition:Vector3=this.transform.position;//the variable for position of game object
	var otherPosition : Vector3=targetPosition;//the variable for the targets position
	var applyForce:Vector3;
	direction = otherPosition-myPosition;//gets the difference between position
	direction.Normalize();//turns the difference into a direction
	applyForce= direction*maxAcceleration*-1;//create acceleration
	moveAcceleration=applyForce;
}

function FLEEING_end()
{
}

function ESCAPING_begin()
{
}


function ESCAPING(targetPosition:Vector3) : boolean
/*
Implementation notes:
runs fast way from targetPos until it's further away, then walks, then when it's safe, stops running
*/
{
	var walkDist:float =20;//distance to be walking
	var stopDist:float= 3;//distance to be running
	var timeToArrive : float=.1;//variable to lower running speed when in walkind based on how far it is from the target
	var dist : float;//distance from target to character
	var direction:Vector3;//the variable for the direction the ai is trying to get to
	var myPosition:Vector3=this.transform.position;//the variable for position of game object
	var otherPosition : Vector3=targetPosition;//the variable for the targets position
	var targetVelocity: Vector3;
	var myVelocity:Vector3;
	var applyForce:Vector3;
	
	direction = myPosition-otherPosition;//gets the difference between position
	dist=direction.magnitude;
	direction.Normalize();//turns the difference into a direction
	FaceWhereYourGoing();
	
	if(dist>walkDist)
	{
		StopMoving();
		return true;
	}
	else if(dist>stopDist)
	{
		targetVelocity=direction*(maxSpeed*dist/walkDist);
	}
	else
	{
		targetVelocity= direction*maxSpeed;//create acceleration
	}
	myVelocity=this.rigidbody.GetPointVelocity(Vector3(0,0,0));
	applyForce= targetVelocity-myVelocity;
	
	applyForce/=timeToArrive;
	if(applyForce.magnitude>maxAcceleration)
	{
		applyForce*=maxAcceleration;
		
	}
	moveAcceleration=applyForce;
	return false;
	
}

function ESCAPING_end()
{
}


function ARRIVING_begin()
{
	arrived=false;
}

function ARRIVING(targetPosition:Vector3) : boolean
/*
Implementation notes:
runs towards target until it reaches walk distance, then walks until it reaches stop dist
*/
{
	var walkDist:float =20;
	var stopDist:float= 3;
	var timeToArrive : float=.1;
	var dist : float;
	var direction:Vector3;//the variable for the direction the ai is trying to get to
	var myPosition:Vector3=this.transform.position;//the variable for position of game object
	var otherPosition : Vector3=targetPosition;//the variable for the targets position
	var targetVelocity: Vector3;
	var myVelocity:Vector3;
	var applyForce:Vector3;
	
	direction = otherPosition-myPosition;//gets the difference between position
	dist=direction.magnitude;
	direction.Normalize();//turns the difference into a direction
	FaceWhereYourGoing();
	
	if(dist>walkDist)
	{
		targetVelocity= direction*maxSpeed;//create acceleration
	}
	else if(dist>stopDist)
	{
		targetVelocity=direction*(maxSpeed*dist/walkDist);
	}
	else
	{
		StopMoving();
		return true;
	}
	myVelocity=this.rigidbody.velocity;
	applyForce= targetVelocity-myVelocity;
	
	applyForce/=timeToArrive;
	if(applyForce.magnitude>maxAcceleration)
	{
		applyForce*=maxAcceleration;
		
	}
	moveAcceleration=applyForce;
	return false;
	
}

function ARRIVING_end()
{
	StopMoving();
}

function ALIGNING_begin()
{
}

function ALIGNING(aTarget : Vector3):boolean
/*
gets the same orientation as the target, used over several frames...currently not effecient for constantly changing target
*/
{
	var slowDist:float=1.0;//slow distance in radians(about 6 degrees)
	var stopDist:float=.017;//stop distance in radians(about 1 degree)
	var orientation:Vector3= this.transform.forward;//forward vector
	var mOrientation:float=Mathf.Atan2(orientation.x,orientation.z);//my direction in radians
	var tOrientation:float=Mathf.Atan2(aTarget.x,aTarget.z);//target direction in radians
	var dOrientation:float= tOrientation-mOrientation;
	var distanceTest:float=Mathf.Abs(dOrientation);
	var tRotation:float;
	var tAcceleration:float;
	var timetoAlign:float=.1;
	
	
	if(distanceTest>slowDist)//still turning max amount
	{
		
		tRotation=maxRotation;
		
	}
	
	else if(distanceTest<stopDist)//stop the spinning already
	{
		this.rigidbody.angularVelocity=Vector3(0,0,0);
		return true;
	}
	else//scale down the rotation
	{
		tRotation=maxRotation*dOrientation/slowDist;
		tRotation=tRotation-(this.rigidbody.angularVelocity).magnitude;
		tRotation/=timetoAlign;
	}
	var rotateVector:Vector3=Vector3(0,tRotation,0);
	this.rigidbody.AddRelativeTorque(rotateVector,ForceMode.Acceleration);
	
	return false;
}

function ALIGNING_end()
{
}

function FaceWhereYourGoing()
//currently kinematic, eventually will use align to constantly turn the same direction the object is moving
{
	var direction=this.rigidbody.velocity.normalized;
	direction.y=0;
	if(direction.magnitude>0)
	//{Align(direction);}
	this.transform.forward=direction;//kinematic solution...because the above one is being a pain
}

function WANDERING_begin()
{
myTarget=transform.FindChild("StartTarget");
timer= Random.Range(5,12);

}

function WANDERING()
/*
Implementation Notes:
uses an invisible carrot placed randomly on the radius of a circle drawn a few units ahead of the object, using the seek to move the character towards this object
timer involved to delay the target change. too high a value and character randomly starts to back-steer, too low and character moves straight, unable to turn towards the target
*/
{
	timer-=Time.deltaTime;
	if(timer<0)
	{asm.ChangeState(State.IDLE);}
	var searchDist:float=5;
	var searchRad:float=10;
	var searchDirection:Vector3=Vector3(Random.Range(-10.0,10.0),0,Random.Range(-10.0,10.0));
	var wanderRate:float=.5;
	
	timeSoFar+=Time.deltaTime;
	if(timeSoFar>=timeToChange)
	{changeTarget=true;timeSoFar=0;}
	
	if(changeTarget==true)
	{
		
		searchPosition= this.transform.position+ this.transform.forward*searchDist;
		
		searchDirection*=wanderRate;
		searchDirection.Normalize();
		searchPosition+= searchRad*searchDirection;
		changeTarget=false;
	}
	myTarget.transform.position=searchPosition;
	SEEKING(searchPosition);

}

function WANDERING_end()
{
}

function StopMoving()
{
	//exit clause for turning
	this.rigidbody.angularVelocity=Vector3(0,0,0);
	//exit clause for moving
	var myVelocity:Vector3=this.rigidbody.GetPointVelocity(Vector3(0,0,0));
	this.rigidbody.AddForce(-myVelocity,ForceMode.VelocityChange);
}


function AvoidCollision()
/*
implementation notes:
finds the closest object that might intercept it and adjusts it's acceleration appropriately
hoping to use the lamina tracker object to get the list of lamina's but currently just uses it directly since using the function from the other script isn't working out right now
*/
{
	var laminas:GameObject[];
	var laminaTracker = GameObject.Find("laminaTracker");
	//laminas=laminaTracker.GetComponent(TrackLamina).getLamina();
	laminas = GameObject.FindGameObjectsWithTag("lamina");
	var myRadius=laminaWidth;
	var otherRadius:float;
	
	
	
	//information pertaining to the object that is closest to colliding with the object
	var closestTarget:int=-1;
	var shortestTime:float= Mathf.Infinity;
	var closestMinSeperation:float;
	var closestDistance:float;
	var closestRelativePos:Vector3;
	var closestRelativeVel:Vector3;
	var distanceCheck=10;
	
	//find closest to collision
	for(var target:int=0;target<laminas.length;target++)
	{
		if(laminas[target].transform.position!=this.transform.position)//prevents checking against self
		{
			var relativePos:Vector3 = laminas[target].transform.position-this.transform.position;
			if(Mathf.Abs(relativePos.magnitude)>= distanceCheck)
			{
				var relativeVel:Vector3 = laminas[target].rigidbody.GetPointVelocity(Vector3(0,0,0))
												- this.rigidbody.GetPointVelocity(Vector3(0,0,0));
				var relativeSpeed:float= relativeVel.magnitude;
				var collisionTime:float= Vector3.Dot(relativePos,relativeVel)/(relativeSpeed*relativeSpeed)*-1;//dot product of velocity*position divided by speed squared
				var tempOtherRadius=laminas[target].GetComponent(AI).laminaWidth;
				var posMag=relativePos.magnitude;
				var minSeparation:float=posMag-relativeSpeed*collisionTime;//the closest these two objects will get to each other
				if(minSeparation> myRadius+tempOtherRadius)//if a collision will occur
				{
					if((shortestTime>collisionTime) && (collisionTime>=0))//if this collision will happen before any other possible collisions we have found
					//set all of the information for the closest collision
					{
						shortestTime=collisionTime;
						closestTarget=target;
						closestMinSeperation=minSeparation;
						closestDistance=posMag;
						closestRelativePos=relativePos;
						closestRelativeVel=relativeVel;
						otherRadius=tempOtherRadius;
						avoidNumber=target;
					}//exit copy to target
				}//exit collision check
			}//exit distancecheck
		}//exit if target != this object
		else{myNumber=target;}
	}//exit for loop
	avoidCollisionAcceleration=Vector3(0,0,0);
	if(closestTarget!=-1)//exit if no object will collide
	{
		if(closestMinSeperation>=0 && closestDistance> myRadius+otherRadius)////if we have not already collided
			{closestRelativePos=closestRelativePos+closestRelativeVel*shortestTime;}
		avoidCollisionAcceleration=closestRelativePos.normalized*maxAcceleration/(closestDistance*.5);
	}
}
				
				
function DebugStar(pos : Vector3)
//draws a star at said point...can be used to figure out what targets you are setting for debugging purposes
{
	var start : Vector3;
	var end : Vector3;
	start = pos + Vector3( 0.5, 0, 0 );
	end = pos + Vector3( -0.5, 0, 0 );
	Debug.DrawLine ( start, end, Color.red, 0, false );
	start = pos + Vector3( 0, 0.5, 0 );
	end = pos + Vector3( 0, -0.5, 0 );
	Debug.DrawLine ( start, end, Color.red, 0, false);
	start = pos + Vector3( 0, 0, 0.5 );
	end = pos + Vector3( 0, 0, -0.5 );
	Debug.DrawLine ( start, end, Color.red, 0, false);
}
				