using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ResourceCount
{
	public RESOURCE_NAMES resourceName;
	public int resourceCount;
}


public class Controls : MonoBehaviour {
	//stats
	public float speed=1f;
	public float maxHealth=10f;
	public float health;
	private Vector2 dirFacing;
	private Transform myTransform;

	//hudsgod
	HUD_equipment hud_equipment;
	private AdventureMenuGUI myAdventureMenuGUI;

	//inventory stuff
	public Hashtable resourceHash=new Hashtable();//holds all of your resources
	public List<string> weaponInventory=new List<string>();//holds all your weapons
	public List<string> toolInventory= new List<string>(); //holds all your tools
	int toolIndex=0;
	public List<BluePrints> bluePrintInventory=new List<BluePrints>();//holds all your blue prints
	public List<Genie> genieInventory= new List<Genie>(); // holds all your genies
	public List<KeyItems> myKeyItems= new List<KeyItems>();

	public GameObject[] myWeapons= new GameObject[4];//equiped weapons
	public Genie myGenie;
	public GameObject myTool=null;

	//delay timers
	private Timer startTimer= new Timer(.5f,true);

	public bool canAttack=true;

	//handles

	public Transform equipmentPlaceHolder;



	void Start () 
	{
		myTransform=this.transform;

		weaponInventory.Add("Fist");
	
		if(God.god.GodMode)
		{
			bluePrintInventory= AvalaibleBluePrints.available;
			toolInventory.Add("Axe");
			
			//        type              name         array casting         function that gets all of the enums
			foreach (RESOURCE_NAMES resource in (RESOURCE_NAMES[]) RESOURCE_NAMES.GetValues(typeof(RESOURCE_NAMES)))
			{
				resourceHash.Add(resource,99);
			}
			


		}
		else if(!God.god.midGame)
		{
			EnterAdventureMode();
		}
		else
		{
		//resourcetesting
			resourceHash.Add(RESOURCE_NAMES.wood,10);
			resourceHash.Add(RESOURCE_NAMES.scoropionTail,10);
			toolInventory.Add("Axe");
			toolInventory.Add("PickAxe");
			weaponInventory.Add("Bow");
			bluePrintInventory.Add(AvalaibleBluePrints.available[(int)BLUEPRINT_NAMES.Whip]);
			myKeyItems.Add(KeyItems.poop);

			for(int i=0; i<4; i++)
			{
				GameObject newEquipment=(GameObject)Resources.Load("Weapons/Fist");
				myWeapons[i]= (GameObject)Instantiate(newEquipment,myTransform.position,myTransform.rotation);
				myWeapons[i].transform.parent= equipmentPlaceHolder;
			}

			Destroy(myWeapons[0]);
			GameObject freeWhip=(GameObject)Resources.Load("Weapons/Whip");
			myWeapons[0]= (GameObject)Instantiate(freeWhip,myTransform.position,myTransform.rotation);
			myWeapons[0].transform.parent= equipmentPlaceHolder;

			Destroy(myWeapons[1]);
			GameObject freeBow=(GameObject)Resources.Load("Weapons/Bow");
			myWeapons[1]= (GameObject)Instantiate(freeBow,myTransform.position,myTransform.rotation);
			myWeapons[1].transform.parent= equipmentPlaceHolder;

			Destroy(myWeapons[2]);
			GameObject freeSword=(GameObject)Resources.Load("Weapons/Sword");
			myWeapons[2]= (GameObject)Instantiate(freeSword,myTransform.position,myTransform.rotation);
			myWeapons[2].transform.parent= equipmentPlaceHolder;

			maxHealth=10;
			myTool=null;
		}
		hud_equipment= (HUD_equipment) transform.GetComponent(typeof(HUD_equipment));

		myAdventureMenuGUI= (AdventureMenuGUI) transform.GetComponent(typeof(AdventureMenuGUI));


	
		health=maxHealth;
		dirFacing= new Vector2(0,1);
	

		hud_equipment.enabled=true;
		hud_equipment.UpdateTools();
		hud_equipment.UpdateWeapons();
	}

	public void ResetCanAttack()
	{
		for(int i=0; i<4; i++)
		{
			myWeapons[i].SetActive(false);
			
		}
		canAttack=true;

	}

	void Update()
	{

		if(Input.GetButtonDown("Start"))
			{
			myAdventureMenuGUI.SetPlayGame(!myAdventureMenuGUI.playGame);

			ResetCanAttack();

			}
			//if you can attack, and click attack, call the correct attack script
			if(canAttack&& myAdventureMenuGUI.playGame)
			{
				if(Input.GetButtonDown("X"))
				{
					myWeapons[0].SetActive(true);
					myWeapons[0].SendMessage("Attack",dirFacing);
						canAttack=false;
				}
				if(Input.GetButtonDown("Circle"))
				{
					myWeapons[1].SetActive(true);
					myWeapons[1].SendMessage("Attack",dirFacing);
					canAttack=false;
				}
				
				if(Input.GetButtonDown("Triangle"))
				{
					myWeapons[2].SetActive(true);
					myWeapons[2].SendMessage("Attack",dirFacing);
					canAttack=false;
				}
				
				if(Input.GetButtonDown("Square"))
				{
					myWeapons[3].SetActive(true);
					myWeapons[3].SendMessage("Attack",dirFacing);
					canAttack=false;
				}

				if(Input.GetButtonDown("R2")&& myTool!=null)//use tool
				{
					myTool.SetActive(true);
					myTool.SendMessage("Use",dirFacing);
					canAttack=false;
				}
				if(Input.GetButtonDown("L1")&&toolInventory.Count>0)//switch tool left
			   	{
				toolIndex--;
				if(toolIndex<0)
				{
					toolIndex=toolInventory.Count-1;
				}
				if(myTool)
				{Destroy(myTool);}

				GameObject newTool=(GameObject)Resources.Load("Tools/"+toolInventory[toolIndex]);
				myTool= (GameObject)Instantiate(newTool,myTransform.position,myTransform.rotation);
				myTool.transform.parent= equipmentPlaceHolder;
				hud_equipment.UpdateTools();

				}
				if(Input.GetButtonDown("R1")&&toolInventory.Count>0)//switch tool right
				{
				toolIndex++;
				if(toolIndex>toolInventory.Count-1)
				{
					toolIndex=0;
				}
				if(myTool)
				{Destroy(myTool);}
				
				GameObject newTool=(GameObject)Resources.Load("Tools/"+toolInventory[toolIndex]);
				myTool= (GameObject)Instantiate(newTool,myTransform.position,myTransform.rotation);
				myTool.transform.parent= equipmentPlaceHolder;
				hud_equipment.UpdateTools();
				}
			}//end if can attack



	}//end update

	public void DoneAttacking()
	{
		canAttack=true;
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		Vector2 newVelocity;
		newVelocity.y= Input.GetAxis ("L_Vertical")*speed;
		if(Input.GetKey("up"))
		{
			newVelocity.y=speed;
		}
		else if(Input.GetKey("down"))
		{
			newVelocity.y=-speed;
		}

		newVelocity.x=Input.GetAxis ("L_Horizontal")* speed;

		if(Input.GetKey("left"))
		{
			newVelocity.x=-speed;
		}
		else if(Input.GetKey("right"))
		{
			newVelocity.x=speed;
		}

		rigidbody2D.velocity=newVelocity;
		if(newVelocity!= new Vector2(0,0))
			{dirFacing=newVelocity.normalized;}

	}

	void EnterAdventureMode()
	{
		bluePrintInventory=God.god.data.bluePrintInventory;
		genieInventory=God.god.data.genieInventory;
		maxHealth=God.god.data.maxHealth;
		toolInventory= God.god.data.toolInventory;
		toolIndex=God.god.data.toolIndex;
		myKeyItems=God.god.data.myKeyItems;

		if(toolInventory.Count>0)
		{

			GameObject newTool=(GameObject)Resources.Load("Tools/"+toolInventory[toolIndex]);
			myTool= (GameObject)Instantiate(newTool,myTransform.position,myTransform.rotation);
			myTool.transform.parent= equipmentPlaceHolder;
		}


		for(int i=0; i<4; i++)
		{
			GameObject newEquipment=(GameObject)Resources.Load("Weapons/"+God.god.data.myWeapons[i]);
			myWeapons[i]= (GameObject)Instantiate(newEquipment,myTransform.position,myTransform.rotation);
			myWeapons[i].transform.parent= equipmentPlaceHolder;
		}

		resourceHash=God.god.data.resourceHash;

		weaponInventory=God.god.data.weaponInventory;
	}
	public void EnterMerchantMode()
	{
		God.god.midGame=false;
		God.god.data.bluePrintInventory=bluePrintInventory;
		God.god.data.genieInventory=genieInventory;
		God.god.data.maxHealth=maxHealth;
		God.god.data.toolIndex=toolIndex;
		God.god.data.myKeyItems=myKeyItems;
		for(int i=0;i<4; i++)
		{
			string weaponName=myWeapons[i].name;
			weaponName=weaponName.Remove(weaponName.Length-7);
			God.god.data.myWeapons[i]=weaponName;
			                                         
		}
		God.god.data.resourceHash=resourceHash;
		God.god.data.sceneName=Application.loadedLevelName;
		God.god.data.toolInventory=toolInventory;
		God.god.data.weaponInventory=weaponInventory;

		Application.LoadLevel("merchantMode");
	}
	public void AddKeyItem(KeyItems item)
	{
		myKeyItems.Add (item);

	}
	public void AddResource(ResourceCount newResourceCount)
	{
		RESOURCE_NAMES name=newResourceCount.resourceName;
		int add= newResourceCount.resourceCount;

		int oldValue=0;
	
		if(resourceHash.ContainsKey(name))
		   {
			resourceHash[name] = (int)resourceHash[name]+ add;
			oldValue=(int)resourceHash[name];
			}
		else
		{
			resourceHash[name] = add;
			oldValue=add;
		}

	}

	public void AddBluePrint(BluePrints bluePrint)
	{
		bluePrintInventory.Add(bluePrint);
		Debug.Log(bluePrint);
	}

	public void AddWeapon(string wep)
	{
		weaponInventory.Add(wep);
	}

	public void EquipWeapon(string wepName,int wepNum)
	{
		if(wepNum<0)
		{wepNum=0;}
		else if(wepNum>3)
		{wepNum=3;}

		Destroy(myWeapons[wepNum]);
		GameObject newWeapon=(GameObject)Resources.Load("Weapons/"+wepName);
			myWeapons[wepNum]= (GameObject)Instantiate(newWeapon,myTransform.position,myTransform.rotation);
			myWeapons[wepNum].transform.parent= equipmentPlaceHolder;

		hud_equipment.UpdateWeapons();
	}
	public void AddGenie()
	{

	}

	public void AddTool(string tool)
	{
		toolInventory.Add(tool);
	}
	public void ChangeHealth(float change)
	{
		health+=change;
		if(health<=0)
		{Die();}
		else if(health>maxHealth)
		{
			health=maxHealth;
		}
	}

	public void Die()
	{
		Application.LoadLevel("MainMenu");
	}
}
