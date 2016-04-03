#pragma strict
/*
	The player's main interface
*/

//////////////////////
//stats
var f_MaxStamina:float;
var f_Stamina:float;
var f_MaxHealth:float;
var f_Health:float;
var i_Strength:int;
var i_Style:int;
var i_Luck:int;
var i_ShieldLevel:int;

var b_canFire:boolean;
var b_isFire:boolean;

var b_canShield:boolean;
var b_isShield:boolean;
var f_ShieldDelay:float=3;
var f_ShieldDelayTimer:float=0;

//////////////////
//movement
var f_XDirection:float;
var f_YDirection:float;
var f_HighSpeed:float;
var f_LowSpeed:float;
var f_CurrentSpeed:float;
var f_Distance:float;
var f_StartDistance:float;
var f_LeftLimit:float;
var f_RightLimit:float;
///////////////////////
var b_Invincible:boolean;
var f_InvincibleTimer:float;
var f_InvincibleDelay:float;

//Hud
var v3_StartPosition:Vector3;

var DragonFirePrefab:GameObject;
var ShieldPrefab:GameObject;

function Start()
{
	var GodRef : GodSingleton = GodSingleton.GetInstance();
	if(GodRef.i_DragonLevel==0)
		{GodRef.ResetCharacter();GodRef.ResetGame();}

	 //Hud
	 v3_StartPosition=transform.position;
	  
	 //movement
	 f_HighSpeed=GodRef.f_Speed; 
	 f_LowSpeed=f_HighSpeed*.5;
	 f_CurrentSpeed=f_HighSpeed;
	 f_XDirection=0.0f; 
	 f_YDirection=0.0f;
	 f_Distance=GodRef.f_Distance;
	 f_StartDistance=f_Distance;
	 f_LeftLimit=-75;
	 f_RightLimit=75;
	 ////////////////
	 
	 //stats
	 f_MaxStamina=GodRef.f_Stamina;
	 f_Stamina=f_MaxStamina;
	 f_MaxHealth=GodRef.f_Health;
	 f_Health=f_MaxHealth;
	 i_Strength=GodRef.i_Strength;
	 i_Style=GodRef.i_Style;
	 i_Luck=GodRef.i_Luck;
	 i_ShieldLevel=GodRef.i_ShieldLevel;
	 
	 //action delays
	 b_isFire=false;
	 b_canFire=true;
	 b_isShield=false;
	 b_canShield=true;
	 f_ShieldDelayTimer=10;
	 
	 //invinciblity
	 b_Invincible=true;
	 f_InvincibleDelay=2.0f;
	 f_InvincibleTimer= f_InvincibleDelay;
}

function Update()
{
	//use fire
	if(Input.GetButton("Fire")&&b_canFire)
	{
		DragonFirePrefab.SetActive(true);
		f_CurrentSpeed=f_LowSpeed;
	}
	else
	{
		DragonFirePrefab.SetActive(false);
		f_CurrentSpeed=f_HighSpeed;
	}

	//use shield
	if(Input.GetButton("Shield")&&b_canShield)
	{
		var ShieldScript=ShieldPrefab.GetComponent(Shield);
		ShieldScript.Activate();
		b_canShield=false;
	}

	//move left & right
	if(Input.GetButton("Horizontal"))
	{
		f_XDirection= Input.GetAxis("Horizontal");
		
		transform.Translate( new Vector3(-f_XDirection*f_CurrentSpeed*Time.deltaTime,0,0));
		if(transform.position.x<f_LeftLimit||transform.position.x>f_RightLimit)
		{transform.Translate( new Vector3(f_XDirection*f_CurrentSpeed*Time.deltaTime,0,0));}
		
	}

	//move up & down
	if(Input.GetButton("Vertical"))
	{
		f_YDirection= Input.GetAxis("Vertical");
		
		transform.Translate( new Vector3(0,f_YDirection*f_CurrentSpeed*Time.deltaTime),0);
	}
	f_Distance= f_StartDistance - Vector3.Distance(transform.position,v3_StartPosition);




}//end update


function LateUpdate()
{
	var GodRef : GodSingleton = GodSingleton.GetInstance();
	f_Stamina-=Time.deltaTime;

	//determine if player has died
	if (f_Stamina <=0)
		{
		 Score();
		 GodRef.e_intermediate=IntermediateState.STAMINALOSE;
		 Application.LoadLevel("IntermediateScreen");
		}
	else if(f_Health<=0)
		{
		 Score();
		 GodRef.e_intermediate=IntermediateState.HEALTHLOSE;
		 Application.LoadLevel("IntermediateScreen");
		}

	//determine if the player has won
	if (f_Distance <=0)
		{
		GodRef.i_Score+=f_StartDistance-f_Distance;	
		Application.LoadLevel("Upgrade");
		}

	//state changes
		
	if(!b_canShield&&!b_isShield)//deactivate shield timer
	{
		f_ShieldDelayTimer-=Time.deltaTime;
		if(f_ShieldDelayTimer<=0)
		{
			b_canShield=true;
			f_ShieldDelayTimer=f_ShieldDelay;
		}
	}

	if(b_Invincible)
	{
		f_InvincibleTimer-=Time.deltaTime;
		if(f_InvincibleTimer<=0)
		{b_Invincible=false; f_InvincibleTimer=f_InvincibleDelay;}
		
	}
	
}

function Score()
{
	var GodRef : GodSingleton = GodSingleton.GetInstance();
	GodRef.i_Score+=f_StartDistance-f_Distance;
	var f_HighScores:float[]= new float[10];
	
	//retrieve current scores
	for(var i:int=0;i<10;i++)
	{
		f_HighScores[i]=PlayerPrefs.GetInt(("HighScore")+i.ToString(),0);
	}
	
	//re-arrange current scores
	for(i=0;i<10;i++)
	{
		var tempScore:int;
		if(f_HighScores[i]<GodRef.i_Score)
		{
		tempScore=f_HighScores[i];
		f_HighScores[i]=GodRef.i_Score;
		GodRef.i_Score=tempScore;
		}
	}
	
	//set up new scores
	for(i=0;i<10;i++)
	{
		PlayerPrefs.SetInt(("HighScore")+i.ToString(),f_HighScores[i]);
	}

}


function OnTriggerEnter(other: Collider)
{
	if(other.CompareTag("reward"))
	{
	var rewardscript= other.gameObject.GetComponent(Rewards);
	rewardscript.PowerUp(this);
	Destroy(other.gameObject);
	}
	else if(!b_Invincible && !b_isShield)//invulnerabilitycheck
	{
		if(other.CompareTag("monster"))
		{
			var monsterscript = other.gameObject.GetComponent(Monster);
			f_Health-=monsterscript.DealDamage();
			monsterscript.AIChange(AIState.IGNORE);
			b_Invincible=true;
		}
	
		else if(other.CompareTag("enemyprojectile"))
		{
			var projectilescript = other.gameObject.GetComponent(Projectile);
			f_Health-=projectilescript.DealDamage();
	 		b_Invincible=true;
	 		Destroy(other.gameObject);
		}
		else if(!other.CompareTag("fire"))//errorcheck
		{Debug.Log("error..unknown collision between " + this.gameObject.name + "and " +other.gameObject.name);}
	}//end involnerability check
	
	
}//end function ontriggerenter

	
function OnGUI()
{
	//Produces the main HUD
	var GodRef : GodSingleton = GodSingleton.GetInstance();
	GUI.Label(new Rect(0,0,100,50), "health= " + f_Health.ToString("F0"));
	GUI.Label(new Rect(Screen.width - 100,0,100,50), "Stamina = " + f_Stamina.ToString("F0"));
	GUI.Label(new Rect(Screen.width/2 -100,0,100,50), "Score = " + GodRef.i_Score.ToString("F0"));
	GUI.Label(new Rect(Screen.width-100,Screen.height/2,100,50), "Distance = " + f_Distance.ToString("F0"));
	if(b_canShield)
	{GUI.Label(new Rect(Screen.width/2-100,Screen.height-50,100,50), "Shield is ready to use");}

}