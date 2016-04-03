using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using InControl;

public class LevelManagerInitialization
{
  public List<string> ownedMonsters= new List<string>();
  public  int player1Mon=-1;
  public int player2Mon=-1;
  public int ownedGraveMiroks = 0;
}

public class LoadControls
{
public InputDevice firstPlayerInputDevice=null;
public InputDevice secondPlayerInputDevice=null;
public Controls firstPlayerControls=null;
public Controls secondPlayerControls=null;

}//used to pass controls between levels

public class LevelManager : MonoBehaviour 
{
protected bool firstPlayerSet=false;
public bool isCoOp=false;

//controls
protected InputDevice firstPlayerInputDevice=null;
protected InputDevice secondPlayerInputDevice=null;
protected Controls firstPlayerControls=null;
protected Controls secondPlayerControls=null;

//cameras
public GameObject singlePlayerCamera=null;
public GameObject[] splitScreenCameras= new GameObject[2];

public MissionManager myMissionManager;

public GameObject registerFirstPlayer;

public PlayMirok myPlayMirok;

  public void PlayMirok(MovieTexture newMovie)
  {
    myPlayMirok.Play( newMovie);
    
  }


virtual protected void SetUpCameras(){}
  virtual public void Die(int playerNum){}

virtual public void InitializeLevel(LevelManagerInitialization initialize){}

  virtual public void MeetLamina( string newLamina ){}
  virtual public bool ReviveLamina( ){return false;}

  //used to pass controls between scenes
public LoadControls GetLoadControls()
{
//    UnityEngine.Debug.Log("get load controls!");
	LoadControls controls=new LoadControls();
	controls.firstPlayerControls=firstPlayerControls;
	controls.firstPlayerInputDevice=firstPlayerInputDevice;

	controls.secondPlayerControls=secondPlayerControls;
	controls.secondPlayerInputDevice=secondPlayerInputDevice;
	return controls;
}

//used to pass controls between scenes
public void SetLoadControls(LoadControls cont)
{
    
  if(cont.firstPlayerInputDevice==null||cont.firstPlayerControls==null){return;}
    RegisterController(cont.firstPlayerInputDevice,cont.firstPlayerControls);
    
  //wierd repeating bug...this catches it but probably doesn't solve the actual problem
	if(cont.firstPlayerInputDevice==cont.secondPlayerInputDevice){return;}
	
	if(cont.secondPlayerInputDevice==null||cont.secondPlayerControls==null){return;}
	
  
    RegisterController(cont.secondPlayerInputDevice,cont.secondPlayerControls);

    
  
}

void FixedUpdate()
{

}



virtual public void RegisterController(InputDevice inputDevice,Controls newControls)
{
}

virtual public void UnRegisterController(int playerNum)
{
}

virtual public void PauseGame(int playerNum)
{

  }

virtual public void UnPauseGame(int playerNum)
{}






}
