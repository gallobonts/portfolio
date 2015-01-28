#pragma strict

/*


//scratched attempt at singleton


class MiddleChain_EntityManager
{
	private static var instance:MiddleChain_EntityManager = new MiddleChain_EntityManager();
	var script:String;

		public static function GetInstance():MiddleChain_EntityManager
	{
		return instance;
	}

	public function start()
	{
		

	}
	
	public function update()
	{
	
	}

	private function ChangeState(entity:GameObject,state:State,front:String)
	{
	 var scriptname:Component=entity.GetComponent(front+"_AI");
	 scriptname.state=state;
	}
	
	
	}
	
	
}



*/

var player:GameObject;
var enemy:GameObject;
var timer:float;
enum GameState{START,RUN,WIN};
var game_state:GameState=GameState.RUN;
var objects_created:int=0;
var objects_destroyed:int=0;

var limit:float=1;


function Start()
{
timer=60.0;

var self=GameObject.FindGameObjectsWithTag("EntityManager");
var length=self.Length;
var destroy:Array;

if(length<1||length==null)
{
	for(var i:int=1;i<length;i++)
		{GameObject.Destroy(self[i]);
		objects_destroyed++;}
}

}


function Update () 

{


switch(game_state)
{
	case GameState.RUN:
			timer-= Time.deltaTime;
			if(timer<0)
			{game_state=GameState.WIN;}
			Create_Entity(player,1);
			limit+= Time.deltaTime/3;
			Create_Entity(enemy,limit);
			var enemies=GameObject.FindGameObjectsWithTag("enemy");
			for(var i:int=0;i<enemies.length;i++)
			{
			
			var script=enemies[i].GetComponent(MiddleChainAI);
			if(script.readyToChange==true)
			{
			switch(script.state)
			{
				case State.BIRTH:
					print("Enemy " + script.id+" is changing from " + script.state +" to wandering");
					script.Birth_End();script.readyToChange=false;script.timer=5;
					break;//end of state birth
				case State.WANDERING:
					var closestObject:int;
					var distance:float=Mathf.Infinity;
					
					for(var j:int=0;j<enemies.length;j++)//find closest object
					{
						if(i!=j)
						{
						var testDistance:float=Vector3.Distance(enemies[i].transform.position,enemies[j].transform.position);
						if(distance>testDistance)
						{distance=testDistance;closestObject=j;}
						}
						
					}
					var playerCheck:GameObject= GameObject.FindGameObjectWithTag("Player");
					if(playerCheck!=null)
					{
						testDistance=Vector3.Distance(enemies[i].transform.position,playerCheck.transform.position);
						if(distance>testDistance)
							{distance=testDistance;closestObject=-1;}
					}
					//determine relative size
					var relativeSize:int;
					if(closestObject==-1)
					{
						var playerScript=playerCheck.GetComponent(MiddleChain_Player);
						relativeSize= Mathf.Abs(playerScript.size-script.size);
						script.target=playerCheck.transform;
					}
					else
					{
						relativeSize= Mathf.Abs(script.size-enemies[closestObject].GetComponent(MiddleChainAI).size);
						script.target=enemies[closestObject].transform;
					}
					var stateChange=Fuzzification(relativeSize);
					script.readyToChange=false;
					script.timer=5;
					print("Enemy " + script.id+" is changing from " + script.state+" to " + stateChange);
					script.state=stateChange;
					break;//end of state wander
				case State.FLEEING:
					print("Enemy " + script.id+" is changing from " + script.state+" to "+ State.WANDERING);
					script.state=State.WANDERING;
					script.timer=5;
					break;
				case State.SEEKING:
					print("Enemy " + script.id+" is changing from " +script.state+" to "+ State.WANDERING);
					script.state=State.WANDERING;
					script.timer=5;
					break;
				case State.DEATH:
					print("Enemy " + script.id+" is changing from " +script.state+" to...well now its deleted");
					GameObject.Destroy(enemies[i]);
					objects_destroyed++;
					break;
			}//end state switch
			}//end check if ready for change
			}//end for loop of checking all the objects
			break;//end of gamestate run
			case GameState.WIN:
			print("Number of objects created "+ objects_created +" Number of objects deleted "+objects_destroyed+" Seconds since minigame "+Time.timeSinceLevelLoad);
			
			break;
}//end gamestate switch

}//end update


function Create_Entity(entity:GameObject,limit:int)
{
	var length=GameObject.FindGameObjectsWithTag(entity.tag).Length;
	 if(length<limit||length==null)
	 {
	 	GameObject.Instantiate(entity,Vector3(0,0,0),Quaternion(0,0,0,0));
	 	objects_created++;
	 }
}


function Fuzzification(relative_size:int):State//doesnt work???
{
	var chase_dom:float;
	var flee_dom:float;
	var wander_dom:float;
	var state:State;


	if(relative_size==0)
		{chase_dom=0; flee_dom=0;wander_dom=0;}
	else if(relative_size>0)
	{
		relative_size=Mathf.Min(10,relative_size);  //make it in range
		chase_dom=relative_size/10;  //determine chase’s degree of 	membership
		wander_dom=1-chase_dom;
		flee_dom=0;
	}
	else
	{
		relative_size=Mathf.Max(-10,relative_size);  //make it in range
		flee_dom=relative_size/-10;  
		wander_dom=1-flee_dom;
		chase_dom=0;
	}
	

	if(chase_dom>wander_dom && chase_dom>flee_dom)
	{
	state=State.SEEKING;
	}
	else if(flee_dom>wander_dom && flee_dom>chase_dom)
	{
	state=State.FLEEING;
	}
	else
	{state=State.WANDERING;}
	
	return state;
}
