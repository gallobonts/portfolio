#pragma strict




var i_MonsterCount:int;
var f_LeftScreenX:float;
var f_RightScreenX:float;
var f_TopScreenY:float;
var g_Monsters:GameObject[];
var DragonOrientation:Transform;

function Start () 
{
f_TopScreenY=75;
f_LeftScreenX=-75f;
f_RightScreenX=75f;


var GodRef : GodSingleton = GodSingleton.GetInstance();
i_MonsterCount=GodRef.i_MonsterCount;


var f_Distance= GodRef.f_Distance;


for(var i:int; i<i_MonsterCount;i++)
{
	var r:int= Random.Range(0,g_Monsters.length);
	var v3_NewPosition:Vector3;
	v3_NewPosition= new Vector3(Random.Range(f_LeftScreenX,f_RightScreenX),Random.Range(f_TopScreenY,f_Distance),25);
	Instantiate(g_Monsters[r],v3_NewPosition,DragonOrientation.rotation);
	

	
}

}

function Update () 
{

}