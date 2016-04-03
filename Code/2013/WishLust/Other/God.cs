using UnityEngine;
using System.Collections;
//save & load libraries
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public enum GameState
{
	Adventure,
	Merchant,
	Menu
};



public class God : MonoBehaviour {
	
	public bool midGame=true;
	public static God god;
	public PlayerData data;
	public GameState gamestate;
	public bool GodMode=false;

	//formatter used to save file
	BinaryFormatter bf =new BinaryFormatter();
	//open file stream
	FileStream file;

	// Use this for initialization
	void Awake () 
	{

		if(god==null)
		{
			god=this;
			AvalaibleBluePrints.SetUp();
			DontDestroyOnLoad(this);

		}
		else if(god!=this)
		{
			Destroy (gameObject);
		}

	}


	public void SetGameState(GameState newState)
	{
	/*	switch(newState)
		{
	
		}
	*/	gamestate=newState;
	}



	public void SaveGame()//won't work on web
	{
		file=File.Create(Application.persistentDataPath + "/playerInfo.dat");

		//save file
		bf.Serialize(file,data);
		//close file
		file.Close();
	}
	public void LoadGame()
	{
		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
		 	file= File.Open(Application.persistentDataPath + "/playerInfo.dat",FileMode.Open);
			
			//load file
			data= (PlayerData) bf.Deserialize(file);
			//close file
			file.Close();
		}

	}



	public void NewGame(int playerCount)
	{
		midGame=false;
		data.sceneName="adventure0";
        data.playerCount = playerCount;

		data.maxHealth=10;
		data.resourceHash= new Hashtable();
		data.resourceHash.Clear();
		data.weaponInventory=new List<string>();//holds all your weapons
		data.toolInventory= new List<string>(); //holds all your tools
		data.bluePrintInventory=new List<BluePrints>();//holds all your blue prints
		data.genieInventory= new List<Genie>(); // holds all your genies

		for(int i=0; i<4; i++)
		{
			data.myWeapons[i]= "Fist";
		}
		data.toolIndex=0;
		data.myGenie=null;

		Application.LoadLevel(data.sceneName);
	}

}

//Data container 
[Serializable]
public class PlayerData
{
	public string sceneName;
	public float maxHealth;

    public int playerCount;
	//inventory stuff
	public Hashtable resourceHash=new Hashtable();//holds all of your resources

	public List<string> weaponInventory=new List<string>();//holds all your weapons
	public List<string> toolInventory= new List<string>(); //holds all your tools
	public List<BluePrints> bluePrintInventory=new List<BluePrints>();//holds all your blue prints
	public List<Genie> genieInventory= new List<Genie>(); // holds all your genies
	
	public string[] myWeapons= new string[4];//equiped weapons
	public Genie myGenie=null;
	public int toolIndex=0;

	public List<KeyItems> myKeyItems= new List<KeyItems>();
}
