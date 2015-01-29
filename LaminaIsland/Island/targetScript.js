/*

	intermediate script used by the AI
*/
#pragma strict

var priorities:Priorities;
var done:boolean=false;
var user:Transform=null;


function Update () {

}

function FindHappiness(otherPosition:Vector3,tempPriorities:Priorities):float//finds the happiness determined from this 
{
var direction= this.transform.position-otherPosition;
var distance:float=direction.magnitude;

if(user==null)
{
return tempPriorities.FindNewHappiness(distance,priorities);
}
else
{
return -1;
}

}

function SetUser(newUser:Transform)
{
user=newUser;
}

function Interact():boolean
{
done=false;
SendMessage("Interaction");
if(done==true)
{
//print(user+" just used the "+ this.name);
user.GetComponent(AI).priorities=user.GetComponent(AI).priorities.Add(priorities);
user=null;
}//if done, set user back to empty

return done;
}