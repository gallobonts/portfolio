using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class AdventureMenuGUI : MonoBehaviour 
{
	public int selected=0;
	Rect menuPosition= new Rect(0,0,250,500);
	Rect helpTextposition= new Rect(260,0,250,375);
	string helpText= "poop";
	Rect backPosition= new Rect(260,375,250,125);
	string guiFocus= "Null";
	float lastTime=0;
	public bool playGame=true;

	Controls myControls;//= new Controls();
	public enum  PauseMenuState
	{
		Main,
		Equipment,
		Equipment_Weapons,
		Equipment_Tools,
		Inventory,
		Inventory_Blueprints,
		Inventory_Resources,
		Options,
		ManageData,
		Exit
	};
	public PauseMenuState pauseMenustate= PauseMenuState.Main;
	PauseMenuState lastState= PauseMenuState.Main;

	string[] mainPauseMenuButtons= new string[]
	{
		"EQUIPMENT",
		"INVENTORY",
		"OPTIONS",
		"MANAGEDATA",
	};

	string[] inventoryPauseMenuButtons= new string[]
	{
		"BluePrints",
		"Resources",
		"",
		""

	};

	string[] manageDataPauseMenuButtons= new string[]
	{
		"Save",
		"Load",
		"",
		""
	};

	List<BluePrints> bluePrints= new List<BluePrints>();
	string [] visibleButtons= new string[4];
	int buttonIndex=0;//to determine where in thelist the visible blue prints start

	string[] equipmentPauseMenuButtons= new string[]
	{
		"Weapons",
		"Tools",
		"Genies",
		""
	};

	List<string> weaponInventory= new List<string>();
	string[] equipedWeapons= new string[4];
	Hashtable resources= new Hashtable();

	List<string> toolInventory= new List<string>();

	string[] tempButtons= new string[]
	{
		"","","",""
	};

	string[] pauseButtons;

	public void Start()
	{
		pauseButtons=mainPauseMenuButtons;
		myControls= (Controls) transform.GetComponent(typeof(Controls));
	}
	Timer analogTimer= new Timer(.25f,true);

	public void Update()
	{
		if(playGame)
		{return;}
		float deltaTime= Time.realtimeSinceStartup-lastTime;
		lastTime= Time.realtimeSinceStartup;

		analogTimer.Update(deltaTime);
		int direction=0;
		float verticalInput =Input.GetAxisRaw ("L_Vertical");
		if(verticalInput>0)
		{
			direction=-1;
		}
		else if(verticalInput<0)
		{
			direction=1;
		}

		float horizontalInput =Input.GetAxisRaw ("L_Vertical");
		if(horizontalInput<-.50||Input.GetKeyDown("left"))
		{
			selected=0;

			guiFocus="Null";
		}
		else if(horizontalInput>.50f||Input.GetKeyDown("right"))
		{
			selected=-1;
			guiFocus="backText";
		}

		if(Input.GetKeyDown("down"))
			direction=1;
		else if(Input.GetKeyDown("up"))
			direction=-1;

		if(direction!=0&&selected!=-1)
		{

			//the rotating menues
			if(pauseMenustate==PauseMenuState.Inventory_Blueprints
			   ||pauseMenustate==PauseMenuState.Equipment_Weapons)
			{
				if(analogTimer.CheckDone())
				{
					selected+=direction;
				
					if(selected<0)//if you go up
					{
						if(pauseMenustate==PauseMenuState.Inventory_Blueprints)
						{ 
							if(buttonIndex==0 && bluePrints.Count>pauseButtons.Length)//top of the list
							{
								selected=pauseButtons.Length-1;

							//blue print rotating menu
								buttonIndex=bluePrints.Count-pauseButtons.Length;
				
								for(int i=0; i<bluePrints.Count && i<4; i++)
								{
									visibleButtons[i]=bluePrints[buttonIndex+i].name.ToString();
								}
							
							}
							else //not top of the list
							{
				
								buttonIndex--;
								selected++;
								for(int i=0; i<bluePrints.Count && i<4; i++)
								{
									visibleButtons[i]=bluePrints[buttonIndex+i].name.ToString();
								}
							}

						}
							//weapon rotating menu
						else if(pauseMenustate==PauseMenuState.Equipment_Weapons)
						{

							if(buttonIndex==0&&weaponInventory.Count>pauseButtons.Length)//top of the list
							{
								selected=pauseButtons.Length-1;
								

								buttonIndex=weaponInventory.Count-pauseButtons.Length;
								for(int i=0; i<weaponInventory.Count && i<4; i++)
								{
									visibleButtons[i]=weaponInventory[buttonIndex+i];
								}

							}
							else //not top
							{	
								buttonIndex--;
								selected++;
								for(int i=0; i<weaponInventory.Count && i<4; i++)
								{
									visibleButtons[i]=weaponInventory[buttonIndex+i];
								}
							}
						}
					}

					else if(selected>pauseButtons.Length-1)//go down
					{
						Debug.Log("go down");
						if(pauseMenustate==PauseMenuState.Inventory_Blueprints)
						{
							if(buttonIndex+pauseButtons.Length > bluePrints.Count-1)
							{
								selected=0;
								buttonIndex=0;
								for(int i=0; i<bluePrints.Count && i<4; i++)
								{
									visibleButtons[i]=bluePrints[i].name.ToString();
								}
							}
							else
							{
								buttonIndex++;
								selected--;
								for(int i=0; i<bluePrints.Count && i<4; i++)
								{
									visibleButtons[i]=bluePrints[buttonIndex+i].name.ToString();
								}
							}
						}

						else if(pauseMenustate==PauseMenuState.Equipment_Weapons)
						{
							if(buttonIndex+weaponInventory.Count > weaponInventory.Count-1)
							{
								selected=0;
								buttonIndex=0;
								for(int i=0; i<weaponInventory.Count && i<4; i++)
								{
									visibleButtons[i]=weaponInventory[i];
								}
							}
							else
							{

								buttonIndex++;
								selected--;
								for(int i=0; i<weaponInventory.Count && i<4; i++)
								{
									visibleButtons[i]=weaponInventory[buttonIndex+i];
								}
							}
						}
					}//	else if(selected>pauseButtons.Length-1)


					helpText="";
					if(pauseMenustate==PauseMenuState.Inventory_Blueprints)
					{
					
						Hashtable resourceHash;
						if(bluePrints.Count>0)
						{
							Debug.Log ("selected"+selected+"buttonindex"+buttonIndex+"count"+bluePrints.Count);
							if(selected+buttonIndex < bluePrints.Count)
							{
								resourceHash= bluePrints[selected+buttonIndex].required_resources;
								foreach(RESOURCE_NAMES key in resourceHash.Keys)
								{
										
									helpText+= key.ToString()+"\n ";
									if(resources.ContainsKey(key))
									{
										helpText+= resources[key].ToString();
									}
									else
									{
										helpText+= "0";
									}
									helpText+=" / ";
									helpText+=  resourceHash[key].ToString()+"\n";
								}
							}
	
						}
					}//if blueprints
					else if(pauseMenustate==PauseMenuState.Equipment_Weapons)
					{
						GameObject[] myWeapons= myControls.myWeapons;
						for(int i=0; i<myWeapons.Length; i++)
						{
							equipedWeapons[i]=myWeapons[i].name;
							equipedWeapons[i]=equipedWeapons[i].Remove(equipedWeapons[i].Length-7);//remove clone from name
							helpText+=equipedWeapons[i] +" " + i +"\n";
						}


					}
				}
			}

			else//not rotating menu
			{
				if(analogTimer.CheckDone())
				{
					selected+=direction;
					if(selected<0)
					{selected=pauseButtons.Length-1;}
					else if(selected>pauseButtons.Length-1)
					{
						selected=0;
					}
				}
			}//not inventory->blueprints

		}

		if(pauseMenustate==PauseMenuState.Equipment_Weapons &&//if in the equip menu and a button is pressed
		   weaponInventory.Count>selected &&
		   selected>=0 &&
		    (
			Input.GetButtonDown("X")||
			Input.GetButtonDown("Circle")||
			Input.GetButtonDown("Triangle")||
			Input.GetButtonDown("Square")
			)
		  )
	{

			int weaponKey= buttonIndex+ selected;
			int wepNum=0;
	
			if(Input.GetButton("X"))
			{wepNum=0;}
			else if(Input.GetButtonDown("Circle"))
			{wepNum=1;}
			else if(Input.GetButtonDown("Triangle"))
			{wepNum=2;}
			else if(Input.GetButtonDown("Square"))
			{wepNum=3;}

			myControls.EquipWeapon(weaponInventory[weaponKey],wepNum);
				
			weaponInventory=myControls.weaponInventory;
			GameObject[] myWeapons= myControls.myWeapons;
			helpText="";
			for(int i=0; i<myWeapons.Length; i++)
			{
				equipedWeapons[i]=myWeapons[i].name;
				equipedWeapons[i]=equipedWeapons[i].Remove(equipedWeapons[i].Length-7);//remove clone from name
				helpText+=equipedWeapons[i] +" " + i +"\n";
			}
			
			for(int i=0; i<weaponInventory.Count && i<4; i++)
			{
				visibleButtons[i]=weaponInventory[i];
			}

				
		}//end if in the equip menu and a button is pressed



		if(Input.GetButtonDown("X")||Input.GetKeyDown("space"))
		{
			if(selected==-1)
			{
				if(pauseMenustate==PauseMenuState.Main)
				{
					SetPlayGame(true);
					myControls.ResetCanAttack();
				}
				else
				{
					pauseMenustate= lastState;
					selected=0;
				}
			}
			else
			{
				switch(pauseMenustate)
				{
				case PauseMenuState.Main:
					switch(selected)
				{
				case 0:
					pauseMenustate=PauseMenuState.Equipment;
					break;
				case 1:
					pauseMenustate=PauseMenuState.Inventory;
					break;
				case 2:
					pauseMenustate=PauseMenuState.Options;
					break;
				case 3:
					pauseMenustate=PauseMenuState.ManageData;
					break;
				case 4:
					pauseMenustate=PauseMenuState.Main;
					break;
				}
					break;

				//equipment menu
				case PauseMenuState.Equipment:
					switch(selected)
				{
				case 0://equipment->weapons button
						pauseMenustate=PauseMenuState.Equipment_Weapons;
						weaponInventory=myControls.weaponInventory;
						GameObject[] myWeapons= myControls.myWeapons;
						helpText="";
						for(int i=0; i<myWeapons.Length; i++)
						{
							equipedWeapons[i]=myWeapons[i].name;
							equipedWeapons[i]=equipedWeapons[i].Remove(equipedWeapons[i].Length-7);//remove clone from name
							helpText+=equipedWeapons[i] +" " + i +"\n";
						}

				
						for(int i=0;  i<pauseButtons.Length; i++)
						{
							if(weaponInventory.Count>i)
							{visibleButtons[i]=weaponInventory[i];}
							else
							{visibleButtons[i]="";}
						}
					


						break;
					case 1://equipment->tools button
						pauseMenustate=PauseMenuState.Equipment_Tools;
						helpText="";
						toolInventory=myControls.toolInventory;
						for(int i=0; i<toolInventory.Count; i++)
						{
							helpText+= toolInventory[i] + "\n";
						}

						break;
				}//end switch selected
					break;

	
				
				//inventory menu
				case PauseMenuState.Inventory:

					switch(selected)
					{
						case 0://inventory->blueprints button
						bluePrints= myControls.bluePrintInventory;
						resources= myControls.resourceHash;

						for(int i=0; i< pauseButtons.Length; i++)
						{
							if(i<bluePrints.Count)
							{visibleButtons[i]=bluePrints[i].name.ToString();}
							else
							{visibleButtons[i]="";}
						}

					if(bluePrints.Count>0)
						{
							helpText= "";
							Hashtable resourceHash;
							resourceHash= bluePrints[0].required_resources;
							foreach(RESOURCE_NAMES key in resourceHash.Keys)
							{
								helpText+= key.ToString()+"\n ";
								if(resources.ContainsKey(key))
								{
									helpText+= resources[key].ToString();
								}
								else
								{
									helpText+= "0";
								}
								helpText+=" / ";
								helpText+=  resourceHash[key].ToString()+"\n";
							}
						}

						pauseMenustate=PauseMenuState.Inventory_Blueprints;
						break;

					case 1: //inventory->Resources
						resources= myControls.resourceHash;
						pauseMenustate=PauseMenuState.Inventory_Resources;
						helpText="";
						foreach(RESOURCE_NAMES key in resources.Keys)
						{
							helpText+= key.ToString();
							helpText+= "\n "+ resources[key].ToString()+"\n";
						}

						break;

					}
					break;
				
				//inventory->blueprints menu
				case PauseMenuState.Inventory_Blueprints:

					if(! (bluePrints.Count>selected))
					{
						break;
					}
					int bluePrintKey= buttonIndex+ selected;
					Hashtable requiredResources= bluePrints[bluePrintKey].required_resources;

					Hashtable ownedResources=resources;
					bool use=true;

					foreach(RESOURCE_NAMES key in requiredResources.Keys)//determine if you have enough resources to use the blueprint
					{
						if(!use)//if it's already been determined you can't use it stop the loop short
						{break;}

						if(ownedResources.ContainsKey(key))//if you have at least 1 resource continue, else exit
						{
							if( (int)ownedResources[key] >= (int)requiredResources[key])//if you have enough resources, use them, else exit
							{
								ownedResources[key]= (int)ownedResources[key]-(int)requiredResources[key];
							}
							else
							{
								use=false;
							}
						}
						else
						{
							use=false;
						}

					}//end of foreachloop

					if(use)
					{
						myControls.resourceHash=ownedResources;
						resources=ownedResources;

						switch(bluePrints[bluePrintKey].type)
						{
						case BLUEPRINT_TYPE.WEAPON:
							myControls.AddWeapon(bluePrints[bluePrintKey].name.ToString());
							break;
						case BLUEPRINT_TYPE.TOOLS:
							myControls.AddTool(bluePrints[bluePrintKey].name.ToString());
							break;
						}

						//set up the help text again
						helpText= "";
						Hashtable resourceHash;
						resourceHash= bluePrints[bluePrintKey].required_resources;

						foreach(RESOURCE_NAMES key in resourceHash.Keys)
						{
							helpText+= key.ToString()+"\n ";
							if(resources.ContainsKey(key))
							{
								helpText+= resources[key].ToString();
							}
							else
							{
								helpText+= "0";
							}
							helpText+=" / ";
							helpText+=  resourceHash[key].ToString()+"\n";
						}


					}

					break;
				
				case PauseMenuState.Inventory_Resources:
					break;

				case PauseMenuState.ManageData:
				break;
				
				case PauseMenuState.Options:
				break;

				
			}
		}
		}//if get button


	}//end update


	public void SetPlayGame(bool newPlayGame)
	{
		playGame=newPlayGame;
		if(playGame)
		{
			Time.timeScale=1;
			selected=0;
			pauseMenustate=PauseMenuState.Main;
			
		}
		else
		{
			Time.timeScale=0;
		}
	}

	public void OnGUI()
	{
		if(playGame)
		{return;}
		string backText="Back";
		switch(pauseMenustate)
		{
		case PauseMenuState.Main:
			pauseButtons=mainPauseMenuButtons;
			backText="Exit";
			break;
		case PauseMenuState.Inventory:
			lastState=PauseMenuState.Main;
			pauseButtons=inventoryPauseMenuButtons;
			break;
		case PauseMenuState.ManageData:
			lastState=PauseMenuState.Main;
			pauseButtons=manageDataPauseMenuButtons;
			break;
		case PauseMenuState.Options:
			lastState=PauseMenuState.Main;
			pauseButtons=tempButtons;
			break;
		case PauseMenuState.Equipment:
			lastState=PauseMenuState.Main;
			pauseButtons=equipmentPauseMenuButtons;
			break;
		case PauseMenuState.Equipment_Weapons:
			lastState=PauseMenuState.Equipment;
			pauseButtons=visibleButtons;
			break;
		case PauseMenuState.Equipment_Tools:
			lastState=PauseMenuState.Equipment;
			pauseButtons=tempButtons;
			break;
		case PauseMenuState.Inventory_Blueprints:
			lastState=PauseMenuState.Inventory;
			pauseButtons=visibleButtons;
			break;
		case PauseMenuState.Inventory_Resources:
			lastState=PauseMenuState.Inventory;
			pauseButtons=tempButtons;
			break;

		}

		selected= GUI.SelectionGrid(menuPosition,selected,pauseButtons,1); 
		GUI.TextArea(helpTextposition,helpText);

		GUI.SetNextControlName("backText");
		GUI.Button(backPosition,backText);
		GUI.FocusControl(guiFocus);

	}
		                            
}

