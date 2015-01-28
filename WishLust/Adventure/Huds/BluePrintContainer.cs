using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public enum BLUEPRINT_NAMES
{
	//weapons
	Sword,
	Bow,
	Whip,

	//tools
	Axe,
	PickAxe,

	//ships

	length
};

//[System.Serializable]
public enum BLUEPRINT_TYPE
{
	WEAPON,
	TOOLS,
	SHIP
}
[System.Serializable]
public class BluePrints
{
	public BLUEPRINT_NAMES name;
	public BLUEPRINT_TYPE type;
	public Hashtable required_resources;//=new Hashtable();

}


static public class AvalaibleBluePrints
{
	static public List<BluePrints> available= new List<BluePrints>();


	static public void SetUp()
	{
		//weapons
		BluePrints swordBluePrint= new BluePrints();
		swordBluePrint.type=BLUEPRINT_TYPE.WEAPON;
		swordBluePrint.name=BLUEPRINT_NAMES.Sword;
		swordBluePrint.required_resources=new Hashtable();
		swordBluePrint.required_resources.Add(RESOURCE_NAMES.scoropionTail,1);
		available.Add(swordBluePrint);

		BluePrints BowBluePrint= new BluePrints();
		BowBluePrint.type=BLUEPRINT_TYPE.WEAPON;
		BowBluePrint.name=BLUEPRINT_NAMES.Bow;
		BowBluePrint.required_resources=new Hashtable();
		BowBluePrint.required_resources.Add(RESOURCE_NAMES.scoropionTail,1);
		available.Add(BowBluePrint);
		
		BluePrints whipBluePrint= new BluePrints();
		whipBluePrint.type=BLUEPRINT_TYPE.WEAPON;
		whipBluePrint.name=BLUEPRINT_NAMES.Whip;
		whipBluePrint.required_resources=new Hashtable();
		whipBluePrint.required_resources.Add(RESOURCE_NAMES.scoropionTail,1);
		available.Add(whipBluePrint);


		//tools
		BluePrints axeBluePrint= new BluePrints();
		axeBluePrint.type= BLUEPRINT_TYPE.TOOLS;
		axeBluePrint.name=BLUEPRINT_NAMES.Axe;
		axeBluePrint.required_resources=new Hashtable();
		axeBluePrint.required_resources.Add(RESOURCE_NAMES.wood,2);
		axeBluePrint.required_resources.Add(RESOURCE_NAMES.scoropionTail,1);
		available.Add(axeBluePrint);

		BluePrints pickAxeBluePrint= new BluePrints();
		pickAxeBluePrint.type= BLUEPRINT_TYPE.TOOLS;
		pickAxeBluePrint.name=BLUEPRINT_NAMES.PickAxe;
		pickAxeBluePrint.required_resources=new Hashtable();
		pickAxeBluePrint.required_resources.Add(RESOURCE_NAMES.wood,2);
		pickAxeBluePrint.required_resources.Add(RESOURCE_NAMES.scoropionTail,1);
		available.Add(pickAxeBluePrint);



	}
}

public class BluePrintContainer : MonoBehaviour {
	public BLUEPRINT_NAMES myBluePrintName;
	private BluePrints myBluePrint;

	public void Start()
	{

		myBluePrint=AvalaibleBluePrints.available[(int)myBluePrintName];
	/*
		for(int i=0; i<AvalaibleBluePrints.available.Count;i++)
		{
			Debug.Log (AvalaibleBluePrints.available[i].name.ToString()+"/n");
		}
	*/

	}

	public void SetUp(BLUEPRINT_NAMES newBluePrint)
	{
			
		myBluePrint=AvalaibleBluePrints.available[(int)newBluePrint];
		myBluePrintName= newBluePrint;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag=="Player")
		{
			Controls script= (Controls) other.transform.GetComponent(typeof(Controls));
			script.AddBluePrint(myBluePrint);
			Destroy(gameObject);
		}
	}
	
	
	
}

