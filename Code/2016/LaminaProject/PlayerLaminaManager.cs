using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using InControl;



public class PlayerLaminaManager : MonoBehaviour 
{
  [HideInInspector]
	public Transform myTransform;
  [HideInInspector]
  public GameObject myGameObject;

	
	Controls myControls;
	public int playerNum;

	public GraveLevelManager myLevelManager;
	public GameObject crystalState;
	public Transform parentPlayerLamina;



	GameObject playerLamina=null;

  LaminaBrain_Grave playerLaminaBrainScript=null;

	void Awake()
	{
		myTransform = this.transform;
    myGameObject = this.gameObject;
 	}

    public GUI_info GetGUIInfo()
  {
    GUI_info myInfo=new GUI_info();
    myInfo.name = playerLaminaBrainScript.myName;
    myInfo.myLevelInfo = playerLaminaBrainScript.myLevelInfo;
    myInfo.myPriorities = playerLaminaBrainScript.myPriorities;
    myInfo.myStats = playerLaminaBrainScript.currentStats;
    myInfo.myStatusEffects = playerLaminaBrainScript.myStatusEffects;
    return myInfo;
  }

  public Controls GetController()
  {
    return myControls;
  }
	public void SetActive(bool active,InputDevice inputDevice,Controls newControls,int newPlayerNum,GameObject newPlayerLamina)
	{
		if (active)
  {
    myControls = newControls;
    myControls.Device = inputDevice;
    playerNum = newPlayerNum;
    SwitchLamina(newPlayerLamina);
  }
  else
  {
      crystalState.SetActive(false);
  }

	}
 

	public void SwitchLamina(GameObject newPlayerLamina)
  {
    crystalState.SetActive(false);
		playerLamina = newPlayerLamina;
		playerLamina.transform.SetParent (parentPlayerLamina);
		playerLamina.transform.localPosition = new Vector3 (0, 0, 0);
		playerLamina.SetActive (true);
    playerLaminaBrainScript=playerLamina.GetComponent<LaminaBrain_Grave>();
    playerLaminaBrainScript.SetPlayerLaminaManager(this);
    playerLaminaBrainScript.SetActive(myControls.Device,myControls,playerNum);
	}

  public void RemoveLamina(Transform ownedLaminaParent)
  {
  

    playerLamina.transform.SetParent(ownedLaminaParent);
    playerLamina.SetActive(false);

  }

  public void EnterCyrstalState()
  { 
    RemoveLamina(myLevelManager.ownedLaminaParent);//remove lamina afterwards, so the equiped isn't reset before enter crystal state runs
    crystalState.SetActive(true);
    myLevelManager.EnterCrystalState(playerNum);
   
  }

  public void RewindPosition(Vector2 oldPosition)
  {
    myTransform.position = oldPosition;
  }
 
  public void FollowChild()
  {
    myTransform.position = playerLamina.transform.position;
    playerLamina.transform.localPosition = new Vector3(0, 0, 0);
  }
}
