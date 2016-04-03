using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using InControl;


public class LaminaBrain_Grave : Brain_Base 
{
	public HumanController_Grave playerController;
  [HideInInspector]
  public PlayerLaminaManager myPlayerLaminaManager;

 
 

  void OnEnable()
  {
    GOD_Memory.SaveNormal += Save;
    GOD_Memory.LoadNormal += Load;
    //we reset it on level change
  }

	override public void SetActive(InputDevice inputDevice,Controls newControls,int newPlayerNum)
	{	
			myControls=newControls;
			myControls.Device=inputDevice;
			playerController.SetController(myControls,newPlayerNum);
      playerController.Reset();
			//update gui?
		

	}
  public void SetPlayerLaminaManager(PlayerLaminaManager newPlayerLaminaManager)
  {
    myPlayerLaminaManager = newPlayerLaminaManager;
  }



  
  void Save()
  {
    if (myGameObject == null)
    {
      Debug.Log("I don't know why " + gameObject.name + " has a null reference to self...but it needs to be fixed");
      myGameObject = this.gameObject;
    }
    
    string savePath = GOD_Memory.rootFolder + "laminaBrain";
    
    GOD_Memory.instance.CheckPath(savePath);
    
    savePath+="?tag=" + myGameObject.name;
    
    
    //save base stats
    ES2.Save<float>(baseStats.maxHealth, savePath    + "maxhealth");
    ES2.Save<float>(baseStats.health, savePath +"health");
    ES2.Save<int>(baseStats.strength, savePath +"strength");
    ES2.Save<int>(baseStats.speed, savePath +"speed");
    
    //save level info
    ES2.Save<int>(myLevelInfo.level, savePath +"level");
    ES2.Save<int>(myLevelInfo.experience, savePath +"experience");
    ES2.Save<int>(myLevelInfo.experienceTilNxtLvl, savePath +"experienceTilNxtLvl");
    
    
    
    
  }
  
  void Load()
  {
    if(GOD.myGOD.isNewGame){return;}//if it's a new game, leave the defaults
    
    
    string loadPath = GOD_Memory.rootFolder + "laminaBrain"+"?tag=" + myGameObject.name;
    if (!ES2.Exists(loadPath)){return;}
    
    
    baseStats.maxHealth = ES2.Load<float>(loadPath + "maxhealth");
    //save base stats
    baseStats.maxHealth = ES2.Load<float>(loadPath + "maxhealth");
    baseStats.health = ES2.Load<float>(loadPath + "health");
    baseStats.strength = ES2.Load<int>(loadPath + "strength");
    baseStats.speed = ES2.Load<int>(loadPath +"speed");
    
    //save level info
    
    myLevelInfo.level =  ES2.Load<int>(loadPath  +"level");
    myLevelInfo.experience =  ES2.Load<int>(loadPath  +"experience");
    myLevelInfo.experienceTilNxtLvl =  ES2.Load<int>(loadPath +"experienceTilNxtLvl");
    
    
  }


}
