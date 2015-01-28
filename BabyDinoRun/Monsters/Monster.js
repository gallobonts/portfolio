enum AIState{ATTACK,FLEE,IGNORE};
var PowerUp:GameObject[];

public class Monster extends MonoBehaviour
{
var f_Health:float;
var f_Speed:float;
var f_Damage:float;
var i_Score:int;
var f_Experience:float;
var o_Player:Transform;
var f_BottomCut:float;
var i_DropChance:int;
var ai:AIState;

public function Initialize(f_newHealth:float,f_newSpeed:float,f_newDamage:float,i_newScore:int,f_newExperience:float,i_newDropChance:int)
{
var GodRef : GodSingleton = GodSingleton.GetInstance();
var f_DifficultyBoost=1+ .2*GodRef.i_GameLevel;
f_Health=f_newHealth*f_DifficultyBoost;
f_Speed=f_newSpeed;
f_Damage=f_newDamage*f_DifficultyBoost;
i_Score=i_newScore*f_DifficultyBoost;
f_Experience=f_newExperience*f_DifficultyBoost;
i_DropChance=i_newDropChance+GodRef.i_Luck;

o_Player=GameObject.Find("dragoncube").transform;
ai=AIState.ATTACK;
f_BottomCut=150;

}

public function AIChange(newAI:AIState)
{
ai=newAI;
}

public function Run()
{
if(renderer.isVisible)
{
switch(ai)
{
case AIState.ATTACK: Attack();
	break;
case AIState.IGNORE: Ignore();
	break;

}//end switch
}//end if visible

else if(this.transform.position.y<o_Player.position.y-f_BottomCut)
{Destroy(this.gameObject);}

if(f_Health<=0)
{Death();}



}       
public function FacePlayer()
{
if (o_Player) // we get sure the target is here
{
	Debug.Log(o_Player.position);
	var dir = (o_Player.position-this.transform.position);
	transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(dir),Time.deltaTime * 1);
 	transform.eulerAngles = Vector3(0, 0, transform.eulerAngles.z);
	
}
}
public function Attack()
{

}

public function Death()
{
var GodRef : GodSingleton = GodSingleton.GetInstance();
GodRef.i_Score+=i_Score;
ReleaseReward();
GodRef.f_Experience+=f_Experience;
Destroy(this.gameObject);
}

public function Ignore()
{

}

public function ReleaseReward()
{
var isReward:boolean=false;
var r:int= Random.Range(0,100);
if(r<=i_DropChance)
{isReward=true;}

if(isReward)
{
var reward:int= Random.Range(0,PowerUp.length);
if (reward<PowerUp.length)
{Instantiate(PowerUp[reward],this.transform.position,this.transform.rotation);}
}

}

public function DealDamage():float
{
	return f_Damage;
}

public function TakeDamage(f_newDamage:float)
{
	f_Health-=f_newDamage;
}

}//end class




