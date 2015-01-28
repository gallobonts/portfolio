//remove pragma strict to allow dynamic type casting


enum State{NULLSTATE,WANDERING,ARRIVING,ALIGNING,SEEKING,FLEEING,ESCAPING,BIRTH,DEATH,IDLE,INTERACT,CONTROL};
var state_length:int=System.Enum.GetValues(State).Length;
var current_state:State;
var NULL_VECTOR3:Vector3= Vector3(Mathf.Infinity,Mathf.Infinity,Mathf.Infinity);//constant...not supported, but if afraid of changing it, can always create a constant function

public class AIStateMachine extends MonoBehaviour //holds all of the states, and changes them appropriately
{
var states:State[]=new State[9];

var entity:GameObject;//entity that owns the AIStateMachine

public function AIStateMachine(input:GameObject,start_state:State)
{
entity=input;//set the entity
current_state=start_state;
//print("make me");
}

public function AddState(state:State)
{

//print("addstate");
for(var i:int=0;i<states.length;i++)
{
	//print("i="+i+"states.length="+states.Length+"states[i]="+states[i]+"state="+state);
	if(states[i]==State.NULLSTATE ||(states[i]!= state && i==states.length-1))
	{
	//print("added");
	states[i]=state;//add to the array
	break;
	}
	else if(states[i]==state)
	{
	break;
	}

}

}


public function ChangeState(new_state:State)
{
	var quit:boolean=true;
	var quit2:boolean=true;

//	print(states.length);
	for(var i:int=0;i<states.length;i++)
	{
//		print("state#"+i+"="+states[i]);
		if(states[i]== current_state)
		{
			quit=false;
//			print("quit1");
		}
		
		if(states[i]== new_state)
		{
			quit2=false;
//			print("quit2");
		}

	}


//	print("quit="+quit+"quit2"+quit2);
	
	if(quit==true||quit2==true)
	{return;}
//	print("continue");
	var temp:String;
	temp=current_state.ToString()+"_end";;
	entity.SendMessage(temp);
	temp=new_state.ToString()+"_begin";
	entity.SendMessage(temp);
//	print("begin="+entity.GetComponent(AI).currentState+"newstate"+new_state);
	entity.GetComponent(AI).currentState=new_state;
//	print("answer="+entity.GetComponent(AI).currentState+"newstate"+new_state);
	current_state=new_state;
//	print("current_State="+current_state);
}

}




public class Priorities
{
public var hunger:float;//determines when to eat
public var exercise:float;//for hunting/swiming/climbing/building
public var habitat:float;//pushes lamina closer to player's hut
public var entertainment:float;//causes lamina to play
public var social:float;//causes lamina to interact with other lamina
public var knowledge:float;//causes lamina to try something n

//const
private var fadeConst:float=.25;
//consts normalized to 1
private var hungerConst:float=.15;
private var exerciseConst:float=.15;
public var habitatConst:float=.15;
public var entertainmentConst:float=.15;
public var socialConst:float=.15;
public var knowledgeConst:float=.15;

public function Priorities()
{
hunger=0;
exercise=0;
habitat=0;
entertainment=0;
social=0;
knowledge=0;
}

private function Fade(distance:float):Priorities
{
	var tempPriorities:Priorities=new Priorities();
	tempPriorities.hunger=hunger/(distance*fadeConst);
	tempPriorities.exercise=exercise/(distance*fadeConst);
	return tempPriorities;
}

public function FindHappiness():float
{
	var happiness:float;
	happiness= 	hunger*hungerConst+
				exercise*exerciseConst+
				habitat*habitatConst+
				entertainment*entertainmentConst+
				social*socialConst+
				knowledge*knowledgeConst;
	 print("happy?");
	return happiness;
}

public function Add(other:Priorities)
{
	var tempPriorities:Priorities=new Priorities();
	tempPriorities.hunger=Mathf.Min(this.hunger+other.hunger,100);
	tempPriorities.exercise=Mathf.Min(this.exercise+other.exercise,100);
	tempPriorities.habitat=Mathf.Min(this.habitat+other.habitat,100);
	tempPriorities.entertainment=Mathf.Min(this.entertainment+other.entertainment,100);
	tempPriorities.social=Mathf.Min(this.social+other.social,100);
	tempPriorities.knowledge=Mathf.Min(this.knowledge+other.knowledge,100);;
	 print("add?");
	return tempPriorities;
}

public function FindNewHappiness(distance:float,tempPriorities:Priorities):float
{
	 print("find new happiness begins");
	 print("temppriorities= " +tempPriorities);
	 print("this priorities= "+this);
	var thesePriorities:Priorities=new Priorities();//create a 0'd out 	thesePriorities=Add(tempPriorities);//add these priorities to 
//	thesePriorities=Fade(distance);//fade these priorities
	thesePriorities=Add(tempPriorities);//add these priorities to another
	 print("new priorities= "+thesePriorities);
	return thesePriorities.FindHappiness();
	
}

public function Decrement(time:float)
{
var decrementation:float= .0001/time;
hunger= Mathf.Max(hunger-decrementation,0);
exercise= Mathf.Max(exercise-decrementation,0);
habitat= Mathf.Max(habitat-decrementation,0);
entertainment= Mathf.Max(entertainment-decrementation,0);
social= Mathf.Max(social-decrementation,0);
knowledge= Mathf.Max(knowledge-decrementation,0);

}

}


public class Stats
{
public var health:float;
public var maxHealth:float;
public var attack:float;
public var defense:float;
public var speed:float;
public var stamina:float;
public var luck:float;
public var age:float;

public function Stats()
{
health=0;
maxHealth=0;
attack=0;
defense=0;
speed=0;
stamina=0;
luck=0;
age=0;
}

}

